using Exurb1aBot.Data.Mapper;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Exurb1aBot.Data {
    public class ApplicationDbContext : DbContext {

        public DbSet<Quote> Quote {get;set;}
        public DbSet<EntityUser> Users { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Scores> Scores { get; set; }

        private readonly IConfiguration _config;

        public ApplicationDbContext(IConfiguration configuration) {
          _config = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            #if Release
                  optionsBuilder.UseSqlServer(_config.GetConnectionString("DBLive"));
            #elif Staging
                  optionsBuilder.UseSqlServer(_config.GetConnectionString("DBStaging"));
            #else
                  optionsBuilder.UseSqlServer(_config.GetConnectionString("DBDevelopment"));
            #endif
            optionsBuilder.EnableSensitiveDataLogging(true);
            this.Database.AutoTransactionsEnabled = true;
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<EntityUser>(new EntityUserMapper())
                  .ApplyConfiguration<Quote>(new QuoteMapper())
                  .ApplyConfiguration<Location>(new LocationMapper())
                  .ApplyConfiguration<Scores>(new ScoreMapper());
        }

        public void Initialize() {
            Quote.Load();
            Users.Load();
            Location.Load();
            Scores.Load();
        }
    }
}
