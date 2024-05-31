using Gatherly.Domain.Entities;
using Gatherly.Domain.Primitives;

namespace Gatherly.Domain.Repositories;

public interface IRepository<T> where T : AggregateRoot 
{
}

public interface IGatheringRepository : IRepository<Gathering>
{
    Task<Gathering?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Gathering>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Gathering?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Gathering>> GetByIdCreatorAsync(Guid creatorId, CancellationToken cancellationToken = default);

    Task<Gathering?> GetByIdWithInvitationAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(Gathering gathering);

    void Remove(Gathering gathering);
}
