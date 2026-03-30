using System;

namespace LibrarieModele
{
    public class Client
    {
        private const char SEPARATOR_PRINCIPAL_FISIER = ';';

        private const int ID_INDEX = 0;
        private const int NAME_INDEX = 1;
        private const int EMAIL_INDEX = 2;
        private const int PASSWORD_INDEX = 3;

        // Am transformat parola in proprietate auto-implementata curata
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Client()
        {
            ID = 0;
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }

        public Client(int id, string name, string email, string password = "")
        {
            ID = id;
            Name = name;
            Email = email;
            Password = password;
        }

        // Constructor pentru fisier
        public Client(string linieFisier)
        {
            string[] date = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);

            ID = int.Parse(date[ID_INDEX]);
            Name = date[NAME_INDEX];
            Email = date[EMAIL_INDEX];

            if (date.Length > 3)
                Password = date[PASSWORD_INDEX];
            else
                Password = string.Empty;
        }

        public string ConversieLaSirPentruFisier()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}",
                SEPARATOR_PRINCIPAL_FISIER,
                ID,
                Name ?? "NECUNOSCUT",
                Email ?? "NECUNOSCUT",
                Password ?? "");
        }

        public string Info()
        {
            return $"ID:{ID} | Nume:{Name} | Email:{Email}";
        }
    }
}