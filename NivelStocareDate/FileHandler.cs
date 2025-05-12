using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;
using System.Configuration;

namespace NivelStocareDate
{
    public static class FileHandler
    {
        private const char SEPARATOR_PRINCIPAL_FISIER = ';';

        // Scrie o lista de persoane si activitatile lor in fisier
        public static void WriteToFile(string filePath, List<Person> persoane)
{
    try
    {
        using (StreamWriter writer = new StreamWriter(filePath, false)) // false pentru suprascriere
        {
            foreach (Person persoana in persoane)
            {
                // Scrie informațiile de bază despre persoană
                writer.WriteLine(persoana.FileConverter());
                
                // Scrie activitățile persoanei
                if (persoana.ActivityHandler?.Activities != null)
                {
                    foreach (Activity activitate in persoana.ActivityHandler.Activities)
                    {
                        writer.WriteLine(activitate.FileConverter());
                    }
                }
                
                // Separator între persoane
                writer.WriteLine("---");
            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception($"Eroare la scrierea în fișier: {ex.Message}");
    }
}

        public static List<Person> ReadFromFile()
        {
            List<Person> persoane = new List<Person>();
            Person persoanaCurenta = null;

            string filePath = ConfigurationManager.AppSettings["FilePath"];

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Fișierul 'Persoane.txt' nu a fost găsit în folderul Debug.");
                return persoane;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string linie;
                while ((linie = reader.ReadLine()) != null)
                {
                    if (linie == "---")
                    {
                        // Sfarsitul datelor pentru persoana curenta
                        if (persoanaCurenta != null)
                        {
                            persoane.Add(persoanaCurenta);
                            persoanaCurenta = null;
                        }
                    }
                    else if (persoanaCurenta == null)
                    {
                        // Linie de persoana
                        persoanaCurenta = new Person(linie);
                    }
                    else
                    {
                        // Linie de activitate
                        Activity activitate = new Activity(linie);
                        persoanaCurenta.ActivityHandler.AddActivity(activitate);
                    }
                }

                // Adauga ultima persoana citita (daca exista)
                if (persoanaCurenta != null)
                {
                    persoane.Add(persoanaCurenta);
                }
            }

            return persoane;
        }

        public static void AppendToFile(Person persoana)
        {
            string filePath = ConfigurationManager.AppSettings["FilePath"];

            // Check if the file exists, if not create it
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close(); // Create the file and immediately close it
            }

            using (StreamWriter writer = new StreamWriter(filePath, true)) // true for append mode
            {
                // Write person information
                writer.WriteLine(persoana.FileConverter());

                // Write each activity of the person
                if (persoana.ActivityHandler != null && persoana.ActivityHandler.Activities != null)
                {
                    foreach (Activity activitate in persoana.ActivityHandler.Activities)
                    {
                        writer.WriteLine(activitate.FileConverter());
                    }
                }

                // Add separator to indicate end of person data
                writer.WriteLine("---");
            }
        }
    }
}
