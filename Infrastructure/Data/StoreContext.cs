using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductBrand> ProductBrands { get; set; }

    // Overide the migrations method
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Just for Sqlite.
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // Check for any usages of decimals, because Sqlite not support decimal and convert them to double.
            // Loop over all of our different properties.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Where: because we want to find the property types using the decimal.
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                foreach (var property in properties)
                {
                    // Conver to double.
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                }
            }

        }
    }
}
