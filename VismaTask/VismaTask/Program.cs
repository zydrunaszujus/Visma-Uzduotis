using VismaTask;
DB.Load();
DB.LoadUsers();


bool exit = false;
int selection;

var loginVariants = new string[]
{
    "Prisijungti",
    "Registruotis"
};

var variants = new string[]
{
    "Prideti nauja susitikima",
    "Istrinti susitikima",
    "Prideti zmogu prie susitikimo",
    "Isimti zmogu is susitikimo",
    "Perziureti visus susitikimus",
    "Baigti programa"
};


selection = UI_Helper.AskForSelection(loginVariants);

switch (selection)
{
    case 0:
        MeetingController.Login();
        break;
    default:
        MeetingController.Register();
        break;
}


while (!exit)
{
    selection = UI_Helper.AskForSelection(variants);

    switch (selection)
    {
        case 0: MeetingController.Create();
            break;
        case 1: MeetingController.Delete();
            break;
        case 2: MeetingController.AddPerson();
            break;
        case 3: MeetingController.RemovePerson();
            break;
        case 4: MeetingController.GetAll();
            break;
        default: Console.Clear();
            exit = true;
            break;
    }
}

























/*
var selections = new string[]
{
    "Prideti nauja susitikima",
    "Istrinti susitikima",
    "Prideti zmogu prie susitikimo",
    "Isimti zmogu is susitikimo",
    "Perziureti visus susitikimus",
    "Baigti programa"
};

var actions = new Action[]
{
    MeetingController.Create,
    MeetingController.Delete,
    MeetingController.AddPerson,
    MeetingController.RemovePerson,
    MeetingController.GetAll,
    () => { Console.Clear(); exit = true; }
};


while (!exit)
{
    UI_Helper.UniversalSelectPrompt(selections, actions);
}
*/
