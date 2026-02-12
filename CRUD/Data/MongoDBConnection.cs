using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.Models;
using MongoDB.Driver;

namespace CRUD.Data
{
    internal class MongoDBConnection
    {
        private static MongoDBConnection instancia;
        private IMongoDatabase database;

        private MongoDBConnection()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("PeliculasDB");
        }

        public static MongoDBConnection Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = new MongoDBConnection();
                return instancia;
            }
        }

        public IMongoCollection<Pelicula> Peliculas =>
            database.GetCollection<Pelicula>("Peliculas");
    }
}
