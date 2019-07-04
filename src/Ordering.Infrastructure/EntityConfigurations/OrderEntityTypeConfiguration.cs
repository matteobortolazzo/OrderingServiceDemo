using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Infrastructure.EntityConfigurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> orderConfiguration)
        {
            orderConfiguration.ToTable("Orders");

            orderConfiguration.HasKey(o => o.Id);

            //Address value object persisted as owned entity type supported since EF Core 2.0
            orderConfiguration.OwnsOne(o => o.DeliveryAddress);

            // Shadow properties
            orderConfiguration.Property<int>("UserId").IsRequired();
            orderConfiguration.Property<int>("ProductId").IsRequired();
            orderConfiguration.Property<int>("OrderStatusId").IsRequired();
            
            // Navigation properties
            orderConfiguration.HasOne(o => o.OrderStatus)
                .WithMany()
                .HasForeignKey("OrderStatusId");
        }
    }
}
