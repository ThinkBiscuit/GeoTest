using GeoTest.DataAccess;
using GeoTest.Hubs;
using GeoTest.Models;
using Microsoft.AspNet.SignalR;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace GeoTest.Services
{
    [Route("/Geo/Save")]
    public class SaveCoords : GeoPoint
    {
    }

    [Route("/Geo/GetLocal")]
    public class GetCoords : GeoPoint
    {
    }

    [Route("/Geo/GetAll")]
    public class GetAllCoords
    {     
    }

    [Route("/Geo/Delete")]
    public class DeleteCoord
    {
        public int ID { get; set; }
    }

    public class GeoService : Service
    {
        private readonly GeoRepository _geoRepository;

        public GeoService()
        {
            _geoRepository = new GeoRepository();
        }

        public object Delete(DeleteCoord request)
        {
            var success = _geoRepository.DeleteCoord(request.ID);

            if (success)
                PushPinRemovedNotification(request.ID);

            return new {Success = success};
        }

        public object Post(SaveCoords request)
        {
            int id;
            var success = _geoRepository.SaveCoords(request, out id);

            if (success)
            {
                request.ID = id;
                PushPinDropNotification(request);
            }

            return new {Success = success, Results = success ? request : null};
        }

        public object Get(GetAllCoords request)
        {

            var result = _geoRepository.GetAllCoords();
            return new { Success = result != null, Results = result };
        }

        public object Get(GetCoords request)
        {

            var result = _geoRepository.GetLocalCoords(request);

            return new { Success = result != null, Results = result };
        }

        private void PushPinDropNotification(GeoPoint point)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<GeoHub>();

            hub.Clients.Group("GeoPlacements").pinPlaced(point);
        }

        private void PushPinRemovedNotification(int point)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<GeoHub>();

            hub.Clients.Group("GeoPlacements").pinRemoved(point);
        }
    }
}