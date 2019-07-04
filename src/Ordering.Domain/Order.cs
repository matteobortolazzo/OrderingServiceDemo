using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Order
    {
        public int Id { get; set; }
        public Guid IdentityGuid { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public DateTime OrderDate { get; private set; }
        public Address DeliveryAddress { get; private set; }
        public OrderRejectionReason? RejectionReason { get; private set; }

        private int _orderStatusId;
        public OrderStatus OrderStatus { get; private set; }

        private int _userId;
        private int _productId;

        protected Order() { }
        public Order(Guid identityGuid, int quantity, decimal unitPrice, Address deliveryAddress, int productId, int userId)
        {
            IdentityGuid = identityGuid;
            Quantity = quantity;
            UnitPrice = unitPrice;
            DeliveryAddress = deliveryAddress;
            _productId = productId;
            _userId = userId;

            _orderStatusId = OrderStatus.Submitted.Id;
            OrderDate = DateTime.UtcNow;
        }

        public void Confirm()
        {
            if (_orderStatusId != OrderStatus.Submitted.Id)
            {
                throw new InvalidOperationException("Can confirm only submitted orders.");
            }

            _orderStatusId = OrderStatus.Confirmed.Id;
        }

        public void Reject(OrderRejectionReason rejectionReason)
        {
            if (_orderStatusId != OrderStatus.Submitted.Id)
            {
                throw new InvalidOperationException("Can reject only submitted orders.");
            }

            _orderStatusId = OrderStatus.Rejected.Id;
            RejectionReason = rejectionReason;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Order o))
            {
                return false;
            }

            return o.Id == Id;
        }
    }

    public enum OrderRejectionReason
    {
        TooManyPendingOrders = 1,
        TooManyProductsOrdered = 2
    }
}
