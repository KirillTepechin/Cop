using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class ProductDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Product> Products { set; get; }
        public virtual DbSet<Vendor> Vendors { set; get; }
    }
}