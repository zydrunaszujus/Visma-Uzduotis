using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaTask
{
    public static class MeetingController
    {
        public static string[] FILTER_VARIANTS =
        {
            "Filtras pagal aprasyma",
            "Filtras pagal atsakinga asmeni",
            "Filtras pagal kategorija",
            "Filtras pagal tipa",
            "Filtras pagal data",
            "Filtras pagal dalyviu skaicius"
        };

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

                var newUser = new User() { Id = DB.Index.NextUserId++, Name = username, Password = hashedPassword };

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
            var meeting = new Meeting()
            {
                Name = UI_Helper.AskForString("Iveskite susitikimo pavadinima : "),
                ResponsiblePersonId = DB.CurrentUser.Id,
                Description = UI_Helper.AskForString("Iveskite susitikimo aprasyma : "),
                Category = (Category)UI_Helper.AskForSelection(Enum.GetNames(typeof(Category)),"Pasirinkite susitikimo kategorija : "),
                Type = (Type)UI_Helper.AskForSelection(Enum.GetNames(typeof(Type)), "Pasirinkite susitikimo tipa : "),
                StartDate = UI_Helper.AskForDate("Iveskite susitikimo pradzios data : "),
                EndDate = UI_Helper.AskForDate("Iveskite susitikimo pabaigos data : ")
            };
            meeting.People.Add(DB.CurrentUser);
            DB.Meetings.Add(meeting);
            DB.SaveChanges();
            Console.Clear();
            Console.WriteLine("Susitikimas sukurtas !");
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
            var screen = DB.Meetings;
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Rodomi susitikimai ...");
                Console.WriteLine("Esc - baigti");
                Console.WriteLine("f - filtruoti");
                screen.ForEach(x => Console.WriteLine(x));
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape) exit = true;
                if (key.Key == ConsoleKey.F)
                {
                    var selection = UI_Helper.AskForSelection(FILTER_VARIANTS, "Pasirinkite filtra : ");
                    var text = UI_Helper.AskForString("Iveskite filtro teksta : ");

                    switch (selection)
                    {
                        case 0:
                            screen = screen.Where(x => x.Description.Contains(text)).ToList();
                            break;
                        case 1:
                            screen = screen.Where(x => x.People.Select(x => x.Name).Contains(text)).ToList();
                            break;
                        case 2:
                            screen = screen.Where(x => x.Category.ToString().Contains(text)).ToList();
                            break;
                        case 3:
                            screen = screen.Where(x => x.Type.ToString().Contains(text)).ToList();
                            break;
                        case 4:
                            screen = screen.Where(x => x.StartDate.ToShortDateString().Contains(text) || x.EndDate.ToShortDateString().Contains(text)).ToList();
                            break;
                        default:
                            var count = 0;
                            if(int.TryParse(text, out count))
                            {
                                screen = screen.Where(x => x.People.Count >= count).ToList();
                            }
                            break;
                    }
                }
            }
            
        }

    }
}
