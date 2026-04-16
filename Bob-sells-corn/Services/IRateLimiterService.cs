using System.Security.Cryptography;

namespace Bob_sells_corn.Services;

public interface IRateLimiterService
{
    bool CanPurchase(Guid clientId);
    void RecordPurchase(Guid clientId);
}