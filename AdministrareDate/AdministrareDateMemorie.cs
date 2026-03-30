using System;
using System.Collections.Generic;
using System.Linq;
using LibrarieModele;

namespace AdministrareDate
{
    public class AdministrareDateMemorie : IStocareDate
    {
        private List<Client> clienti;

        public AdministrareDateMemorie()
        {
            clienti = new List<Client>();
        }

        public void AdaugaClient(Client client)
        {
            // Generare automata de ID
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
    }
}