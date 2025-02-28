using System;

namespace Daily_Activities_App
{
    class Program
    {
        static void Main()
        {
            Activity newActivity = new Activity("Lucrez la proiect", "Implementarea primelor 2 clase", DateTime.Now.AddDays(1), priority_level.High);
            Console.WriteLine(newActivity.Info());
            Console.ReadKey(); 
        }
    }
}
