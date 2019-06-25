using Exurb1aBot.Data.Mapper;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Exurb1aBot.Data {
    public class ApplicationDbContext : DbContext {

        public DbSet<Quote> Quote {get;set;}
        public DbSet<EntityUser> Users { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<BannedWord> BannedWords { get; set; }
        public DbSet<CurrencyUser> CurrencyUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Exurb1a;Integrated Security=True");
            SqliteConnection sql = new SqliteConnection($"Data Source=Exurb1a.db");
            sql.Open();
            optionsBuilder.UseSqlite(sql);
            optionsBuilder.EnableSensitiveDataLogging(true);
            this.Database.AutoTransactionsEnabled = true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<EntityUser>(new EntityUserMapper())
                        .ApplyConfiguration<Quote>(new QuoteMapper())
                        .ApplyConfiguration<Location>(new LocationMapper())
                        .ApplyConfiguration<BannedWord>(new BannedWordMapper())
                        .ApplyConfiguration<CurrencyUser>(new CurrencyUserMapper());
        }

        public void Initialize() {
            Quote.Load();
            Users.Load();
            Location.Load();
            BannedWords.Load();
            CurrencyUsers.Load();
        }
    }
}
