using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using customer_api.Model;
using AutoMapper;
using customer_api.Service;
using customer_api.ViewModel;
using System.Collections.Generic;
using customer_api.Responses;
using customer_api.Filters;
using customer_api.Helpers;
using System;
using Microsoft.AspNetCore.Authorization;
using customer_api.JWT;
using Swashbuckle.AspNetCore.Annotations;

namespace customer_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IJWTAuthManager _jWTAuthManager;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly CustomerService _customerService;

        public CustomerController(IMapper mapper, CustomerService customerService,IUriService uriService, IJWTAuthManager jWTAuthManager)
        {
            _mapper = mapper;
            _customerService = customerService;
            _uriService = uriService;
            _jWTAuthManager = jWTAuthManager;
        }
        [AllowAnonymous]
        [HttpPost("Auth")]
        [SwaggerOperation(Summary = "Get Access Token", Description = "Username:'user1' | Password:'pass1'")]
        public IActionResult AuthUser([FromQuery] UserAuth userAuth)
        {
            var token = _jWTAuthManager.Auth(userAuth.UserName, userAuth.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }
        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult> GetCustomers([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _customerService.GetAll(validFilter.PageNumber, validFilter.PageSize);
            var customerViewModel = _mapper.Map<List<CustomerViewModel>>(pagedData);
            var totalRecords = await _customerService.TotalRecord();
            var pagedReponse = PaginationHelper.CreatePagedReponse<CustomerViewModel>(customerViewModel, validFilter, totalRecords, _uriService, route);


            return Ok(pagedReponse);
        }
        

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomer(int id)
        {
            var customer = await _customerService.Get(id);

            if (customer == null)
            {
                return NotFound(new Response<CustomerViewModel>(null) { Message = "Record can't found!", Succeeded=false });
            }
            var customerViewModel = _mapper.Map<CustomerViewModel>(customer);
            var response = new Response<CustomerViewModel>(customerViewModel){Message="Found", Succeeded = true};
            return Ok(response);
        }
        [HttpGet("search/{keyword}")]
        public async Task<ActionResult> Search(string keyword)
        {
            var list = await _customerService.Search(keyword);
            if (list.Count == 0)
            {
                return NotFound(new Response<List<CustomerViewModel>>(null) { Message = "0 Result", Succeeded = false });
            }
            var viewList = _mapper.Map<List<CustomerViewModel>>(list);
            var response = new Response<List<CustomerViewModel>>(viewList) { Message = $"{list.Count} Results Found", Succeeded = true };
            return Ok(response);
        }
        
        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.Id)
            {
                return BadRequest(new Response<string>(null) { Message = "Model and Id dont'match", Succeeded = false, Errors = new[] { "Bad Request" } });
            }
            var customer = _mapper.Map<Customer>(customerViewModel);         
            bool success= await _customerService.Update(id, customer);
            if (!success)
            {
                return NotFound(new Response<string>(null) { Message = "Record Not Found", Succeeded = false, Errors = new[]{"Not Found"}});
            }

            return NoContent();
        }
        
        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult> PostCustomer(CustomerViewModel customerViewModel)
        {
            var customer = _mapper.Map<Customer>(customerViewModel);
            int id = await _customerService.Add(customer);
            customerViewModel.Id = id;
            return CreatedAtAction("GetCustomer",new { id=id }, new Response<CustomerViewModel>(customerViewModel));
        }
        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            bool value = await _customerService.Delete(id);
            if (!value)
            {
                return NotFound(new Response<string>(null) { Message = "Record Not Found", Succeeded = false, Errors = new[] { "Not Found" } });
            }
            return NoContent();
        }
    }
}
