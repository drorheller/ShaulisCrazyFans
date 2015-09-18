using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShaulisCrazyFans.Models
{
    public enum Gender
    {
        Male,
        Female,
        Other,
        IDontLikeDefinitions,
        NoneOfTheAbove
    }
    public class CrazyFan
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string City { get; set; }

        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [DisplayName("Time in club")]
        public double TimeInClub { get; set; }

    }
}