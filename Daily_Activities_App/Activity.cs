using System;


//Lista cu nivelele de prioritate posibile ale unei activitati
public enum priority_level
{
    Low, Medium, High
}

namespace Daily_Activities_App
{
    public class Activity
    {
        //Stocheaza numele activitatii
        public string Activity_name { get; set; }
        //Stocheaza informatii suplimentare referitoare la activitate
        public string Description { get; set; }
        //Stocheaza data si ora la care are loc activitatea 
        public DateTime Date_time { get; set; }
        //Stocheaza nivelul de prioritate al activitatii
        public priority_level Priority { get; set; }
        //Verifica daca aplicatia este finalizata
        public bool Is_finished { get; set; }

        //Constructor
        public Activity(string _activity_name, string _Description, DateTime _Date_time, priority_level _Priority)
        {
            Activity_name = _activity_name;
            Description = _Description;
            Date_time = _Date_time;
            Priority = _Priority;
            Is_finished = false; 
        }

        //Metoda pentru marcarea unei activitati ca finalizata
        public void MarkAsFinished()
        {
            Is_finished = true; 
        }

        public string Info()
        {
            return  $"Activity: {Activity_name}, \nDescription: {Description}, \nTime: {Date_time}, \nPriority: {Priority}, \nFinished: {Is_finished}"; 
        }

    }
}
