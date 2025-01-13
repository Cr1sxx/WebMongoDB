using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace WebMongoDB.Models
{
    [Table("Transação")]
    public class Transacao
    {
        [Column("Id")]
        [Display(Name = "Código")]
        public Guid Id { get; set; }

        [Column("Data")]
        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [Column("Valor")]
        [Display(Name = "Valor")]
        public string Amount { get; set; }

        [Column("Categoria")]
        [Display(Name = "Categoria")]
        public string Category { get; set; }

        public static class MongoDbClassMap
        {
            public static void RegisterClassMaps()
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(Transacao)))
                {
                    BsonClassMap.RegisterClassMap<Transacao>(cm =>
                    {
                        cm.AutoMap();
                        cm.MapIdProperty(t => t.Id)
                          .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                    });
                }
            }
        }
    }
}
