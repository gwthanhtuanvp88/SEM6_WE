using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClaimReport.Models
{
    public class UserModel
    {
        public int id { get; set; }

        [Display(Name = "User Name*")]
        [Required(ErrorMessage = "The user name field is required!")]
        [StringLength(50)]
        public string username { get; set; }

        [Display(Name = "Password*")]
        [DataType(DataType.Password)]
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email*")]
        public string email { get; set; }

        public string phone { get; set; }

        public string address { get; set; }

        [Required]
        [Display(Name = "Type*")]
        public int usertypeid { get; set; }

        [Display(Name = "Faculty")]
        public string facultyId { get; set; }
    }

    public class AcademyYearModel
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Name*")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true )]
        [Display(Name = "Start Report Date*")]
        public DateTime startReportDate { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Closure Report Date*")]
        public DateTime closureReportDate { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "closure Evidence Date*")]
        public DateTime closureEvidenceDate { get; set; }        
    }

    public class AssessmentModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Academy Year")]
        public int? academyeYearId { get; set; }
    }

    public class ItemModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Start Report Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime startReportDate { get; set; }

        [Required]
        [Display(Name = "Closure Report Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime closureReportDate { get; set; }

        [Required]
        [Display(Name = "Closure Evidence Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime closureEvidenceDate { get; set; }

        [Required]
        [Display(Name = "Assessment")]
        public int? assessmentID { get; set; }
    }
}