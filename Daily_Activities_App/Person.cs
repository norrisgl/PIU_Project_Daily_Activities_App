using DailyActivitiesApp;
using System;
using System.Collections.Generic;
using System.Linq;

public class Person
{
    //Numele persoanei
    public string Name { get; set; }        
    //Varsta persoanei
    public int Age { get; set; }            
    //Adresa de email a persoanei
    public string Email { get; set; }          
    //Lista de activitati a persoanei
    public List<Activity> Activities { get; set; } 

    // Constructor
    public Person(string nume, int varsta, string email)
    {
        Name = nume;
        Age = varsta;
        Email = email;
        Activities = new List<Activity>(); // Inițializare listă goală de activități
    }

    //Metoda pentru adaugarea unei activitati
    public void AddActivity(Activity activitate)
    {
        Activities.Add(activitate);
    }

    //Metoda pentru stergerea dupa titlu
    public void RemoveActivity(string title)
    {
        var activity = Activities.FirstOrDefault(a => a.ActivityName == title);
        if (activity != null)
        {
            Activities.Remove(activity);
            Console.WriteLine($"Activitatea '{title}' a fost ștearsă.");
        }
        else
        {
            Console.WriteLine($"Activitatea '{title}' nu a fost găsită.");
        }
    }

    //Metoda pentru afisarea activitatilor
    public void DisplayActivities()
    {
        if (Activities.Count == 0)
        {
            Console.WriteLine("Nu există activități înregistrate.");
        }
        else
        {
            Console.WriteLine($"Activitățile lui {Name}:");
            foreach (var activity in Activities)
            {
                Console.WriteLine($"- {activity.ActivityName} (Prioritate: {activity.Priority}, Data: {activity.DateAndTime})");
            }
        }
    }

    //Metoda afisare informatii persoana
    public void Info()
    {
        Console.WriteLine($"Nume: {Name}");
        Console.WriteLine($"Vârsta: {Age}");
        Console.WriteLine($"Email: {Email}");
        DisplayActivities(); // Afișează și activitățile
    }
}