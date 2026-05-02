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
            cmbCategorie.ItemsSource = Enum.GetValues(typeof(Instrument_Category));
            AfisareClienti();
            AfisareInstrumente();
        }
        private void BtnClienti_Click(object sender, RoutedEventArgs e)
        {
            GridClienti.Visibility = Visibility.Visible;
            GridInstrumente.Visibility = Visibility.Hidden;
            GridLogin.Visibility = Visibility.Hidden;
        }

        private void BtnInstrumente_Click(object sender, RoutedEventArgs e)
        {
            GridClienti.Visibility = Visibility.Hidden;
            GridInstrumente.Visibility = Visibility.Visible;
            GridLogin.Visibility = Visibility.Hidden;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            GridClienti.Visibility = Visibility.Hidden;
            GridInstrumente.Visibility = Visibility.Hidden;
            GridLogin.Visibility = Visibility.Visible;
        }

        // clienti

        private void AfisareClienti()
        {
            var clienti = adminClienti.GetClienti();

            lblNrClienti.Content = $"Numar clienti: {clienti.Count}";

            listClienti.Items.Clear();

            foreach (var c in clienti)
            {
                listClienti.Items.Add($"{c.Name} ({c.Email})");
            }
        }

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            if (Validare() != 0)
            {
                MessageBox.Show("Date invalide!");
                return;
            }

            Client client = new Client(0, txtUsername.Text, txtEmail.Text, txtParola.Text);

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

        // instrumente

        private void BtnAdaugaInstrument_Click(object sender, RoutedEventArgs e)
        {
            double.TryParse(txtPret.Text, out double pret);

            Instrument_Category categorie = (Instrument_Category)cmbCategorie.SelectedItem;

            CustomOrderColor culoare = CustomOrderColor.Black;
            if (rbRed.IsChecked == true) culoare = CustomOrderColor.Red;
            if (rbBlue.IsChecked == true) culoare = CustomOrderColor.Blue;
            if (rbGreen.IsChecked == true) culoare = CustomOrderColor.Green;
            if (rbWhite.IsChecked == true) culoare = CustomOrderColor.White;
            if (rbPurple.IsChecked == true) culoare = CustomOrderColor.Purple;
            if (rbOrange.IsChecked == true) culoare = CustomOrderColor.Orange;

            double discount = chkDiscount.IsChecked == true ? 10 : 0;

            Instrument instrument = new Instrument(
                0,
                txtNumeInstrument.Text,
                txtBrand.Text,
                categorie,
                pret,
                "Fara descriere",
                1,
                discount,
                culoare
            );

            adminClienti.AdaugaInstrument(instrument);

            AfisareInstrumente();
        }

        private void AfisareInstrumente()
        {
            listInstrumente.Items.Clear();

            foreach (var i in adminClienti.GetInstrumente())
            {
                listInstrumente.Items.Add(i.Name);
            }
        }

        private void ListInstrumente_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (listInstrumente.SelectedItem == null) return;

            string nume = listInstrumente.SelectedItem.ToString();

            var instrument = adminClienti.CautaInstrumentDupaNume(nume);

            if (instrument != null)
            {
                MessageBox.Show(
                    $"Nume: {instrument.Name}\n" +
                    $"Brand: {instrument.Brand}\n" +
                    $"Pret: {instrument.Price}\n" +
                    $"Pret final: {instrument.Final_Price()}\n" +
                    $"Cantitate: {instrument.Quantity}"
                );
            }
        }

        private void BtnCautaInstrument_Click(object sender, RoutedEventArgs e)
        {
            string text = txtCautareInstrument.Text.Trim().ToLower();

            var toateInstrumentele = adminClienti.GetInstrumente();

            var rezultate = toateInstrumentele
                .Where(i => i.Name.ToLower().Contains(text))
                .ToList();

            listRezultate.Items.Clear();

            foreach (var i in rezultate)
            {
                listRezultate.Items.Add($"{i.Name} - {i.Brand} ({i.Price} RON)");
            }
        }
        
        private void BtnDeschideStergere_Click(object sender, RoutedEventArgs e)
        {
            panelStergere.Visibility = Visibility.Visible;

            listStergere.Items.Clear();

            foreach (var i in adminClienti.GetInstrumente())
            {
                listStergere.Items.Add(i.Name);
            }
        }
        private void BtnConfirmaStergere_Click(object sender, RoutedEventArgs e)
        {
            if (listStergere.SelectedItem == null)
            {
                MessageBox.Show("Selecteaza un instrument!");
                return;
            }

            string nume = listStergere.SelectedItem.ToString();

            bool sters = adminClienti.StergeInstrumentDupaNume(nume);

            if (sters)
            {
                MessageBox.Show("Instrument sters!");

                panelStergere.Visibility = Visibility.Collapsed;

                AfisareInstrumente();
            }
            else
            {
                MessageBox.Show("Eroare la stergere!");
            }
        }
        private void BtnAnuleazaStergere_Click(object sender, RoutedEventArgs e)
        {
            panelStergere.Visibility = Visibility.Collapsed;
        }





        private void ListRezultate_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (listRezultate.SelectedItem == null) return;

            string selected = listRezultate.SelectedItem.ToString();

            string nume = selected.Split('-')[0].Trim();

            var instrument = adminClienti.CautaInstrumentDupaNume(nume);

            if (instrument != null)
            {
                MessageBox.Show(
                    $"Nume: {instrument.Name}\n" +
                    $"Brand: {instrument.Brand}\n" +
                    $"Pret: {instrument.Price}\n" +
                    $"Pret final: {instrument.Final_Price()}"
                );
            }
        }



        // logare

        private void BtnLoginUser_Click(object sender, RoutedEventArgs e)
        {
            var client = adminClienti.CautaClientDupaNume(txtLoginUser.Text);

            if (client != null &&
                client.Email.Equals(txtLoginEmail.Text, StringComparison.OrdinalIgnoreCase))
            {
                listLogati.Items.Add(client.Name);
            }
            else
            {
                MessageBox.Show("Date gresite!");
            }
        }
    }
}

