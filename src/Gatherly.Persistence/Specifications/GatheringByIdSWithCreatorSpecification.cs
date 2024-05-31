using Gatherly.Domain.Entities;

namespace Gatherly.Persistence.Specifications;
internal class GatheringByIdSWithCreatorSpecification : Specification<Gathering>
{
    public GatheringByIdSWithCreatorSpecification(Guid gatheringId)
        : base(gathering => gathering.Id == gatheringId)
    {
        AddInclude(gathering => gathering.Creator);
    }
}
