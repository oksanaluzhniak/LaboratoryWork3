using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace l3.Controllers
{
    public class APIController : Controller
    {
        //public static People people = new People();
        //public static JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

        private readonly People _people;

        public APIController(People people)
        {
            _people = people;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return Ok("Laboratory work 3");
        }

        
        [Route("/listofpeople")]
        public IActionResult ListOfPeople()
        {
            if (_people != null)
            {
                return Json(_people.DataPeople);
            }
            else
            {
                return Ok("Missing data");
            }
        }
        
        [Authorize]
        [HttpGet("/searchbyemail")]
        public IActionResult SearchByEmail()
        {
            string email = Request.Query["email"];
            Person person = _people.SearchByEmail(email);
            if (person != null)
            {

                return Json(_people.SearchByEmail(email));
            }
            else
            {
                return Json(new { Error = "Email not found" });
            }
        }
        [Authorize]
        [HttpGet("/filterbysex")]
        public IActionResult FilterBySex()
        {
            string sex = Request.Query["sex"];
            List<Person> listperson = _people.FilterBySex(sex);
            if (listperson.Count != 0)
            {
                return Json(_people.FilterBySex(sex));
            }
            else
            {
                return Json(new { Error = "Sex not found" });
            }
        }
        [Authorize]
        [HttpDelete("/deleteperson")]
        public IActionResult DeletePerson()
        {
            string email = Request.Query["email"];
            string name = _people.SearchByEmail(email).FirstName;
            _people.DeletePerson(email);
            return Json(new { Status = $"Person {name} is succesful deleted" });
        }
        [Authorize]
        [HttpPost("/addperson")]
        public async Task<IActionResult> AddPerson()
        {

            var request = Request.Body;
            string body;
            using (StreamReader reader = new StreamReader(request))
            {
                body = await reader.ReadToEndAsync();
            }
            List<Person> form = JsonSerializer.Deserialize<List<Person>>(body);
            int count = form.Count;
            int countnames = 0;
            for (int i = 0; i < count; i++)
            {

                bool ans = _people.AddPerson(form[i]);
                if (ans == true)
                {
                    countnames++;
                }

            }
            return Json(new { Status = $"Added {countnames} people" });

        }
    }
}
