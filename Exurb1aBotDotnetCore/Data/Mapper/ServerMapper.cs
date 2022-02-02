using Exurb1aBot.Model.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exurb1aBot.Data.Mapper {
  public class ServerMapper : IEntityTypeConfiguration<Server> {
    public void Configure(EntityTypeBuilder<Server> builder) {
      builder.ToTable("Server");
      builder.Property(p => p.ID).IsRequired();
      builder.HasKey(p => p.ID);
    }
  }
}
