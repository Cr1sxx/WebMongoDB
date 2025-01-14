using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;


namespace WebMongoDB.Models
{
    public class ContextMongodb
    {
        public static string ConnectionString { get; set; }
        public static string DatabaseName { get; set; }
        public static bool IsSSL { get; set; }
        private IMongoDatabase _database { get; }

        static ContextMongodb()
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));
        }

        public ContextMongodb()
        {
            try
            {
                MongoClientSettings setting = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
          
                if (IsSSL)
                {
                    setting.SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                    };
                }

                var MongoClient = new MongoClient(setting);
                _database = MongoClient.GetDatabase(DatabaseName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IMongoCollection<Usuario> Usuario
        {
            get
            {
                return _database.GetCollection<Usuario>("Usuario");
            }
        }
        public IMongoCollection<Transacao> Transacao
        {
            get
            {
                return _database.GetCollection<Transacao>("Transação");
            }
        }
        public IMongoCollection<SomTransacao> SomaTransacao
        {
            get
            {
                return _database.GetCollection<SomTransacao>("Soma");
            }
        }
    }
}
