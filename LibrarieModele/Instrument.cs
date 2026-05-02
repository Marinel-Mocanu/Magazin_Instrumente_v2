using System;

namespace LibrarieModele
{

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
        private const char SEPARATOR_PRINCIPAL_FISIER = ';';

        private const int ID_INDEX = 0;
        private const int NAME_INDEX = 1;
        private const int BRAND_INDEX = 2;
        private const int CATEGORY_INDEX = 3;
        private const int PRICE_INDEX = 4;
        private const int DESCRIPTION_INDEX = 5;
        private const int QUANTITY_INDEX = 6;
        private const int DISCOUNT_INDEX = 7;
        private const int COLOR_INDEX = 8;

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
  
            return Price - (Price * (Discount / 100));
        }

        public Instrument()
        {
            ID = 0;
            Name = string.Empty;
            Brand = string.Empty;
            Category = Instrument_Category.Guitars;
            Price = 0;
            Description = string.Empty;
            Quantity = 0;
            Discount = 0;
            Color = CustomOrderColor.Black;
        }

        public Instrument(int id, string name, string brand, Instrument_Category category,
                        double price, string description, int quantity,
                        double discount, CustomOrderColor color)
        {
            ID = id;
            Name = name;
            Brand = brand;
            Category = category;
            Price = price;
            Description = description;
            Quantity = quantity;
            Discount = discount;
            Color = color;
        }
        public Instrument(string linieFisier)
        {
            string[] date = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);

            ID = int.Parse(date[ID_INDEX]);
            Name = date[NAME_INDEX];
            Brand = date[BRAND_INDEX];
            Category = (Instrument_Category)int.Parse(date[CATEGORY_INDEX]);
            Price = double.Parse(date[PRICE_INDEX]);
            Description = date[DESCRIPTION_INDEX];
            Quantity = int.Parse(date[QUANTITY_INDEX]);
            Discount = double.Parse(date[DISCOUNT_INDEX]);
            Color = (CustomOrderColor)int.Parse(date[COLOR_INDEX]);
        }

        // salvare in fisier
        public string ConversieLaSirPentruFisier()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}",
                SEPARATOR_PRINCIPAL_FISIER,
                ID,
                Name ?? "NECUNOSCUT",
                Brand ?? "NECUNOSCUT",
                (int)Category,
                Price,
                Description ?? "",
                Quantity,
                Discount,
                (int)Color);
        }

        public string Info()
        {
            return $"ID:{ID} | Nume:{Name} | Brand:{Brand} | Pret:{Price} RON | Cantitate:{Quantity}";
        }
    }
}