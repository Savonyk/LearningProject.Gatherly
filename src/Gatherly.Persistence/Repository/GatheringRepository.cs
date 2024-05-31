using Gatherly.Domain.Entities;
using Gatherly.Domain.Repositories;
using Gatherly.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Gatherly.Persistence.Repository;

internal sealed class GatheringRepository : IGatheringRepository
{
    private readonly ApplicationDbContext _context;

    public GatheringRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<Gathering>> GetByNameAsync(string name, CancellationToken cancellationToken = default) => 
        await ApplySpecification(new GatheringByNameSpecification(name))
            .ToListAsync(cancellationToken);

    public async Task<Gathering?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => 
        await ApplySpecification(new GatheringByIdSplitSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Gathering?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default) => 
        await ApplySpecification(new GatheringByIdSWithCreatorSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Gathering?> GetByIdWithInvitationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Gathering>()

            .Include(gathering => gathering.Invitations)
            .FirstOrDefaultAsync(gathering => gathering.Id == id, cancellationToken);
    }

    public void Add(Gathering gathering)
    {
        _context.Set<Gathering>().Add(gathering);
    }

    public void Remove(Gathering gathering)
    {
        _context.Set<Gathering>().Remove(gathering);
    }

    public async Task<List<Gathering>> GetByIdCreatorAsync(Guid creatorId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Gathering>()
            .Where(gathering => gathering.Creator.Id == creatorId)
            .ToListAsync();
    }

    private IQueryable<Gathering> ApplySpecification(Specification<Gathering> specification)
    {
        return SpecificationEvaluator.GetQuery( _context.Set<Gathering>(), specification);
    }
}
