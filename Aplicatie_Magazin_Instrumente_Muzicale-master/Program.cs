using System;
using System.Collections.Generic;
using AdministrareDate;
using LibrarieModele;

namespace Aplicatie_Magazin_Instrumente_Muzicale
{
    class Program
    {
        public static void Main()
        {
            // Angajatii nostri
            IStocareDate adminClienti = StocareFactory.GetAdministratorStocare();
            List<Instrument> instruments = new List<Instrument>(); // Instrumentele raman in memorie deocamdata

            Instrument instrumentNou = null;
            Client clientNou = null;

            string optiune;

            do
            {
                Console.WriteLine("\n=== MAGAZIN INSTRUMENTE MUZICALE ===");
                Console.WriteLine("INSTRUMENTE:");
                Console.WriteLine("  C.  Citire informatii instrument");
                Console.WriteLine("  I.  Afisarea ultimului instrument citit");
                Console.WriteLine("  S.  Salvare instrument in lista");
                Console.WriteLine("  A.  Afisare instrumentele din lista");
                Console.WriteLine("CLIENTI:");
                Console.WriteLine("  C1. Citire info client");
                Console.WriteLine("  S1. Salvare info client in fisier");
                Console.WriteLine("  A1. Afisare toti clientii");
                Console.WriteLine("  P.  Recuperare parola client");
                Console.WriteLine("  X.  Inchidere program");
                Console.Write("\nAlegeti o optiune: ");

                optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

                switch (optiune)
                {
                    // --- ZONA INSTRUMENTE ---
                    case "C":
                        instrumentNou = CitireInstrumentTastatura();
                        break;
                    case "I":
                        if (instrumentNou != null) Console.WriteLine(instrumentNou.Info());
                        else Console.WriteLine("Nu ai citit inca niciun instrument! Apasa C mai intai.");
                        break;
                    case "S":
                        if (instrumentNou != null)
                        {
                            instrumentNou.ID = instruments.Count + 1;
                            instruments.Add(instrumentNou);
                            Console.WriteLine("Instrument salvat in lista (memorie) cu succes!");
                            instrumentNou = null; // Il resetam dupa salvare
                        }
                        else Console.WriteLine("Nu ai niciun instrument gata de salvat.");
                        break;
                    case "A":
                        AfisareInstrumente(instruments);
                        break;

                    // --- ZONA CLIENTI ---
                    case "C1":
                        clientNou = CitireClientTastatura();
                        break;
                    case "S1":
                        if (clientNou != null)
                        {
                            adminClienti.AdaugaClient(clientNou); // Folosim managerul!
                            Console.WriteLine("Client salvat in fisier cu succes!");
                            clientNou = null; // Il resetam dupa salvare
                        }
                        else Console.WriteLine("Nu ai citit niciun client. Apasa C1 mai intai.");
                        break;
                    case "A1":
                        List<Client> clientiDinFisier = adminClienti.GetClienti();
                        AfisareClienti(clientiDinFisier);
                        break;
                    case "P":
                        Console.Write("Introduceti username-ul: ");
                        string numeCautat = Console.ReadLine();
                        Console.Write("Introduceti email-ul: ");
                        string emailCautat = Console.ReadLine();

                        // Cerem managerului sa caute! (Nu mai face Program.cs munca grea)
                        Client clientGasit = adminClienti.CautaClientDupaNume(numeCautat);

                        if (clientGasit != null && clientGasit.Email.Equals(emailCautat, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Parola contului este: {clientGasit.Password}");
                        }
                        else
                        {
                            Console.WriteLine("Eroare: Nu a fost gasit niciun cont cu acest nume si email.");
                        }
                        break;

                    case "X":
                        Console.WriteLine("Inchidere program...");
                        break;
                    default:
                        Console.WriteLine("Optiune inexistenta! Mai incearca.");
                        break;
                }

            } while (optiune != "X");
        }

        public static Instrument CitireInstrumentTastatura()
        {
            Console.Write("Introduceti numele: ");
            string nume = Console.ReadLine();
            Console.Write("Introduceti brandul: ");
            string brand = Console.ReadLine();

            Console.Write("Introduceti price: ");
            double.TryParse(Console.ReadLine(), out double price);

            Console.Write("Introduceti discount (%): ");
            double.TryParse(Console.ReadLine(), out double discount);

            Console.Write("Introduceti cantitate: ");
            int.TryParse(Console.ReadLine(), out int cantitate);

            return new Instrument(nume, brand, price, 0, discount, cantitate);
        }

        public static Client CitireClientTastatura()
        {
            Console.Write("Introduceti username-ul: ");
            string nume = Console.ReadLine();
            Console.Write("Introduceti email-ul: ");
            string email = Console.ReadLine();
            Console.Write("Introduceti o parola: ");
            string parola = Console.ReadLine();

            return new Client(0, nume, email, parola);
        }

        public static void AfisareInstrumente(List<Instrument> instrumente)
        {
            Console.WriteLine("\n--- Instrumentele din stoc ---");
            if (instrumente.Count == 0) Console.WriteLine("Nu exista instrumente in stoc.");
            foreach (Instrument instrument in instrumente)
            {
                Console.WriteLine(instrument.Info());
            }
        }

        public static void AfisareClienti(List<Client> clienti)
        {
            Console.WriteLine("\n--- Clientii Inregistrati ---");
            if (clienti.Count == 0) Console.WriteLine("Nu exista clienti salvati.");
            foreach (Client client in clienti)
            {
                Console.WriteLine(client.Info());
            }
        }
    }
}