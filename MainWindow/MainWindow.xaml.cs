using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LibrarieModele;
using AdministrareDate;
using Aplicatie_Magazin_Instrumente_Muzicale;
using System.Windows.Media;
namespace NivelUIWPF
{
    public partial class MainWindow : Window
    {
        IStocareDate adminClienti;
        List<string> cos = new List<string>();
        List<string> comenzi = new List<string>();
        Instrument produsSelectat;

        public MainWindow()
        {
            InitializeComponent();

            adminClienti = StocareFactory.GetAdministratorStocare();

            AfisareInstrumente();
            AfisareClienti();
            AfisareProduseClient();
        }
        // START

        private void BtnManager_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridStart.Visibility = Visibility.Collapsed;
            GridManager.Visibility = Visibility.Visible;
        }

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridStart.Visibility = Visibility.Collapsed;
            GridClient.Visibility = Visibility.Visible;
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BtnBackHome_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridStart.Visibility = Visibility.Visible;

            GridManager.Visibility = Visibility.Collapsed;
            GridClient.Visibility = Visibility.Collapsed;
        }

        // MENIU MANAGER

        private void BtnMeniuInstrumente_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridManagerInstrumente.Visibility = Visibility.Visible;
            GridManagerClienti.Visibility = Visibility.Collapsed;
            GridManagerComenzi.Visibility = Visibility.Collapsed;
        }

        private void BtnMeniuClienti_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridManagerInstrumente.Visibility = Visibility.Collapsed;
            GridManagerClienti.Visibility = Visibility.Visible;
            GridManagerComenzi.Visibility = Visibility.Collapsed;
        }

        private void BtnMeniuComenzi_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridManagerInstrumente.Visibility = Visibility.Collapsed;
            GridManagerClienti.Visibility = Visibility.Collapsed;
            GridManagerComenzi.Visibility = Visibility.Visible;
        }

        // ADAUGARE INSTRUMENT
        private void BtnAdaugaInstrument_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumeInstrument.Text) ||
                string.IsNullOrWhiteSpace(txtBrand.Text) ||
                string.IsNullOrWhiteSpace(txtPret.Text) ||
                cmbCategorie.SelectedItem == null)
            {
                AfiseazaMesaj("Completeaza toate campurile mai intai!", Brushes.Red);
                return;
            }

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

            AfiseazaMesaj("Instrument adaugat cu succes!", Brushes.Lime);

            AfisareInstrumente();
        }

        private void AfisareInstrumente()
        {
            listInstrumente.Items.Clear();

            foreach (var i in adminClienti.GetInstrumente())
            {
                listInstrumente.Items.Add(i.Name + " - " + i.Brand);
            }
        }

        private void ListInstrumente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listInstrumente.SelectedItem == null)
                return;

            string selected = listInstrumente.SelectedItem.ToString();

            string nume = selected.Split('-')[0].Trim();

            var instrument = adminClienti.CautaInstrumentDupaNume(nume);

            if (instrument != null)
            {
                produsSelectat = instrument;

                txtProdusNume.Text = instrument.Name;
                txtProdusBrand.Text = "Brand: " + instrument.Brand;
                txtProdusCategorie.Text = "Categorie: " + instrument.Category;
                txtProdusPret.Text = "Pret: " + instrument.Final_Price() + " RON";
                txtProdusDiscount.Text = "Discount: " + instrument.Discount + "%";
                txtProdusCantitate.Text = "Stoc: " + instrument.Quantity;
                txtProdusDescriere.Text = instrument.Description;

                GridClient.Visibility = Visibility.Collapsed;
                GridProductPage.Visibility = Visibility.Visible;
            }
        }
        private void txtSearchManager_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = txtSearchManager.Text.ToLower();

            listInstrumente.Items.Clear();

            foreach (var i in adminClienti.GetInstrumente())
            {
                if (i.Name.ToLower().Contains(text))
                {
                    listInstrumente.Items.Add(i.Name + " - " + i.Brand);
                }
            }
        }

        private void BtnStergeInstrument_Click(object sender, RoutedEventArgs e)
        {
            if (listInstrumente.SelectedItem == null)
            {
                AfiseazaMesaj("Selecteaza un instrument!", Brushes.Red);
                return;
            }
            string text = listInstrumente.SelectedItem.ToString();
            string nume = text.Split('-')[0].Trim();

            adminClienti.StergeInstrumentDupaNume(nume);

            AfisareInstrumente();
            AfisareProduseClient();

            AfiseazaMesaj("Instrument sters!", Brushes.Lime);
        }

        // CLIENTI

        private void AfisareClienti()
        {
            listClienti.Items.Clear();

            foreach (var c in adminClienti.GetClienti())
            {
                listClienti.Items.Add(c.Name + " - " + c.Email);
            }
        }

        // CLIENT HOME

        private void AfisareProduseClient(Instrument_Category? filtru = null)
        {
            wrapProduse.Children.Clear();

            var lista = adminClienti.GetInstrumente();

            if (filtru != null)
                lista = lista.Where(i => i.Category == filtru.Value).ToList();

            foreach (var i in lista)
            {
                Border card = new Border();
                card.Width = 220;
                card.Height = 260;
                card.Margin = new Thickness(10);
                card.Background = Brushes.Black;
                card.BorderBrush = Brushes.Cyan;
                card.BorderThickness = new Thickness(2);

                StackPanel stack = new StackPanel();

                TextBlock txt = new TextBlock();
                txt.Text = i.Name + "\n" +
                           i.Brand + "\n" +
                           i.Price + " RON";
                txt.Foreground = Brushes.White;
                txt.Margin = new Thickness(5);
                txt.FontSize = 16;

                Button btnVezi = new Button();
                btnVezi.Content = "Vezi produs";
                btnVezi.Tag = i.Name;
                btnVezi.Click += BtnVeziProdus_Click;

                Button btnCos = new Button();
                btnCos.Content = "Adauga in cos";
                btnCos.Tag = i.Name;
                btnCos.Click += BtnAddToCart_Click;

                stack.Children.Add(txt);
                stack.Children.Add(btnVezi);
                stack.Children.Add(btnCos);

                card.Child = stack;
                wrapProduse.Children.Add(card);
            }

            if (filtru != null && wrapProduse.Children.Count == 0)
                AfiseazaMesaj("Nu exista produse in aceasta categorie!", Brushes.Red);
        }
        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string produs = btn.Tag.ToString();

            cos.Add(produs);

            listCos.Items.Add(produs);

            AfiseazaMesaj("Produs adaugat!", Brushes.Lime);
        }

        // COS

        private void BtnCos_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridClientHome.Visibility = Visibility.Collapsed;
            GridBlog.Visibility = Visibility.Collapsed;
            GridCos.Visibility = Visibility.Visible;
        }

        private void BtnPlaseazaComanda_Click(object sender, RoutedEventArgs e)
        {
            if (cos.Count == 0)
            {
                AfiseazaMesaj("Cos gol!", Brushes.Red);
                return;
            }

            string comanda = "Comanda noua: " + string.Join(", ", cos);

            comenzi.Add(comanda);
            listComenzi.Items.Add(comanda);

            cos.Clear();
            listCos.Items.Clear();

            AfiseazaMesaj("Comanda plasata!", Brushes.Lime);
        }

        // CONFIRMARE COMANDA

        private void BtnConfirmaComanda_Click(object sender, RoutedEventArgs e)
        {
            if (listComenzi.SelectedItem == null)
            {
                AfiseazaMesaj("Selecteaza o comanda!", Brushes.Red);
                return;
            }

            AfiseazaMesaj("Comanda confirmata!", Brushes.Lime);
        }

        // BLOG

        private void BtnBlog_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridClientHome.Visibility = Visibility.Collapsed;
            GridCos.Visibility = Visibility.Collapsed;
            GridBlog.Visibility = Visibility.Visible;
        }

        private void BtnHomeClient_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridClientHome.Visibility = Visibility.Visible;
            GridCos.Visibility = Visibility.Collapsed;
            GridBlog.Visibility = Visibility.Collapsed;
            AfisareProduseClient();
        }

        private void BtnAdaugaAnunt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAnunt.Text))
                return;

            listAnunturi.Items.Add(txtAnunt.Text);

            txtAnunt.Clear();
        }

        // CATEGORII

        private void BtnChitari_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridClientHome.Visibility = Visibility.Visible;
            GridCos.Visibility = Visibility.Collapsed;
            GridBlog.Visibility = Visibility.Collapsed;
            AfisareProduseClient(Instrument_Category.Guitars);
        }

        private void BtnTobe_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridClientHome.Visibility = Visibility.Visible;
            GridCos.Visibility = Visibility.Collapsed;
            GridBlog.Visibility = Visibility.Collapsed;
            AfisareProduseClient(Instrument_Category.Drums);
        }

        private void BtnClape_Click(object sender, RoutedEventArgs e)
        {
            AscundeMesaj();
            GridClientHome.Visibility = Visibility.Visible;
            GridCos.Visibility = Visibility.Collapsed;
            GridBlog.Visibility = Visibility.Collapsed;
            AfisareProduseClient(Instrument_Category.Keyboards);
        }
        private void AfiseazaMesaj(string mesaj, Brush culoare)
        {
            txtStatus.Text = mesaj;
            txtStatus.Foreground = culoare;
            txtStatus.Visibility = Visibility.Visible;
        }
        private void AscundeMesaj()
        {
            txtStatus.Visibility = Visibility.Collapsed;
        }



        private void BtnSearchClient_Click(object sender, RoutedEventArgs e)
        {
            string text = txtSearchClient.Text.ToLower().Trim();

            wrapProduse.Children.Clear();

            var rezultate = adminClienti.GetInstrumente()
                .Where(i => i.Name.ToLower().Contains(text))
                .ToList();

            foreach (var i in rezultate)
            {
                Border card = new Border();
                card.Width = 220;
                card.Height = 260;
                card.Margin = new Thickness(10);
                card.Background = Brushes.Black;
                card.BorderBrush = Brushes.Cyan;
                card.BorderThickness = new Thickness(2);

                StackPanel stack = new StackPanel();

                TextBlock txt = new TextBlock();
                txt.Text = i.Name + "\n" +
                           i.Brand + "\n" +
                           i.Price + " RON";

                txt.Foreground = Brushes.White;
                txt.Margin = new Thickness(5);

                Button btnVezi = new Button();
                btnVezi.Content = "Vezi produs";
                btnVezi.Tag = i.Name;
                btnVezi.Click += BtnVeziProdus_Click;

                Button btnCos = new Button();
                btnCos.Content = "Adauga in cos";
                btnCos.Tag = i.Name;
                btnCos.Click += BtnAddToCart_Click;

                stack.Children.Add(txt);
                stack.Children.Add(btnVezi);
                stack.Children.Add(btnCos);

                card.Child = stack;

                wrapProduse.Children.Add(card);
            }

            if (rezultate.Count == 0)
            {
                AfiseazaMesaj("Nu exista produse gasite!", Brushes.Red);
            }
        }

        private void BtnVeziProdus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string nume = btn.Tag.ToString();

            var instrument = adminClienti.CautaInstrumentDupaNume(nume);

            if (instrument == null)
                return;

            GridClientHome.Visibility = Visibility.Collapsed;
            GridProductPage.Visibility = Visibility.Visible;

            txtProdusNume.Text = instrument.Name;
            txtProdusBrand.Text = "Brand: " + instrument.Brand;
            txtProdusCategorie.Text = "Categorie: " + instrument.Category.ToString();
            txtProdusPret.Text = "Pret: " + instrument.Price + " RON";
            txtProdusDiscount.Text = "Discount: " + instrument.Discount + "%";
            txtProdusCantitate.Text = "Stoc: " + instrument.Quantity;

            txtProdusDescriere.Text = instrument.Description;
        }
        private void BtnActualizeazaInstrument_Click(object sender, RoutedEventArgs e)
        {
            if (listInstrumente.SelectedItem == null)
            {
                AfiseazaMesaj("Selecteaza un instrument din lista!", Brushes.Red);
                return;
            }

            string text = listInstrumente.SelectedItem.ToString();
            string nume = text.Split('-')[0].Trim();

            var instrument = adminClienti.CautaInstrumentDupaNume(nume);

            if (instrument == null) return;

            instrument.Name = txtNumeInstrument.Text;
            instrument.Brand = txtBrand.Text;
            double.TryParse(txtPret.Text, out double pret);
            instrument.Price = pret;

            AfisareInstrumente();
            AfisareProduseClient();

            AfiseazaMesaj("Instrument actualizat!", Brushes.Lime);
        }

        private void BtnBackFromProduct_Click(object sender, RoutedEventArgs e)
        {
            GridProductPage.Visibility = Visibility.Collapsed;
            GridClientHome.Visibility = Visibility.Visible;
        }

        private void BtnAddProductPageToCart_Click(object sender, RoutedEventArgs e)
        {
            cos.Add(txtProdusNume.Text);

            listCos.Items.Add(txtProdusNume.Text);

            AfiseazaMesaj("Produs adaugat in cos!", Brushes.Lime);
        }

    }
}