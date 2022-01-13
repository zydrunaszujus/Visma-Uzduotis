using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaTask
{
    public static class MeetingController
    {
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
