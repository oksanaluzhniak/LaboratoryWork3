using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace l3
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Login { get; set; }
        //public string Password { get; set; }
        //public string Role { get; set; }
        public string DateofBirth { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        public string Phone { get; set; }
        public string City { get; set; }
        public string Adrress { get; set; }
        public string Country { get; set; }
        public string Sex { get; set; }
        //public bool IsFemale { }
        public string Age
        {
            get

            {
                var dateTime = DateTime.Parse(this.DateofBirth);
                int age = (DateTime.Today.Year - dateTime.Year);
                if (DateTime.Today.DayOfYear < dateTime.DayOfYear)
                {
                    age--;
                }
                return age.ToString();
            }
        }
    }
}
