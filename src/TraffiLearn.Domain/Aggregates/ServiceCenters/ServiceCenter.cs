using TraffiLearn.Domain.Aggregates.ServiceCenters.ValueObjects;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters
{
    public sealed class ServiceCenter : AggregateRoot<ServiceCenterId>
    {
        private ServiceCenter()
            : base(new(Guid.Empty))
        { }
    }
}
