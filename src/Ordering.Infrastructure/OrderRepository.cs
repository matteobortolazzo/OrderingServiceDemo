using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Infrastructure
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingContext _context;

        public OrderRepository(OrderingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Order> AddAsync(Order order)
        {
            var result = await _context.Orders.AddAsync(order);
            return result.Entity;
        }
        public void Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }

        public async Task<Order> GetAsync(Guid identity)
        {
            var order = await _context.Orders
                .Include(b => b.OrderStatus)
                .Include(b => b.DeliveryAddress)
                .SingleOrDefaultAsync(b => b.IdentityGuid == identity);

            return order;
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
