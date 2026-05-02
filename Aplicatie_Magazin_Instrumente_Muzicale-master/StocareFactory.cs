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
            string formatSalvare = ConfigurationManager.AppSettings[FORMAT_SALVARE] ?? "txt";
            string numeFisierClienti = ConfigurationManager.AppSettings[NUME_FISIER] ?? "clienti";

            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? "";

            string caleClienti = Path.Combine(locatieFisierSolutie, numeFisierClienti + "." + formatSalvare);
            string caleInstrumente = Path.Combine(locatieFisierSolutie, "instrumente." + formatSalvare);

            if (formatSalvare == "txt")
            {
                return new AdministrareDateText(caleClienti, caleInstrumente);
            }
            else if (formatSalvare == "memorie")
            {
                return new AdministrareDateMemorie();
            }

            return null;
        }

    }
}