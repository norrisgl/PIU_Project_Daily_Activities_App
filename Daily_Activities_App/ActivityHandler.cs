using DailyActivitiesApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyActivitiesApp
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

        public bool RemoveActivity(string title)
        {
            var activity = Activities.FirstOrDefault(a => a.ActivityName == title);
            if (activity != null)
            {
                Activities.Remove(activity);
                return true; // Returnează true daca activitatea a fost stearsa
            }
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
