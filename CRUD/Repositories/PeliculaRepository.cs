using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.Data;
using CRUD.Models;
using MongoDB.Driver;

namespace CRUD.Repositories
{
    internal class PeliculaRepository
    {
        private IMongoCollection<Pelicula> collection;

        public PeliculaRepository()
        {
            collection = MongoDBConnection.Instancia.Peliculas;
        }

        public void Insertar(Pelicula pelicula)
        {
            collection.InsertOne(pelicula);
        }

        public List<Pelicula> ObtenerTodas()
        {
            return collection.Find(p => true).ToList();
        }

        public void Actualizar(string id, Pelicula pelicula)
        {
            collection.ReplaceOne(p => p.Id == id, pelicula);
        }

        public void Eliminar(string id)
        {
            collection.DeleteOne(p => p.Id == id);
        }
    }
}
