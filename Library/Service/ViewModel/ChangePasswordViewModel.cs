using Domain.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModel
{
    public class ChangePasswordViewModel
    {
        public String Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "Password")]
        [MaxLength(20, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MaxLengthPassword")]
        [MinLength(3, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MinLengthPassword")]
        public string NewPassword { get; set; }
        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "ComparePassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "ComparePasswordError")]
        public string ConfirmPassword { get; set; }
    }
}
