using System.Diagnostics.Metrics;

namespace Models
{
    public class Order
    {
        public string ClientName { get; set; }
        public List<Instrument> Produse { get; set; }
        public bool Confirmata { get; set; }

        public Order()
        {
            Produse = new List<Instrument>();
        }
    }
}
