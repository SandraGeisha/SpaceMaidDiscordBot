using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
    class EntityUserMapper : IEntityTypeConfiguration<EntityUser> {
        public void Configure(EntityTypeBuilder<EntityUser> builder) {
            builder.ToTable("Users");
           
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Username).HasMaxLength(100);
            builder.Property(u => u.Username).HasColumnName("username").IsRequired();

        }
    }
}
