using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
    public class LocationMapper : IEntityTypeConfiguration<Location> {
        public void Configure(EntityTypeBuilder<Location> builder) {
            builder.ToTable("Location");

            builder.HasOne(l => l.User).WithOne().IsRequired().HasForeignKey<Location>(l => l.Id);

            builder.Property(l => l.LocationName).IsRequired().HasColumnName("Location");
        }
    }
}
