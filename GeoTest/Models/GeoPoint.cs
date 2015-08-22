namespace GeoTest.Models
{
    public class GeoPoint
    {
        public int ID { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }

        public double Distance { get; set; }
    }
}