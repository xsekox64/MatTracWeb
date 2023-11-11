using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels
{
    public class LoginUserControl
    {
        [Required(ErrorMessage = "E-Posta Alanı Boş Geçilemez !!!")]
        [EmailAddress(ErrorMessage = "Geçersiz E-Posta Adresi")]
        public string email { get; set; }
        [Required(ErrorMessage = "Password Alanı Boş Geçilemez !!!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 Karater Girilmelidir !!!")]
        public string password { get; set; }
    }
}