using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HW_1.Models.DAL;

namespace HW_1.Models
{
    public class User
    {
        int id;
        string name;
        string lastName;
        string email;
        string password;
        string cellphone;
        string gender;
        int yearBirth;
        string genre;
        string address;
        bool isAdmin;

        

        public User()
        {

        }
        public User(int id ,string name, string lastName, string email, string password, string cellphone, string gender, int yearBirth, string genre, string address,bool isAdmin)
        {
            this.Id = Id;
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.Password = password;
            this.Cellphone = cellphone;
            this.Gender = gender;
            this.YearBirth = yearBirth;
            this.Genre = genre;
            this.Address = address;
            this.isAdmin = isAdmin;
        }

        public string Name { get => name; set => name = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Cellphone { get => cellphone; set => cellphone = value; }
        public string Gender { get => gender; set => gender = value; }
        public int YearBirth { get => yearBirth; set => yearBirth = value; }
        public string Genre { get => genre; set => genre = value; }
        public string Address { get => address; set => address = value; }
        public int Id { get => id; set => id = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }

        public List<User> GetUsers()
        {
            DataServices DB = new DataServices();
            return DB.GetUsers();
        }
        public int InsertUser()
        {
            Console.WriteLine("InsertUser - user.cs step 2");

            DataServices ds = new DataServices();
            ds.InsertUserDS(this);
            return 1;
        }

        public int InsertUserToSQL()
        {
            DataServices ds = new DataServices();
            ds.InsertToSQL(this);
            return 1;
        }
    }
}