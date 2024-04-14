using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace l3
{
    public class Users
    {
        static JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        static string filename = "D:\\Practice\\Diplom\\l3\\credentials.txt";
        public List<User> DataUsers { get; set; }
        public Users()
        {
            LoadUsers(filename);
        }
        public void LoadUsers(string filename)
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
                    this.DataUsers = JsonSerializer.Deserialize<List<User>>(datapeople);
                }
                catch (Exception)
                {

                    this.DataUsers = new List<User>();
                }
            }
        }
        public void SaveUsers(List<User> user)
        {
            string text = JsonSerializer.Serialize(user, options);
            using (StreamWriter fileObj = new StreamWriter(filename))
            {
                fileObj.Write(text);
            }
        }
        public bool AddUser(User user)
        {
            bool answer = true;
            if (CheckLogin(user.Login) == null)
            {
                this.DataUsers.Add(user);
                SaveUsers(this.DataUsers);
                answer = true;
            }
            else
            {
                int j = this.DataUsers.IndexOf(CheckLogin(user.Login));
                this.DataUsers[j] = user;
                SaveUsers(this.DataUsers);
                answer = false;
            }
            return answer;
        }
        public User CheckLogin(string login)
        {
            User user = this.DataUsers.Where(u => u.Login == login).FirstOrDefault();

            return user;
        }
        public User CheckPassword(string password)
        {
            User user = this.DataUsers.Where(u => u.Password == password).FirstOrDefault();
            return user;
        }
    }
}