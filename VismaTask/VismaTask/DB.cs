using Newtonsoft.Json;

namespace VismaTask
{
    public static class DB
    {
        const string FILE_NAME = "duomenys.json";

        public static List<Meeting> Meetings = new List<Meeting>();

        public static void SaveChanges()
        {
            var textData = JsonConvert.SerializeObject(Meetings);
            File.WriteAllText(FILE_NAME, textData);
        }

        // JsonConvert.SerializeObject - vercia objekta (arba lista) i teksta
        // JsonConvert.DeserializeObject - vercia teksta i objekta.

        public static void Load()
        {
            if (File.Exists(FILE_NAME))
            {
                var textDataFromFile = File.ReadAllText(FILE_NAME);
                var objectData = JsonConvert.DeserializeObject<List<Meeting>>(textDataFromFile);
                Meetings = objectData;
            }
        }
    }
}
