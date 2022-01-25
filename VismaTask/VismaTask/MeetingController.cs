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
                Id=DB.Index.NextMeetingId++,//skaiciuos susitikimu skaiciu
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
            var variants=DB.Meetings.Select(x=>x.Name).ToArray();
            var selection=UI_Helper.AskForSelection(variants,"Pasirinkite, kuri susirinkima norite istrinti")
           var meeting=DB.Meetings[selection];//parinks meetingaa,kuri mes apsirinkom
            if (meeting.ResponsiblePersonId == DB.CurrentUser.Id)
            {
                DB.Meetings.Remove(meeting);
                DB.SaveChanges();
                Console.Clear();
                Console.WriteLine("Susitikimas {0} istrintas ! ",meeting.Name);
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Susitikimo {0} istrinti negalima. Nes nesate jo savininkas ! ",meeting.Name);
                Console.ReadKey();
            }

        }

        public static void AddPerson()
        {
            Console.Clear();
            var variantsMeeting=DB.Meetings.Select(x=>x.Name).ToArray();
            var selectionMeeting=UI_Helper.AskForSelection(variantsMeeting,"Pasirinkite susitikima : ");//pasirenkkam meetinga
            var meeting=DB.Meetings[selectionMeeting];
            var variantsUser=DB.Users.Select(x=>x.Name).ToArray();
            var selectionUser=UI_Helper.AskForSelection(variantsUser,"Pasirinkite prededama zmogu : ");//pasirenkam zmogu
            var user=DB.Users[selectionUser];

            var intersects=DB.Meetings.Where(x=>x.People.Contains(user)&&meeting.Between(x.SartDate,x.EndDate)); //tikrinama ar meetingas nera tarp kitu meetingu
            var intersects=DB.Meetings.

            if (!meeting.People.Contains(user))//rodo kokie susitikime yra zmones
	        {//jei persikirs mes warning
                Console.Clear();
                var key =ConsoleKey.Y;
                if (intersects.Count>0)
	            {
                    foreach (var item in intersects)
                    {
                        Console.WriteLine("Pilietis jau turi {0}tuo metu, bandykite kitu metu.",item.Name);
                    }
                    Console.WriteLine("Ar norite testi? Y?N");
                    key=Console.ReadKey().Key;//kad isgaut key

	            }
                if (key==ConsoleKey.Y)//noresim prideti zmaogu
	            {
                    meeting.People.Add(user);//pridedam zmogu
                    Console.Clear();
                    Console.WriteLine("Pridedamas vartotojas {0} prie susitikimo {1} laiku {2} .",user.Name,meeting.Name,meeting.StartDate);
	                Console.ReadKey();//kad palauktu,ne iskarto verstu.

                }

	        }
            else
            {
                Console.Clear();
                Console.WriteLine("Zmogus {0} pridetas i susirinkima {1}",user.Name,meeting.Name);
                Console.ReadKey();
            }
            
        }

        public static void RemovePerson()
        {
            Console.Clear();
            var variantsMeeting=DB.Meetings.Select(x=>x.Name).ToArray();
            var selectionMeeting=UI_Helper.AskForSelection(variantsMeeting,"Pasirinkite susitikima : ");//pasirenkkam meetinga
            var meeting=DB.Meetings[selectionMeeting];
            //               zmones is meeting saraso
            var variantsUser=meeting.People.Select(x=>x.Name).ToArray();
            var selectionUser=UI_Helper.AskForSelection(variantsUser,"Pasirinkite pasalinama zmogu : ");//pasirenkam zmogu
            var user=meeting.People[selectionUser];

            
            if (meeting.ResponsiblePersonId != user.Id)//lyginam ar norimas istrinti asmuo nera susitikima sukures zmogelis.Ar nelygu
            {
                meeting.People.Remove(user);//tuomet istrinam useri
                DB.SaveChanges();
                Console.WriteLine("Zmogus {0} pasalintas is susitikimo {1}",user.Nmae,meeting.Name);
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Negalima pasalinti susitikimo kurejo");
                Console.ReadKey();

            }

        }

        public static void GetAll()
        {
            var screen = DB.Meetings;
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Rodomi susitikimai ...");
                Console.WriteLine("X - baigti");
                Console.WriteLine("F - filtruoti");
                screen.ForEach(x => Console.WriteLine(x));
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.X) exit = true;
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
        //                                       kitu susitikimu pradzia ir pabaiga
        public  static bool Between(this Meeting current,DateTime start,DateTime end)//current(dabartinnis)-data kuria tikrinsim
        {
            bool startCheck=start<=current.StartDate && current.StartDate<=end;//tikrinam kvieciamo curent pradzia ar nera kito susitikimo tarpe
            bool endCheck=start<=current.EndDate&&current.EndDate<=end;//tikrinam kvieciamo curent galas ar nera kito susitikimo tarpe
            return startCheck||endCheck;
            //curent dabartinis meetingas, i kuri norima pakviesti
        }


    }
}
