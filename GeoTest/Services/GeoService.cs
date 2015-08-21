using GeoTest.DataAccess;
using GeoTest.Models;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace GeoTest.Services
{
    [Route("/Geo/Save")]
    public class SaveCoords : GeoPoint
    {
    }

    [Route("/Geo/GetAll")]
    public class GetAllCoords
    {     
    }

    public class GeoService : Service
    {
        private readonly GeoRepository _geoRepository;

        public GeoService()
        {
            _geoRepository = new GeoRepository();
        }

        public object Post(SaveCoords request)
        {
            return new {Success = _geoRepository.SaveCoords(request)};
        }

        public object Get(GetAllCoords request)
        {

            var result = _geoRepository.GetAllCoords();
            return new { Success = result != null, Results = result };
        }
    }
}