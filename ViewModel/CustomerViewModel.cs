using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace customer_api.ViewModel
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(24)]
        public string name { get; set; }
        [Required]
        [MaxLength(24)]
        public string city { get; set; }
        [Required]
        [MaxLength(10)]
        [MinLength(10)]
        public string mobile_phone { get; set; }
        [Required]
        [MaxLength(24)]
        public string email { get; set; }

    }
}
