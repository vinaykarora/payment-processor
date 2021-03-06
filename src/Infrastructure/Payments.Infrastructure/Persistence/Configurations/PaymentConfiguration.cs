﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.Property(t => t.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .OwnsOne(b => b.Status);
        }
    }
}
