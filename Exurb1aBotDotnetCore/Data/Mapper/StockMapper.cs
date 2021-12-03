using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
    public class StockMapper : IEntityTypeConfiguration<Stock> {
        public void Configure(EntityTypeBuilder<Stock> builder) {
            builder.ToTable("Stock");

            builder.HasKey(s => s.Symbol);
            builder.Property(s => s.Symbol).IsRequired();

            builder.Property(s => s.Type).HasDefaultValue(StockType.Crypto);
        }
    }
}
