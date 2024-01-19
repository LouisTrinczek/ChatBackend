using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure.Database.EntityConfigurations;

public class UserMessageConfiguration : IEntityTypeConfiguration<UserMessages>
{
    public void Configure(EntityTypeBuilder<UserMessages> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .HasOne(um => um.Receiver)
            .WithMany(um => um.UserMessages)
            .HasForeignKey(c => c.ReceiverId);

        builder
            .HasOne(um => um.Message)
            .WithMany(um => um.UserMessages)
            .HasForeignKey(c => c.MessageId);
    }
}
