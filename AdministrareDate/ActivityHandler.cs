using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele; 

namespace AdministrareDate
{
    public class ActivityHandler
    {
        // Lista de activitati gestionate de administrator
        public List<Activity> Activities { get; set; }

        public ActivityHandler()
        {
            Activities = new List<Activity>();
        }

        public bool AddActivity(Activity activitate)
        {
            Activities.Add(activitate);
            return true; // Returneaza true pentru a indica succesul
        }

        public static bool RemoveActivity(string title, List<Activity> activitati)
        {
            var activity = activitati.FirstOrDefault(a => a.ActivityName == title);
            if (activity != null)
            {
                activitati.Remove(activity);
                Console.WriteLine("Activitatea a fost stearsa"); 
                return true; // Returnează true daca activitatea a fost stearsa
            }
            Console.WriteLine("Activitatea nu a putut fi stearsa"); 
            return false; // Returneaza false daca activitatea nu a fost gasita
        }

        public string GetActivitiesInfo(string personName)
        {
            if (Activities.Count == 0)
            {
                return "Nu exista activitati inregistrate.";
            }

            string result = $"Activitatile lui {personName}:\n";
            foreach (var activity in Activities)
            {
                result += $"- {activity.ActivityName} (Prioritate: {activity.Priority}, Data: {activity.DateAndTime})\n";
            }
            return result;
        }

        public static List<Activity> GetActivitiesByName(List<Person> persoane, string numeCautat)
        {
            if (persoane == null || persoane.Count == 0)
            {
                Console.WriteLine("Lista de persoane este goală.");
                return new List<Activity>();
            }

            Person persoanaGasita = persoane.FirstOrDefault(p =>
                p.Name.Equals(numeCautat, StringComparison.OrdinalIgnoreCase));

            if (persoanaGasita == null)
            {
                Console.WriteLine($"Persoana '{numeCautat}' nu a fost găsită.");
                return new List<Activity>();
            }

            if (persoanaGasita.ActivityHandler?.Activities == null ||
                persoanaGasita.ActivityHandler.Activities.Count == 0)
            {
                Console.WriteLine($"Persoana '{numeCautat}' nu are activități înregistrate.");
                return new List<Activity>();
            }

            return persoanaGasita.ActivityHandler.Activities;
        }

        public bool MarkActivityAsFinished(string title)
        {
            var activity = Activities.FirstOrDefault(a => a.ActivityName == title);
            if (activity != null)
            {
                activity.MarkAsFinished();
                return true; // Returneaza true daca activitatea a fost marcata ca finalizata
            }
            return false; // Returneaza false daca activitatea nu a fost gasita
        }
    }
}
