using System;
using System.Collections.Generic;
using System.Linq; 

namespace DailyActivitiesApp
{
    class Program
    {
        static void Main()
        {
            List<Person> persoane = new List<Person>();

            int PersonsNr = 0;
            Person persoanaNoua = new Person();

            while (true)
            {
                Console.WriteLine("\n1. Adaugati o persoana");
                Console.WriteLine("2. Afisati toate persoanele");
                Console.WriteLine("3. Căutati o persoana dupa nume");
                Console.WriteLine("4. Salvare persoana in fisier");
                Console.WriteLine("5. Citire persoane din fisier");
                Console.WriteLine("6. Iesire");
                Console.Write("Alegeti o optiune: ");
                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        persoanaNoua = ReadPersonFromKeyboard();
                        Console.WriteLine("Cate activitati doriti sa adaugati?");
                        int numarActivitati = int.Parse(Console.ReadLine());

                        for (int i = 0; i < numarActivitati; i++)
                        {
                            Console.WriteLine($"Introduceti activitatea {i + 1}:");
                            Activity activitate = ReadActivityFromKeyboard();
                            persoanaNoua.ActivityHandler.AddActivity(activitate);
                        }

                        persoane.Add(persoanaNoua); 
                        break;

                    case "2":
                        DisplayAllPersons(persoane);
                        break;

                    case "3":
                        Console.WriteLine("Introduceti numele persoanei:");
                        string numeCautat = Console.ReadLine();
                        Person persoanaGasita = SearchPersonByName(persoane, numeCautat);

                        if (persoanaGasita != null)
                        {
                            Console.WriteLine(persoanaGasita.Info());
                        }
                        else
                        {
                            Console.WriteLine("Persoana nu a fost găsita.");
                        }
                        break;

                    case "4":
                        int idPersoana = ++PersonsNr;
                        persoanaNoua.PersonID = idPersoana;

                        FileHandler.WriteInFile("Persoane.txt", persoanaNoua); 
                        break;

                    case "5":
                        FileHandler.ReadFromFile(); 

                        break; 

                    case "6":

                        return;

                    default:
                        Console.WriteLine("Optiune invalida.");
                        break;
                }
            }
        }

        public static Activity ReadActivityFromKeyboard()
        {
            Console.WriteLine("Introduceti numele activitatii:");
            string nume = Console.ReadLine();

            Console.WriteLine("Introduceti descrierea activitatii:");
            string descriere = Console.ReadLine();

            Console.WriteLine("Introduceti data si ora activitatii (format: dd/MM/yyyy HH:mm):");
            DateTime dataOra = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", null);

            Console.WriteLine("Introduceti nivelul de prioritate (Low, Medium, High):");
            PriorityLevel prioritate = (PriorityLevel)Enum.Parse(typeof(PriorityLevel), Console.ReadLine(), true);

            Console.WriteLine("Introduceti tipul activitatii (Work, Study, FreeTime, Shopping, Sport, Other):");
            ActivityType atype = (ActivityType)Enum.Parse(typeof(ActivityType), Console.ReadLine(), true); 
            return new Activity(nume, descriere, dataOra, prioritate, atype);
        }

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
            if(persoane.Count == 0)
            {
                Console.WriteLine("Nu exista persoane inregistrate");
                return; 
            }

            foreach (var persoana in persoane)
            {
                Console.WriteLine("Persoana: "); 
                Console.WriteLine(persoana.Info());
                Console.WriteLine(); // Linie goală pentru separare
            }
        }

        public static Person SearchPersonByName(List<Person> persoane, string nume)
        {
            return persoane.FirstOrDefault(p => p.Name.Equals(nume, StringComparison.OrdinalIgnoreCase));
        }
    }
}
