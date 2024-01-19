using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure.Database.EntityConfigurations;

public class ChannelMessageConfiguration : IEntityTypeConfiguration<ChannelMessage>
{
    public void Configure(EntityTypeBuilder<ChannelMessage> builder)
    {
        builder
            .HasOne(um => um.Channel)
            .WithMany(um => um.ChannelMessages)
            .HasForeignKey(c => c.ChannelId);

        builder
            .HasOne(um => um.Message)
            .WithMany(um => um.ChannelMessages)
            .HasForeignKey(c => c.MessageId);
    }
}
