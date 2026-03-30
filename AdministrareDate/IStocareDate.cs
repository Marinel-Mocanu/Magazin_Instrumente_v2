using System.Collections.Generic;
using LibrarieModele; // Foarte important! Așa recunoaște clasa Client

namespace AdministrareDate
{
    public interface IStocareDate
    {
        void AdaugaClient(Client c);
        List<Client> GetClienti();
        Client CautaClientDupaNume(string nume); // Fara liste trimise ca parametru!
        bool StergeClientDupaNume(string nume);
        bool UpdateClient(Client clientActualizat);
    }
}