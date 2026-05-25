using System.Collections.Generic;
using LibrarieModele; 

namespace AdministrareDate
{
    public interface IStocareDate
    {
        void AdaugaClient(Client c);
        List<Client> GetClienti();
        Client CautaClientDupaNume(string nume); 
        bool StergeClientDupaNume(string nume);
        bool UpdateClient(Client clientActualizat);
        void AdaugaInstrument(Instrument i);
        List<Instrument> GetInstrumente();
        Instrument CautaInstrumentDupaNume(string nume);
        bool StergeInstrumentDupaNume(string nume);
        bool UpdateInstrument(Instrument instrumentActualizat);
    }
}