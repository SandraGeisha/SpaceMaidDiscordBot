using Microsoft.EntityFrameworkCore;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
    public class BannedWordMapper : IEntityTypeConfiguration<BannedWord> {
        public void Configure(EntityTypeBuilder<BannedWord> builder) {
            builder.ToTable("BannedWords");

            builder.Property(bw => bw.WordId).IsRequired();
            builder.HasKey(bw => bw.WordId);

            builder.Property(bw => bw.Word).IsRequired().HasMaxLength(100);
        }
    }
}
