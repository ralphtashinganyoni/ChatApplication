using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ChatApplication.Server.Domain.Entities;

namespace ChatApplication.Server.Persistence
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(2000); // Limit content size

            builder.Property(m => m.SentAt)
                .IsRequired();

            builder.Property(m => m.IsRead)
                .HasDefaultValue(false);

            builder.Property(m => m.IsDeletedBySender)
                .HasDefaultValue(false);

            builder.Property(m => m.IsDeletedByRecipient)
                .HasDefaultValue(false);

            // Table name
            builder.ToTable("Messages");
        }
    }
}
