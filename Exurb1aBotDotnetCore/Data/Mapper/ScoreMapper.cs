using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
    public class ScoreMapper : IEntityTypeConfiguration<Scores> {
        public void Configure(EntityTypeBuilder<Scores> builder) {
            builder.ToTable("Scores");
      builder.HasOne(s => s.Server).WithMany().HasForeignKey(s => s.ServerID);
            builder.HasKey(k => new {
              k.Id,
              k.ServerID
            });
            builder.Property(s => s.Quotes_Created).HasDefaultValue(0).IsRequired();
            builder.Property(s => s.Quoted).HasDefaultValue(0).IsRequired();
        }
    }
}
