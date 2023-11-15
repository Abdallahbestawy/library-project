using Domain.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "RegisterEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "RegisterEmailError")]
        public string Eamil { get; set; }
        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "Password")]
        public string Password { get; set; }
        public bool RememberMy { get; set; }
    }
}
