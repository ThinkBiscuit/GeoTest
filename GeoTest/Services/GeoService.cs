using ServiceStack.ServiceInterface;

namespace GeoTest.Services
{
    public class SaveCoords
    {
        public string Long { get; set; }
        public string Lat { get; set; }
    }

    public class GeoService : Service
    {
        public object Post(SaveCoords request)
        {
            return new {Success = true};
        }
    }
}