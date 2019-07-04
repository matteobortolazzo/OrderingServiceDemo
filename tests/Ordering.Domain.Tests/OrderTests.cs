using System;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Xunit;

namespace Ordering.Domain.Tests
{
    public class OrderTests
    {
        private Order CreateOrder()
        {
            return new Order(Guid.NewGuid(), 10, 10, new Address("A", "A", "A", "A", "A"), 0, 0);
        }

        [Fact]
        public void ConfirmConfirmedOrder_Throws_InvalidOperationException()
        {
            var order = CreateOrder();
            order.Confirm();

            Assert.Throws<InvalidOperationException>(() => order.Confirm());
        }

        [Fact]
        public void ConfirmRejectedOrder_Throws_InvalidOperationException()
        {
            var order = CreateOrder();
            order.Reject(OrderRejectionReason.TooManyPendingOrders);

            Assert.Throws<InvalidOperationException>(() => order.Confirm());
        }

        [Fact]
        public void RejectConfirmedOrder_Throws_InvalidOperationException()
        {
            var order = CreateOrder();
            order.Confirm();

            Assert.Throws<InvalidOperationException>(() => order.Reject(OrderRejectionReason.TooManyPendingOrders));
        }

        [Fact]
        public void RejectRejectedOrder_Throws_InvalidOperationException()
        {
            var order = CreateOrder();
            order.Reject(OrderRejectionReason.TooManyPendingOrders);

            Assert.Throws<InvalidOperationException>(() => order.Reject(OrderRejectionReason.TooManyPendingOrders));
        }
    }
}
