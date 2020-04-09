using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppPrepFT17DatabaseFirst.Models
{
    public class UserRegisterView
    {
        public int Id { get; set; }

        [Required]
        [Remote("IsUniqueEmail", "Accounts", ErrorMessage = "O email já se encontra registado!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[!@#$%^&*_<>?|])(?=.*[a-z])(?=.*[A-Z])[A-Za-z\d][A-Za-z\d!@#$%^&*_<>?|]{6,}[A-Za-z\d]$", ErrorMessage = "a. Should start and end with an alphanumeric character. b. At least 1 uppercase letter. c. At least 1 lower case letter. d. At least 1 Number. e. At least 1 special character from the list(!@#$%^&*_<>?|). f. Minimum 8 characters length.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "As passwords não coincidem!")]
        public string ConfirmPassword { get; set; }
    }
}