using System;


//Lista cu nivelele de prioritate posibile ale unei activitati
public enum PriorityLevel
{
    Low, Medium, High
}

namespace DailyActivitiesApp
{
    public class Activity
    {
        //Stocheaza numele activitatii
        public string ActivityName { get; set; }
        //Stocheaza informatii suplimentare referitoare la activitate
        public string Description { get; set; }
        //Stocheaza data si ora la care are loc activitatea 
        public DateTime DateAndTime { get; set; }
        //Stocheaza nivelul de prioritate al activitatii
        public PriorityLevel Priority { get; set; }
        //Verifica daca aplicatia este finalizata
        public bool IsFinished { get; set; }

        //Constructor
        public Activity(string _ActivityName, string _Description, DateTime _DateAndTime, PriorityLevel _Priority)
        {
            ActivityName = _ActivityName;
            Description = _Description;
            DateAndTime = _DateAndTime;
            Priority = _Priority;
            IsFinished = false; 
        }

        //Metoda pentru marcarea unei activitati ca finalizata
        public void MarkAsFinished()
        {
            IsFinished = true; 
        }

        public string Info()
        {
            return  $"Activity: {ActivityName}, \nDescription: {Description}, \nTime: {DateAndTime}, \nPriority: {Priority}, \nFinished: {IsFinished}"; 
        }

    }
}
