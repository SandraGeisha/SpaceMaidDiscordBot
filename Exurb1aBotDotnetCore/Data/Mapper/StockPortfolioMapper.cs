using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
    public class StockPortfolioMapper : IEntityTypeConfiguration<StockPortfolio> {
        public void Configure(EntityTypeBuilder<StockPortfolio> builder) {
            builder.ToTable("StockPortfolio");
            builder.HasKey(p => p.ID);

            builder.Property(p => p.ID).ValueGeneratedOnAdd();
            
            builder.Property(p => p.BuyInPrice).IsRequired();
            builder.Property(p => p.Amount).IsRequired();

            builder.HasOne(p => p.Portfolio)
                .WithMany(p => p.StockPortfolios)
                .HasForeignKey(p => p.PortfolioID)
                .HasPrincipalKey(p => p.ID)
                .IsRequired();

            builder.HasOne(p => p.Stock)
                .WithMany()
                .HasForeignKey(p => p.Symbol)
                .HasPrincipalKey(s => s.Symbol)
                .IsRequired();
        }
    }
}
