using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
    public class ScoreMapper : IEntityTypeConfiguration<Scores> {
        public void Configure(EntityTypeBuilder<Scores> builder) {
            builder.ToTable("Scores");
            builder.Property(s => s.Quotes_Created).HasDefaultValue(0).IsRequired();
            builder.Property(s => s.Quoted).HasDefaultValue(0).IsRequired();
            builder.Property(s => s.VC_Score).HasDefaultValue(0).IsRequired();
        }
    }
}
