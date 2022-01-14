using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaTask
{
    public static class MeetingController
    {
        public static void Login()
        {
            Console.Clear();
            bool exit = false;
            while (!exit)
            {
                string username = UI_Helper.AskForString("Prasome ivesti vartotojo varda : ");
                string password = UI_Helper.AskForString("Prasome ivesti slaptazodi : ");

                var user = DB.Users.Where(x => x.Name == username).FirstOrDefault();

                if (user == null)
                {
                    Console.Clear();
                    Console.WriteLine("Tokio vartotojo nera.");
                    continue;
                }

                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    Console.Clear();
                    DB.CurrentUser = user;
                    exit = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Neteisingas slaptazodis.");
                    continue;
                }
            }
        }

        public static void Register()
        {
            Console.Clear();
            bool exit = false;
            while (!exit)
            {
                string username = UI_Helper.AskForString("Prasome ivesti vartotojo varda : ");
                string password = UI_Helper.AskForString("Prasome ivesti slaptazodi : ");

                var user = DB.Users.Where(x => x.Name == username).FirstOrDefault();

                if (user != null)
                {
                    Console.Clear();
                    Console.WriteLine("Tokio vartotojas jau registruotas.");
                    continue;
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                var newUser = new User() { Name = username, Password = hashedPassword };

                DB.Users.Add(newUser);
                DB.SaveUsers();
                DB.CurrentUser = newUser;
                Console.Clear();
                exit = true;
            }
        }

        public static void Create()
        {
            Console.Clear();
            Console.WriteLine("Kuriamas susitikimas ...");
            var testas = new Meeting()
            {
                Name = "TestName",
                ResponsiblePerson = "TestPerson",
                Description = "TestDescription",
                Category = Category.Short,
                Type = Type.InPerson,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
            DB.Meetings.Add(testas);
            DB.SaveChanges();
        }

        public static void Delete()
        {
            Console.Clear();
            Console.WriteLine("Trinamas susitikimas ...");
        }

        public static void AddPerson()
        {
            Console.Clear();
            Console.WriteLine("Pridedamas zmogus i susitikima ...");
        }

        public static void RemovePerson()
        {
            Console.Clear();
            Console.WriteLine("Pasalinamas zmogus is susitikimo ...");

        }

        public static void GetAll()
        {
            Console.Clear();
            Console.WriteLine("Rodomi visi susitikimai ...");
        }

    }
}
