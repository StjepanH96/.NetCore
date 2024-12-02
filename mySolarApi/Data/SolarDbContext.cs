using Microsoft.EntityFrameworkCore;
using SolarApp.Models; 
using Microsoft.Data.Sqlite;

namespace SolarApp.Data
{
    public class SolarDbContext : DbContext
    {
        public SolarDbContext(DbContextOptions<SolarDbContext> options) : base(options) { }

        public override int SaveChanges()
        {
            using (var connection = this.Database.GetDbConnection() as SqliteConnection)
            {
                if (connection != null)
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "PRAGMA foreign_keys = ON;";
                        command.ExecuteNonQuery();
                    }
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            using (var connection = this.Database.GetDbConnection() as SqliteConnection)
            {
                if (connection != null)
                {
                    await connection.OpenAsync(cancellationToken);
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "PRAGMA foreign_keys = ON;";
                        await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SolarPowerPlant> SolarPowerPlants { get; set; }
        public DbSet<ProductionData> ProductionData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductionData>()
                .HasOne<SolarPowerPlant>()
                .WithMany() 
                .HasForeignKey(pd => pd.SolarPowerPlantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
