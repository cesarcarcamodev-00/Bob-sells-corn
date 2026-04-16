using Microsoft.Extensions.Caching.Memory;

namespace Bob_sells_corn.Services;

public class RateLimiterService : IRateLimiterService
{
    private readonly IMemoryCache _memoryCache;
    private static readonly TimeSpan RateLimitTimeSpan = TimeSpan.FromMinutes(1); // 1 request per minute

    public RateLimiterService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    public bool CanPurchase(Guid clientId)
    {
        return !_memoryCache.TryGetValue($"purchase:{clientId}", out _);
    }

    public void RecordPurchase(Guid clientId)
    {
        _memoryCache.Set($"purchase:{clientId}", true, RateLimitTimeSpan);
    }

}
