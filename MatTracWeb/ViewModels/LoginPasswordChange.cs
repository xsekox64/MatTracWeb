using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels
{
    public class LoginPasswordChange
    {
        [Required(ErrorMessage = "E-Postanıza Gelen Kod Alanı Boş Geçilemez !!!")]        
        public string ecode { get; set; }
        [Required(ErrorMessage = "Password Alanı Boş Geçilemez !!!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 Karater Girilmelidir !!!")]
        public string passwordagain { get; set; }
    }
}