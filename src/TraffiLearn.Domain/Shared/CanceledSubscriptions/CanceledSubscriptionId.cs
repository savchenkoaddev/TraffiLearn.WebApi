using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraffiLearn.Domain.Shared.CanceledSubscriptions
{
    public readonly record struct CanceledSubscriptionId(Guid Value);
}
