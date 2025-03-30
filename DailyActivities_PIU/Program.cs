using System;
using System.Collections.Generic;
using System.Linq;
using LibrarieModele;
using NivelStocareDate;
using System.Configuration; 

namespace DailyActivities_PIU
{
    class Program
    {
        static void Main()
        {
            List<Person> persoane = new List<Person>();
            persoane = FileHandler.ReadFromFile();

            int PersonsNr = 0;
            Person persoanaNoua = new Person();

            while (true)
            {
                Console.WriteLine("\n1. Adaugati o persoana");
                Console.WriteLine("2. Afisati toate persoanele");
                Console.WriteLine("3. Căutati o persoana dupa nume");
                Console.WriteLine("4. Cautati activitatile unei persoane");
                Console.WriteLine("5. Salvare persoana in fisier");
                Console.WriteLine("6. Citire persoane din fisier");
                Console.WriteLine("7. Sterge activitatea unei persoane"); 
                Console.WriteLine("8. Iesire");
                Console.Write("\nAlegeti o optiune: ");
                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        persoanaNoua = PersonHandler.ReadPersonFromKeyboard();
                        Console.WriteLine("\nCate activitati doriti sa adaugati?");
                        int numarActivitati = int.Parse(Console.ReadLine());

                        for (int i = 0; i < numarActivitati; i++)
                        {
                            Console.WriteLine($"\nIntroduceti activitatea {i + 1}:");
                            Activity activitate = ActivityHandler.ReadActivityFromKeyboard();
                            persoanaNoua.ActivityHandler.AddActivity(activitate);
                        }

                        persoane.Add(persoanaNoua); 
                        break;

                    case "2":
                        PersonHandler.DisplayAllPersons(persoane);
                        break;

                    case "3":
                        Console.WriteLine("\nIntroduceti numele persoanei:");
                        string numeCautat = Console.ReadLine();
                        Person persoanaGasita = Person.SearchPersonByName(persoane, numeCautat);

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
                        Console.WriteLine("\nIntroduceti numele persoanei:");
                        numeCautat = Console.ReadLine();

                        List<Activity> activitati = ActivityHandler.GetActivitiesByName(persoane, numeCautat);
                        if (activitati.Count > 0)
                        {
                            Console.WriteLine($"\nActivitățile lui {numeCautat}:");
                            foreach (var activitate in activitati)
                            {
                                Console.WriteLine(activitate.Info());
                            }
                        }
                        else
                        {
                            Console.WriteLine($"\n{numeCautat} nu are activitati inregistrate"); 
                        }
                            break;

                    case "5":
                        int idPersoana = ++PersonsNr;
                        persoanaNoua.PersonID = idPersoana;

                        FileHandler.WriteInFile("Persoane.txt", persoanaNoua); 
                        break;

                    case "6":
                        Console.WriteLine("\nInformatiile din fisier: "); 
                        PersonHandler.DisplayAllPersons(FileHandler.ReadFromFile()); 

                        break;

                    case "7":
                        Console.WriteLine("Introduceti numele persoanei:");
                        numeCautat = Console.ReadLine();

                        Console.WriteLine("Introduceti numele activitatii pe care doriti sa o stergeti: ");
                        string activitateDeSters = Console.ReadLine();
                        activitati = ActivityHandler.GetActivitiesByName(persoane, numeCautat);
                        ActivityHandler.RemoveActivity(activitateDeSters, activitati); 
                        
                        break; 

                    case "8":

                        return;

                    default:
                        Console.WriteLine("Optiune invalida.");
                        break;
                }
            }
        }
    }
}
