using System;

namespace LibrarieModele
{
    // Am scos enumerarile in afara clasei pentru a fi folosite mai usor
    [Flags]
    public enum Instrument_Category
    {
        Guitars = 1, Drums = 2, Keyboards = 4, Mics = 8,
        Amps = 16, Synths = 32, Cables = 64, Strings = 128, Accessories = 256
    }

    [Flags]
    public enum CustomOrderColor
    {
        Blue = 1, Red = 2, Green = 4, Black = 8,
        Purple = 16, White = 32, Orange = 64
    }

    public class Instrument
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public Instrument_Category Category { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public CustomOrderColor Color { get; set; }

        public double Final_Price()
        {
            // Formula corectata pentru aplicarea discount-ului procentual
            return Price - (Price * (Discount / 100));
        }

        public Instrument()
        {
            ID = 0;
            Name = string.Empty;
            Brand = string.Empty;
            Price = 0;
            Description = string.Empty;
            Quantity = 0;
            Discount = 0;
            Category = Instrument_Category.Guitars; // Valoare default
        }

        public Instrument(string name, string brand, double price, int id, double discount, int quantity)
        {
            ID = id;
            Name = name;
            Brand = brand;
            Price = price;
            Discount = discount;
            Quantity = quantity;
        }

        public string Info()
        {
            return $"ID:{ID} | Nume:{Name} | Brand:{Brand} | Pret:{Price} RON | Cantitate:{Quantity}";
        }
    }
}