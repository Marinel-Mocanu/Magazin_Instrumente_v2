using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibrarieModele;

namespace AdministrareDate
{
    public class AdministrareDateText : IStocareDate
    {
        private string numeFisier;

        public AdministrareDateText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            Stream stream = File.Open(numeFisier, FileMode.OpenOrCreate);
            stream.Close();
        }

        public void AdaugaClient(Client client)
        {
            client.ID = GetNextId();
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                // Refolosim codul din macheta!
                sw.WriteLine(client.ConversieLaSirPentruFisier());
            }
        }

        public List<Client> GetClienti()
        {
            List<Client> clienti = new List<Client>();
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linie)) continue; // protectie la randuri goale

                    // Refolosim constructorul cu string din macheta!
                    clienti.Add(new Client(linie));
                }
            }
            return clienti;
        }

        public Client CautaClientDupaNume(string nume)
        {
            List<Client> clienti = GetClienti();
            return clienti.FirstOrDefault(c => c.Name.Equals(nume, StringComparison.OrdinalIgnoreCase));
        }

        public bool StergeClientDupaNume(string nume)
        {
            List<Client> clienti = GetClienti();
            int sters = clienti.RemoveAll(c => c.Name.Equals(nume, StringComparison.OrdinalIgnoreCase));

            // Daca s-a sters ceva, suprascriem fisierul
            if (sters > 0)
            {
                using (StreamWriter sw = new StreamWriter(numeFisier, false))
                {
                    foreach (Client c in clienti)
                    {
                        sw.WriteLine(c.ConversieLaSirPentruFisier());
                    }
                }
                return true;
            }
            return false;
        }

        public bool UpdateClient(Client clientActualizat)
        {
            List<Client> clienti = GetClienti();
            bool actualizat = false;

            using (StreamWriter sw = new StreamWriter(numeFisier, false)) // false = overwrite
            {
                foreach (Client c in clienti)
                {
                    if (c.ID == clientActualizat.ID)
                    {
                        sw.WriteLine(clientActualizat.ConversieLaSirPentruFisier());
                        actualizat = true;
                    }
                    else
                    {
                        sw.WriteLine(c.ConversieLaSirPentruFisier());
                    }
                }
            }
            return actualizat;
        }

        private int GetNextId()
        {
            List<Client> clienti = GetClienti();
            if (clienti.Count == 0) return 1;
            return clienti.Last().ID + 1;
        }
    }
}