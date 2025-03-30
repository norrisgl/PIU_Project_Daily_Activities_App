using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    public class PersonHandler
    {
        public static Person ReadPersonFromKeyboard()
        {
            Console.WriteLine("Introduceti numele persoanei:");
            string nume = Console.ReadLine();

            Console.WriteLine("Introduceti varsta persoanei:");
            int varsta = int.Parse(Console.ReadLine());

            Console.WriteLine("Introduceti email-ul persoanei:");
            string email = Console.ReadLine();

            return new Person(0, nume, varsta, email);
        }

        public static void DisplayAllPersons(List<Person> persoane)
        {
            if (persoane.Count == 0)
            {
                Console.WriteLine("\nNu exista persoane inregistrate");
                return;
            }

            foreach (var persoana in persoane)
            {
                Console.WriteLine();
                Console.WriteLine("Persoana: ");
                Console.WriteLine(persoana.Info());

                if (persoana.ActivityHandler.Activities != null && persoana.ActivityHandler.Activities.Count > 0)
                {
                    Console.WriteLine("\nActivitati: ");
                    foreach (Activity a in persoana.ActivityHandler.Activities)
                    {
                        Console.WriteLine(a.Info());
                    }
                }
                else
                {
                    Console.WriteLine($"Persoana {persoana.Name} nu are nicio activitate.");
                }
            }
        }
    }
}
