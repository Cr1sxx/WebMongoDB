using Microsoft.EntityFrameworkCore;

namespace WebMongoDB.Data
{
    public class WebMongoDBContext : DbContext
    {
        public WebMongoDBContext (DbContextOptions<WebMongoDBContext> options)
            : base(options)
        {
        }

        public DbSet<WebMongoDB.Models.Usuario> Usuario { get; set; } = default!;
        public DbSet<WebMongoDB.Models.Transacao> Transacao { get; set; } = default!;
        public DbSet<WebMongoDB.Models.SomTransacao> SomTransacao { get; set; } = default!;
    }
}
