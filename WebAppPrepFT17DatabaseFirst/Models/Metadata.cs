using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppPrepFT17DatabaseFirst.Models
{
    public class EmployeesMetadata
    {
        
        public int Id { get; set; }

        [Required]

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Morada")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime DateOfBirth { get; set; }

        [Display(Name = "Salário")]
        [DataType(DataType.Currency)]
        [Range(0, 99999.99, ErrorMessage = "Salário acima do limite")]
        public decimal Salary { get; set; }

        [Required]
        [Display(Name = "Função")]
        public string Worktype { get; set; }

        [Required]
        [Display(Name = "N. Identificação Fiscal")]
        [RegularExpression(@"\d{9}", ErrorMessage = "O formato do NIF deverá ser composto por 9 dígitos: xxxxxxxxxx")]
        [Remote("IsUniqueNIF", "Employees", AdditionalFields = "Id", ErrorMessage = "Remote Validation: O NIF já se encontra registado no sistema!")]
        public string NIF { get; set; }
    }

    public class UsersMetadata
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class DepartmentsMetadata
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Departamento")]

        [Remote("IsUniqueDeptName", "Departments", AdditionalFields = "Id", ErrorMessage = "O Nome do Departamento já se encontra registado no sistema!")]
        
        public string DeptName { get; set; }
    }
}

