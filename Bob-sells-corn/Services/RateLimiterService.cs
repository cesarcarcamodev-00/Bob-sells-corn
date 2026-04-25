using Microsoft.Extensions.Caching.Memory;

namespace Bob_sells_corn.Services;

public class RateLimiterService : IRateLimiterService
{
    private readonly IMemoryCache _memoryCache;
    private static readonly TimeSpan RateLimitTimeSpan = TimeSpan.FromMinutes(1); 
    private static readonly int RequestLimit = 3;

    public RateLimiterService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    public bool CanPurchase(Guid clientId)
    {
        if(_memoryCache.TryGetValue($"purchase:{clientId}", out int requests))
        {
            if(requests >= RequestLimit)
            {
                return false;
            }
        }
        return true;
    }

    public void RecordPurchase(Guid clientId)
    {

        if (_memoryCache.TryGetValue($"purchase:{clientId}", out int requests))
        {
            if(requests < RequestLimit)
                requests++;
        }
        else
        {
            requests = 1;
        }

        _memoryCache.Set($"purchase:{clientId}", requests, RateLimitTimeSpan);
    }

}
