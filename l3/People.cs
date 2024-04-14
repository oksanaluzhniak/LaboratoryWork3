using l3;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace l3
{
    public class People
    {
        static JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        static string filename = "D:\\Practice\\Diplom\\l3\\person.txt";

        public List<Person> DataPeople { get; set; }
        public People()
        {
            LoadData(filename);
        }

        public void LoadData(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            if (!fi.Exists)
            {
                FileStream fs = fi.Create();
                fs.Close();
            }
            using (StreamReader fileObj = new StreamReader(filename))
            {
                string datapeople = fileObj.ReadToEnd();
                try
                {
                    this.DataPeople = JsonSerializer.Deserialize<List<Person>>(datapeople);
                }
                catch (Exception)
                {

                    this.DataPeople = new List<Person>();
                }
            }
        }
        public void SaveData(List<Person> people)
        {
            string text = JsonSerializer.Serialize(people, options);
            using (StreamWriter fileObj = new StreamWriter(filename))
            {
                fileObj.Write(text);
            }
        }
        public Person SearchByEmail(string email)
        {
            Person persona = this.DataPeople.Where(p => p.Email == email).FirstOrDefault();
            return persona;
        }
        public List<Person> FilterBySex(string sex)
        {
            List<Person> persona = new List<Person>(this.DataPeople.Where(p => p.Sex == sex));
            return persona;
        }
        public bool AddPerson(Person person)
        {
            bool answer = true;
            if (ValidatePhone(person) == true)
            {
                if (SearchByEmail(person.Email) == null)
                {
                    this.DataPeople.Add(person);
                    SaveData(this.DataPeople);
                }
                else
                {
                    int j = this.DataPeople.IndexOf(SearchByEmail(person.Email));
                    this.DataPeople[j] = person;
                    SaveData(this.DataPeople);
                }
                return answer;
            }
            else { return answer = false; }
        }
        public void DeletePerson(string email)
        {
            int i = this.DataPeople.IndexOf(SearchByEmail(email));
            this.DataPeople.RemoveAt(i);
            SaveData(this.DataPeople);
        }
        public bool ValidatePhone(Person person)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(person);
            bool validated = false;
            if (Validator.TryValidateObject(person, context, results, true))
            {
                validated = true;
            }
            return validated;
        }
    }
}
