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
        private string numeFisierInstrumente;
    
        public AdministrareDateText(string numeFisier, string numeFisierInstrumente)
        {
            this.numeFisier = numeFisier;
            this.numeFisierInstrumente = numeFisierInstrumente;
            Stream stream = File.Open(numeFisier, FileMode.OpenOrCreate);
            stream.Close();
            Stream streamInstrumente = File.Open(numeFisierInstrumente, FileMode.OpenOrCreate);
            streamInstrumente.Close();
        }

        //clienti

        public void AdaugaClient(Client client)
        {
            client.ID = GetNextId();
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
               
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
                    if (string.IsNullOrWhiteSpace(linie)) continue; 


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

            using (StreamWriter sw = new StreamWriter(numeFisier, false)) 
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

        // instrumente

        public void AdaugaInstrument(Instrument instrument)
        {
            instrument.ID = GetNextInstrumentId();
            using (StreamWriter sw = new StreamWriter(numeFisierInstrumente, true))
            {
                sw.WriteLine(instrument.ConversieLaSirPentruFisier());
            }
        }


        public List<Instrument> GetInstrumente()
        {
            List<Instrument> instrumente = new List<Instrument>();
            using (StreamReader sr = new StreamReader(numeFisierInstrumente))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linie)) continue;

                    instrumente.Add(new Instrument(linie));
                }
            }
            return instrumente;
        }

        public Instrument CautaInstrumentDupaNume(string nume)
        {
            List<Instrument> instrumente = GetInstrumente();
            return instrumente.FirstOrDefault(i => i.Name.Equals(nume, StringComparison.OrdinalIgnoreCase));
        }

        public bool StergeInstrumentDupaNume(string nume)
        {
            List<Instrument> instrumente = GetInstrumente();
            int sters = instrumente.RemoveAll(i => i.Name.Equals(nume, StringComparison.OrdinalIgnoreCase));

            if (sters > 0)
            {
                using (StreamWriter sw = new StreamWriter(numeFisierInstrumente, false))
                {
                    foreach (Instrument i in instrumente)
                    {
                        sw.WriteLine(i.ConversieLaSirPentruFisier());
                    }
                }
                return true;
            }
            return false;
        }

        public bool UpdateInstrument(Instrument instrumentActualizat)
        {
            List<Instrument> instrumente = GetInstrumente();
            bool actualizat = false;

            using (StreamWriter sw = new StreamWriter(numeFisierInstrumente, false))
            {
                foreach (Instrument i in instrumente)
                {
                    if (i.ID == instrumentActualizat.ID)
                    {
                        sw.WriteLine(instrumentActualizat.ConversieLaSirPentruFisier());
                        actualizat = true;
                    }
                    else
                    {
                        sw.WriteLine(i.ConversieLaSirPentruFisier());
                    }
                }
            }
            return actualizat;
        }

        private int GetNextInstrumentId()
        {
            List<Instrument> instrumente = GetInstrumente();
            if (instrumente.Count == 0) return 1;
            return instrumente.Last().ID + 1;
        }

    }
}