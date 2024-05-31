using Gatherly.Domain.Entities;

namespace Gatherly.Persistence.Specifications;
internal class GatheringByIdSplitSpecification : Specification<Gathering>
{
    public GatheringByIdSplitSpecification(Guid id)
        :base(gathering => gathering.Id == id)
    {
        AddInclude(gathering => gathering.Creator);
        AddInclude(gathering => gathering.Attendees);
        AddInclude(gathering => gathering.Invitations);

        IsSplitQuery = true;
    }
}
