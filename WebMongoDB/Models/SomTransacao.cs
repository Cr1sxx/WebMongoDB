using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace WebMongoDB.Models
{
    [Table("Soma Transações")]
    public class SomTransacao
    {
        [Column("Id")]
        [Display(Name = "Código")]
        public Guid Id { get; set; }

        [Column("Total")]
        [Display(Name = "Total")]
        public double Total { get; set; }

        public static class MongoDbClassMap
        {
            public static void RegisterClassMaps()
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(SomTransacao)))
                {
                    BsonClassMap.RegisterClassMap<SomTransacao>(cm =>
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
