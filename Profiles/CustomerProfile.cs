using AutoMapper;
using customer_api.Model;
using customer_api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customer_api.Profiles
{
    public class CustomerProfile:Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerViewModel>().
                ForMember(dest =>
                    dest.mobile_phone,
                    opt => opt.MapFrom(src => src.phone1)).
                ForMember(dest=>dest.name,
                    opt=>opt.MapFrom(src=>src.first_name)).
                ReverseMap();
        }
    }
}
