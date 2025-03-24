using DailyActivitiesApp;
using System;
using System.Collections.Generic;
using System.IO;

public static class FileHandler
{
    private const char SEPARATOR_PRINCIPAL_FISIER = ';';

    // Scrie o listă de persoane și activitățile lor în fișier
    public static void WriteInFile(string filePath, Person persoana)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Scrie informațiile despre persoană
            writer.WriteLine(persoana.FileConverter());

            // Scrie fiecare activitate a persoanei
            if (persoana.ActivityHandler != null && persoana.ActivityHandler.Activities != null)
            {
                // Scrie fiecare activitate a persoanei
                foreach (Activity activitate in persoana.ActivityHandler.Activities)
                {
                    writer.WriteLine(activitate.FileConverter());
                }
            }

            // Adaugă un separator pentru a indica sfârșitul datelor persoanei
            writer.WriteLine("---");
        }
    }

    public static List<Person> ReadFromFile()
    {
        List<Person> persoane = new List<Person>();
        Person persoanaCurenta = null;

        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Persoane.txt");

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
                    // Sfârșitul datelor pentru persoana curentă
                    if (persoanaCurenta != null)
                    {
                        persoane.Add(persoanaCurenta);
                        persoanaCurenta = null;
                    }
                }
                else if (persoanaCurenta == null)
                {
                    // Linie de persoană
                    persoanaCurenta = new Person(linie);
                }
                else
                {
                    // Linie de activitate
                    Activity activitate = new Activity(linie);
                    persoanaCurenta.ActivityHandler.AddActivity(activitate);
                }
            }

            // Adaugă ultima persoană citită (dacă există)
            if (persoanaCurenta != null)
            {
                persoane.Add(persoanaCurenta);
            }
        }

        return persoane;
    }
}

