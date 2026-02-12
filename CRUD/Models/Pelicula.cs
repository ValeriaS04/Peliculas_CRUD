using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CRUD.Models
{
    internal class Pelicula
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Titulo { get; set; }
        public string Genero { get; set; }
        public string Director { get; set; }
        public int Anio { get; set; }
        public int Duracion { get; set; }
        public byte[] Imagen { get; set; }

    }
}
