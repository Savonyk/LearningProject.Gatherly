using Gatherly.Domain.Entities;
using Gatherly.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gatherly.Persistence.Repository;

internal sealed class GatheringRepository : IGatheringRepository
{
    private readonly ApplicationDbContext _context;

    public GatheringRepository(ApplicationDbContext context) => _context = context;

    public async Task<Gathering?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Gathering>()
            .Include(gathering => gathering.Creator)
            .FirstOrDefaultAsync(gathering => gathering.Id == id, cancellationToken);
    }

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
}
