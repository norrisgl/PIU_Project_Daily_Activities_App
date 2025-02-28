using System;
using System.Collections.Generic;
using System.Linq;

namespace Daily_Activities_App
{
    class Activities_Handler
    {
        //Lista ce stocheaza toate activitatile
        private List<Activity> activities; 

        //Creaza o noua lista de activitati
        public void ActivityList()
        {
            activities = new List<Activity>(); 
        }

        //Adauga activitate in lista
        public void AddActivity(Activity activitaty)
        {
            activities.Add(activitaty); 
        }

        //Sterge activitate din lista pe baza de titlu
        public void RemoveActivity(string title)
        {
            var activity = activities.FirstOrDefault(a => a.Activity_name == title); 
            if(activity != null)
            {
                activities.Remove(activity); 
            }
        }
        
        //Returneaza activitatile din lista
        public List<Activity> GetActivities()
        {
            return activities; 
        }

        //Returneaza activitatile nefinalizate din lista
        public List<Activity> GetUnfinishedActivities()
        {
            return activities.Where(a => !a.Is_finished).ToList(); 
        }

        //Marcheaza activitate ca finalizata
        public void MarkActivityAsFinished(string title)
        {
            var activity = activities.FirstOrDefault(a => a.Activity_name == title); 
            if (activity != null)
            {
                activity.MarkAsFinished();
            }
        }
    }
}
