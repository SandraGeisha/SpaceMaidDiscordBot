using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Exurb1aBot.Data.Mapper {
    public class CurrencyUserMapper : IEntityTypeConfiguration<CurrencyUser> {
        public void Configure(EntityTypeBuilder<CurrencyUser> builder) {
            var converter = new EnumToNumberConverter<CurrencyEnum,int>();

            builder.ToTable("CurrencyUser");
            builder.HasKey(cu=>cu.CUID);

            builder.HasOne(cu => cu.Owner)
                .WithMany(u=>u.Currencies)
                .HasPrincipalKey(u=>u.Id)
                .IsRequired(true);

            builder.Property<CurrencyEnum>(cu => cu.Currency)
                .HasConversion(converter);
        }
    }
}
