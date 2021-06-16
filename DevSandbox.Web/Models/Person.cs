using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevSandbox.Web.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Display date data field in the short format 11/12/08.
        // Also, apply format in edit mode.
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? BirthDate { get; set; }
        public int NumberOfKids { get; set; }
        // Display date data field in the short format 11/12/08.
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CreatedDate { get; set; }
    }
}