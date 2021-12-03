using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
    public class PortfolioMapper : IEntityTypeConfiguration<Portfolio> {
        public void Configure(EntityTypeBuilder<Portfolio> builder) {
            builder.ToTable("portfolio");

            builder.HasKey(p => new { p.ID, p.UserId }) ;

            builder.HasOne(p => p.User).WithOne()
               .HasPrincipalKey<EntityUser>(u => u.Id)
               .HasForeignKey<Portfolio>(p => p.UserId)
               .IsRequired();


            //builder.HasMany(p => p.StockPortfolios).WithOne(s => s.Portfolio).IsRequired(false);
        }
    }
}
