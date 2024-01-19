using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure.Database.EntityConfigurations;

public class UserServersConfiguration : IEntityTypeConfiguration<UserServers>
{
    public void Configure(EntityTypeBuilder<UserServers> builder)
    {
        builder.HasKey(us => new { us.UserId, us.ServerId });

        builder
            .HasOne(us => us.Server)
            .WithMany(s => s.UserServers)
            .HasForeignKey(us => us.ServerId);

        builder.HasOne(us => us.User).WithMany(s => s.UserServers).HasForeignKey(us => us.UserId);
    }
}
