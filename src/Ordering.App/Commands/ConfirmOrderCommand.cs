using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MediatR;

namespace Ordering.App.Commands
{
    [DataContract]
    public class ConfirmOrderCommand : IRequest<bool>
    {
        [DataMember]
        public Guid Identifier { get; set; }
    }
}
