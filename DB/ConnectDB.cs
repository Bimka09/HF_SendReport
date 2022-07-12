using Dapper;
using Npgsql;
using SendReportService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SendReportService.DB
{
    public class ConnectDB : IDisposable
    {
        public IDbConnection _dbConnection = new NpgsqlConnection();
        private bool disposed = false;
        public ConnectDB(string connectionString)
        {
            _dbConnection = new NpgsqlConnection(connectionString);
            _dbConnection.Open();
        }
        public List<Ratings> GetTopPlaces()
        {
            var query = @"select id, organization_id, rate from ratings
                        order by rate desc limit 10";
            return _dbConnection.Query<Ratings>(query).ToList();

        }
        public List<Ratings> GetPlacesInfo(List<Ratings> data)
        {
            var query = @"select id, adress, name from places
                        where id = @id";
            foreach (var item in data)
            {
                var result = _dbConnection.Query<PlaceInfo>(query, new { id = item.organization_id }).FirstOrDefault();
                item.adress = result.adress;
                item.name = result.name;
            }
            return data;

        }
       
        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbConnection.Close();// Освобождаем управляемые ресурсы
                }
                // освобождаем неуправляемые объекты
                disposed = true;
            }
        }
    }
}
