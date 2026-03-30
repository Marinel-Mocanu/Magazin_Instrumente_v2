using System.Configuration;
using System.IO;
using AdministrareDate;

namespace Aplicatie_Magazin_Instrumente_Muzicale
{
    public static class StocareFactory
    {
        private const string FORMAT_SALVARE = "FormatSalvare";
        private const string NUME_FISIER = "NumeFisier";

        public static IStocareDate GetAdministratorStocare()
        {
            // Citim din App.config
            string formatSalvare = ConfigurationManager.AppSettings[FORMAT_SALVARE] ?? "txt";
            string numeFisier = ConfigurationManager.AppSettings[NUME_FISIER] ?? "Clienti";

            // Calculam calea ca sa apara fisierul langa proiect
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? "";
            string caleCompletaFisier = locatieFisierSolutie + "\\" + numeFisier;

            if (formatSalvare == "txt")
            {
                return new AdministrareDateText(caleCompletaFisier + "." + formatSalvare);
            }
            else if (formatSalvare == "memorie")
            {
                return new AdministrareDateMemorie();
            }

            return null;
        }
    }
}