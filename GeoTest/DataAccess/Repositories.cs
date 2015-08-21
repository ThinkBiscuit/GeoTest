using System.Collections.Generic;
using System.Configuration;
using Dapper;
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

        public bool SaveCoords(GeoPoint geo)
        {
            using (var cn = new MySqlConnection(_connectionString))
            {
                const string sql = "INSERT INTO `geopoints` (`Long`, `Lat`) VALUES (@Long, @Lat);";

                return cn.Execute(sql, new {Long = geo.Long, Lat = geo.Lat}) > 0;
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