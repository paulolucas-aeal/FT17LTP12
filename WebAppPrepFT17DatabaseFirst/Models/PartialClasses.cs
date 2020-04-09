using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppPrepFT17DatabaseFirst.Models
{
    [MetadataType(typeof(EmployeesMetadata))]
    public partial class Employees
    {
    }

    [MetadataType(typeof(UsersMetadata))]
    public partial class Users
    {
    }

    [MetadataType(typeof(DepartmentsMetadata))]
    public partial class Departments
    {
    }
}

