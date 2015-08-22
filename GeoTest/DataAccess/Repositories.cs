using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using Dapper;
using GeoTest.Helpers;
using GeoTest.Models;
using MySql.Data.MySqlClient;

namespace GeoTest.DataAccess
{
    public class GeoRepository
    {
        private readonly string _connectionString;

        public GeoRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["GeoTest"].ConnectionString ?? string.Empty;
        }

        public bool SaveCoords(GeoPoint geo, out int id)
        {
            id = -1;

            using (var cn = new MySqlConnection(_connectionString))
            {
                using (var transaction = new TransactionScope())
                {
                    const string sql = "INSERT INTO `geopoints` (`Long`, `Lat`) VALUES (@Long, @Lat);";

                    var success = cn.Execute(sql, new {Long = geo.Long, Lat = geo.Lat}) > 0;

                    var newId = cn.Query<ulong>("SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);").SingleOrDefault();
                    geo.ID = id = Convert.ToInt32(newId);

                    transaction.Complete();

                    return success;
                }
            }
        }

        public IEnumerable<GeoPoint> GetLocalCoords(GeoPoint geo)
        {
            var bounding = GeoCalculations.GetBoundingBoxMiles(new GeoCalculations.MapPoint {Latitude = geo.Lat, Longitude = geo.Long}, geo.Distance);

            using (var cn = new MySqlConnection(_connectionString))
            {//TODO: need to bound query with oblong coords to limit DB index lookup
                cn.Open();
                const string sql = "SELECT *,3956 * 2 * ASIN(SQRT(POWER(SIN((@Lat - `Lat`) * PI() / 180 / 2), 2) + COS(@Lat * PI() / 180) * COS(`Lat` *PI() / 180) * POWER(SIN((@Long - `Long`) * PI() / 180 / 2), 2) )) AS distance FROM `geopoints` WHERE `Long` BETWEEN @Long1 AND @long2 AND `Lat` BETWEEN @Lat1 AND @Lat2 HAVING distance < @Distance; ";
                return cn.Query<GeoPoint>(sql, new {@Distance= geo.Distance, @Lat = geo.Lat, @Long = geo.Long, @Long1 = bounding.MinPoint.Longitude, @Long2 = bounding.MaxPoint.Longitude, @Lat1 = bounding.MinPoint.Latitude, @Lat2 = bounding.MaxPoint.Latitude});
            }
        }

        public bool DeleteCoord(int id)
        {
            using (var cn = new MySqlConnection(_connectionString))
            {
                const string sql = "DELETE FROM `geopoints` WHERE `ID`=@id;";

                return cn.Execute(sql, new {ID = id}) > 0;
            }
        }

        public IEnumerable<GeoPoint> GetAllCoords()
        {
            using (var cn = new MySqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM `geopoints`;";

                return cn.Query<GeoPoint>(sql);
            }
        }
    }
}