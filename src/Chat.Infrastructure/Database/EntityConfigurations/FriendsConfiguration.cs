using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure.Database.EntityConfigurations;

public class FriendsConfiguration : IEntityTypeConfiguration<Friends>
{
    public void Configure(EntityTypeBuilder<Friends> builder)
    {
        builder.HasKey(c => c.Id);
    }
}
