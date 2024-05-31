using Gatherly.Domain.Entities;
using Gatherly.Domain.Repositories;
using Gatherly.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace Gatherly.Persistence.Repository;
public sealed class CachedMemberRepository : IMemberRepository
{
    private readonly MemberRepository _decorated;
    private readonly IMemoryCache _memoryCache;

    public CachedMemberRepository(MemberRepository decorated, IMemoryCache memoryCache)
    {
        _decorated = decorated;
        _memoryCache = memoryCache;
    }

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string key = $"member-{id}";

        return await _memoryCache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            return await _decorated.GetByIdAsync(id, cancellationToken);
        });
    }

    public Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default) => _decorated.IsEmailUniqueAsync(email, cancellationToken);

    public void Add(Member member) => _decorated.Add(member);
    public void Update(Member member) => _decorated.Update(member);
}
