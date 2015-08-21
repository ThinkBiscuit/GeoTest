﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
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