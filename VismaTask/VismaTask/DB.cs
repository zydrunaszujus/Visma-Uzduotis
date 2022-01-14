using Newtonsoft.Json;

namespace VismaTask
{
    public static class DB
    {
        const string DATA_FILE_NAME = "duomenys.json";

        const string USER_FILE_NAME = "vartotojai.json";

        public static List<Meeting> Meetings = new List<Meeting>();

        public static List<User> Users = new List<User>();

        public static User CurrentUser = null;

        public static void SaveChanges()
        {
            var textData = JsonConvert.SerializeObject(Meetings);
            File.WriteAllText(DATA_FILE_NAME, textData);
        }

        // JsonConvert.SerializeObject - vercia objekta (arba lista) i teksta
        // JsonConvert.DeserializeObject - vercia teksta i objekta.

        public static void Load()
        {
            if (File.Exists(DATA_FILE_NAME))
            {
                var textDataFromFile = File.ReadAllText(DATA_FILE_NAME);
                var objectData = JsonConvert.DeserializeObject<List<Meeting>>(textDataFromFile);
                Meetings = objectData;
            }
        }

        public static void SaveUsers()
        {
            var textData = JsonConvert.SerializeObject(Users);
            File.WriteAllText(USER_FILE_NAME, textData);
        }

        // JsonConvert.SerializeObject - vercia objekta (arba lista) i teksta
        // JsonConvert.DeserializeObject - vercia teksta i objekta.

        public static void LoadUsers()
        {
            if (File.Exists(USER_FILE_NAME))
            {
                var textDataFromFile = File.ReadAllText(USER_FILE_NAME);
                var objectData = JsonConvert.DeserializeObject<List<User>>(textDataFromFile);
                Users = objectData;
            }
        }
    }
}
