using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
    class QuoteMapper : IEntityTypeConfiguration<Quote> {
        public void Configure(EntityTypeBuilder<Quote> builder) {
            builder.ToTable("Quote");

           
            builder.Property(q => q.Id).IsRequired();
            builder.HasOne(q => q.Qoutee)
                   .WithMany()
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasKey(q=>q.Id);

            builder.HasOne(q => q.Creator).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(q => q.Guild).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Property(q => q.QuoteText).HasMaxLength(100).IsRequired();

            builder.Property(q => q.msgId).HasMaxLength(100).HasDefaultValue(0);

            builder.Ignore(q => q._context);
        }
    }
}
