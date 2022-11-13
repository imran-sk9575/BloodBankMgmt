using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace bloodbank.Models
{
    public class loginDetail
    {
        [Required(ErrorMessage = "User Name is required")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }

    public class RegistrationDetails
    {
        [Required(ErrorMessage = "User Name is required")]
        public string username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
         
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        public string CPassword { get; set; }
    }

    public class CenterDetails
    {
        public string id { get; set; }
        public DataTable dt { get; set; }
    }

    public class DonerDetails
    {
        public string DonateDate { get; set; }
        public string Unit { get; set; }
        public string Reason { get; set; }
        public string BloodGrop { get; set; }
        public string Supervisor { get; set; }
        public DataTable dt { get; set; }
        public string id { get; set; }
    }

    public enum Units
    {
        One, Two, Three, Four
    }
}