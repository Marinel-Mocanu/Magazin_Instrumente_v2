using LibrarieModele;
using AdministrareDate;
using System.Windows;
using System.Windows.Media;
using Aplicatie_Magazin_Instrumente_Muzicale;

namespace NivelUIWPF
{
    public partial class MainWindow : Window
    {
        IStocareDate adminClienti;
        Client? ultimClient;

        public MainWindow()
        {
            InitializeComponent();

            adminClienti = StocareFactory.GetAdministratorStocare();
            AfisareClienti();
        }

        private void AfisareClienti()
        {
            List<Client> clienti = adminClienti.GetClienti();
            lblNrClienti.Content = $"Numar clienti: {clienti.Count}";
            lblClienti.Content = "Clienti:\n" +
                string.Join("\n", clienti.Select(c => $"{c.ID}: {c.Name} ({c.Email})"));
        }

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            int codValidare = Validare();

            if (codValidare != 0)
            {
                MessageBox.Show("Date invalide!");
                return;
            }

            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string parola = txtParola.Text;

            Client client = new Client(0, username, email, parola);

            adminClienti.AdaugaClient(client);
            ultimClient = client;

            AfisareClienti();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (ultimClient != null)
            {
                txtUsername.Text = ultimClient.Name;
                txtEmail.Text = ultimClient.Email;
                txtParola.Text = ultimClient.Password;
            }
        }

        private int Validare()
        {
            int cod = 0;

            txtUsername.ClearValue(BackgroundProperty);
            txtEmail.ClearValue(BackgroundProperty);
            txtParola.ClearValue(BackgroundProperty);

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || txtUsername.Text.Length > 15)
            {
                txtUsername.Background = Brushes.LightPink;
                cod = 1;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Background = Brushes.LightPink;
                cod = 1;
            }

            if (string.IsNullOrWhiteSpace(txtParola.Text))
            {
                txtParola.Background = Brushes.LightPink;
                cod = 1;
            }

            return cod;
        }
    }
}
