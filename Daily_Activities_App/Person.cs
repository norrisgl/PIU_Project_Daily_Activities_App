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
    public ActivityHandler ActivityHandler { get; set; }

    // Constructor
    public Person(string nume, int varsta, string email)
    {
        Name = nume;
        Age = varsta;
        Email = email;
        ActivityHandler = new ActivityHandler(); // Inițializare listă goală de activități
    }
    public string Info()
    {
        return $"Nume: {Name} \nVarsta: {Age} \nEmail: {Email}"; 
    }
}