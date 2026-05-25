using System;
using System.Collections.Generic;
using System.Linq;
using LibrarieModele;

namespace AdministrareDate
{
    public class AdministrareDateMemorie : IStocareDate
    {
        private List<Client> clienti;
        private List<Instrument> instrumente;


        public AdministrareDateMemorie()
        {
            clienti = new List<Client>();
            instrumente = new List<Instrument>();
        }

        // clienti
        public void AdaugaClient(Client client)
        {

            client.ID = clienti.Count > 0 ? clienti.Last().ID + 1 : 1;
            clienti.Add(client);
        }

        public List<Client> GetClienti()
        {
            return clienti;
        }

        public Client CautaClientDupaNume(string nume)
        {
            return clienti.FirstOrDefault(c => c.Name.Equals(nume, StringComparison.OrdinalIgnoreCase));
        }

        public bool StergeClientDupaNume(string nume)
        {
            int elementeSterse = clienti.RemoveAll(c => c.Name.Equals(nume, StringComparison.OrdinalIgnoreCase));
            return elementeSterse > 0;
        }

        public bool UpdateClient(Client clientActualizat)
        {
            var client = clienti.FirstOrDefault(c => c.ID == clientActualizat.ID);
            if (client != null)
            {
                client.Name = clientActualizat.Name;
                client.Email = clientActualizat.Email;
                client.Password = clientActualizat.Password;
                return true;
            }
            return false;
        }

        // instrumente

        public void AdaugaInstrument(Instrument instrument)
        {
            instrument.ID = instrumente.Count > 0 ? instrumente.Last().ID + 1 : 1;
            instrumente.Add(instrument);
        }

        public List<Instrument> GetInstrumente()
        {
            return instrumente;
        }

        public Instrument CautaInstrumentDupaNume(string nume)
        {
            return instrumente.FirstOrDefault(i => i.Name.Equals(nume, StringComparison.OrdinalIgnoreCase));
        }


        public bool StergeInstrumentDupaNume(string nume)
        {
            int elementeSterse = instrumente.RemoveAll(i => i.Name.Equals(nume, StringComparison.OrdinalIgnoreCase));
            return elementeSterse > 0;
        }

        public bool UpdateInstrument(Instrument instrumentActualizat)
        {
            var instrument = instrumente.FirstOrDefault(i => i.ID == instrumentActualizat.ID);
            if (instrument != null)
            {
                instrument.Name = instrumentActualizat.Name;
                instrument.Brand = instrumentActualizat.Brand;
                instrument.Category = instrumentActualizat.Category;
                instrument.Price = instrumentActualizat.Price;
                instrument.Description = instrumentActualizat.Description;
                instrument.Quantity = instrumentActualizat.Quantity;
                instrument.Discount = instrumentActualizat.Discount;
                instrument.Color = instrumentActualizat.Color;
                return true;
            }
            return false;
        }

    }
}