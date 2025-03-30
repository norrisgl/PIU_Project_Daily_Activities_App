using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarieModele
{
    public class Person
    {
        private const char MAIN_FILE_SEPARATOR = ';';
        private const char SECONDARY_FILE_SEPARATOR = ' ';

        //ID unic al persoanei
        public int PersonID { get; set; }
        //Numele persoanei
        public string Name { get; set; }
        //Varsta persoanei
        public int Age { get; set; }
        //Adresa de email a persoanei
        public string Email { get; set; }
        //Lista de activitati a persoanei
        public ActivityHandler ActivityHandler { get; set; }

        //Constructor Implicit
        public Person()
        {
            Name = Email = string.Empty;
        }

        // Constructor
        public Person(int PersID, string nume, int varsta, string email)
        {
            PersonID = PersID;
            Name = nume;
            Age = varsta;
            Email = email;
            ActivityHandler = new ActivityHandler(); // Initializare lista goala de activitati
        }

        // Constructor pentru citire din fisier
        public Person(string linieFisier)
        {
            string[] dateFisier = linieFisier.Split(MAIN_FILE_SEPARATOR);

            PersonID = Convert.ToInt32(dateFisier[0]);
            Name = dateFisier[1];
            Age = Convert.ToInt32(dateFisier[2]);
            Email = dateFisier[3];
            ActivityHandler = new ActivityHandler();
        }

        public string Info()
        {
            return $"ID: {PersonID} \nNume: {Name} \nVarsta: {Age} \nEmail: {Email}";
        }

        public static Person SearchPersonByName(List<Person> persoane, string nume)
        {
            return persoane.FirstOrDefault(p => p.Name.Equals(nume, StringComparison.OrdinalIgnoreCase));
        }

        public string FileConverter()
        {
            string fileObject = string.Format("{1}{0}{2}{0}{3}{0}{4}",
                MAIN_FILE_SEPARATOR,
                PersonID.ToString(),
                Name ?? "NECUNOSCUT",
                Age.ToString() ?? "NECUNOSCUT",
                Email ?? "NECUNOSCUT");

            return fileObject;
        }
    }
}