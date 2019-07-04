using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MediatR;

namespace Ordering.App.Commands
{
    [DataContract]
    public class CreateOrderCommand : IRequest<bool>
    {
        // Foreign Keys

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        // Members

        [DataMember]
        public int Quantity { get; set; }

        // Address

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

    }
}
