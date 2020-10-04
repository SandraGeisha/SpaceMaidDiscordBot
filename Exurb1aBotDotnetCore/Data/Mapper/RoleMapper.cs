using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
    public class RoleMapper : IEntityTypeConfiguration<Role> {
        public void Configure(EntityTypeBuilder<Role> builder) {
            builder.ToTable("Roles");

            builder.HasKey(r => r.ID);
            builder.Property(r => r.ID).IsRequired();

            builder.Property(r => r.RoleType).IsRequired().HasConversion<string>();
            builder.Property(r => r.Name).IsRequired();
            builder.HasIndex(r => r.Name).IsUnique();
            builder.Property(r => r.ReactionEmote).IsRequired(false);
        }
    }
}
