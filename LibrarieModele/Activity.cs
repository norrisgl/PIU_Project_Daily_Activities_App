using System;
using System.Xml.Linq;


//Lista cu nivelele de prioritate posibile ale unei activitati
public enum PriorityLevel
{
    Low, Medium, High
}

//Lista cu tipurile de activitati posibile ale unei activitati
[Flags]
public enum ActivityType
{
    None = 0,
    Work = 1,
    Study = 2,
    FreeTime = 4,
    Shopping = 8,
    Sport = 16,
    Other = 32
}

namespace LibrarieModele
{

    public class Activity
    {
        private const char MAIN_FILE_SEPARATOR = ';';
        //private const char SECONDARY_FILE_SEPARATOR = ' ';


        //Stocheaza numele activitatii
        public string ActivityName { get; set; }
        //Stocheaza informatii suplimentare referitoare la activitate
        public string Description { get; set; }
        //Stocheaza data si ora la care are loc activitatea 
        public DateTime DateAndTime { get; set; }
        //Stocheaza nivelul de prioritate al activitatii
        public PriorityLevel Priority { get; set; }
        //Stocheaza tipul activitatii
        public ActivityType ActType { get; set; }
        //Verifica daca aplicatia este finalizata
        public bool IsFinished { get; set; }

        //Constructor
        public Activity(string _ActivityName, string _Description, DateTime _DateAndTime, PriorityLevel _Priority, ActivityType actType)
        {
            ActivityName = _ActivityName;
            Description = _Description;
            DateAndTime = _DateAndTime;
            Priority = _Priority;
            ActType = actType;  
            IsFinished = false;
        }

        // Constructor pentru citire din fisier
        public Activity(string linieFisier)
        {
            string[] dateFisier = linieFisier.Split(MAIN_FILE_SEPARATOR);

            ActivityName = dateFisier[0];
            Description = dateFisier[1];
            DateAndTime = DateTime.Parse(dateFisier[2]);
            Priority = (PriorityLevel)Enum.Parse(typeof(PriorityLevel), dateFisier[3]);
            if (int.TryParse(dateFisier[4], out int actTypeValue))
            {
                ActType = (ActivityType)actTypeValue;
            }
            else
            {
                ActType = (ActivityType)Enum.Parse(typeof(ActivityType), dateFisier[4]);
            }

            IsFinished = bool.Parse(dateFisier[5]);
        }

        //Metoda pentru marcarea unei activitati ca finalizata
        public void MarkAsFinished()
        {
            IsFinished = true; 
        }

        public string Info()
        {
            return  $"Activity: {ActivityName}; \nDescription: {Description}; \nTime: {DateAndTime}; \nPriority: {Priority}; \nType: {ActType}; \nFinished: {IsFinished}."; 
        }


        public string FileConverter()
        {
            string fileObject = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                MAIN_FILE_SEPARATOR,
                ActivityName ?? "NECUNOSCUT",
                Description ?? "NECUNOSCUT",
                DateAndTime.ToString() ?? "NECUNOSCUT",
                Priority.ToString() ?? "NECUNOSCUT",
                ((int)ActType).ToString(),
                IsFinished.ToString() ?? "Necunoscut");

            return fileObject;
        }
    }
}
