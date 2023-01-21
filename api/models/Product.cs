using System.ComponentModel.DataAnnotations.Schema;


namespace APIDemo.Models
{
    // TODO: use annotation to ajust tablename to camelCase

    [Table("produto")]
    public class Product
    {
        [Key]
        public int id { get; set; }
        public string nome { get; set; }
        public decimal preco { get; set; }
        public string descricao { get; set; }
        public string foto { get; set; }
        public string formatoImagem { get; set; }
        public DateTime dataCriacao { get; set; }
    }

    class CommerceDB : DbContext
    {
        public CommerceDB(DbContextOptions<CommerceDB> options) : base(options) { }
        public DbSet<Product> Produto => Set<Product>();
    }

}
