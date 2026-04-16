using Bob_sells_corn.Models;

namespace Bob_sells_corn.Services;

public interface IClientService
{
    Task<Client> GetOrCreateClientAsync(string clientName);
    Task<Client?> GetClientByIdAsync(Guid clientId);
    Task<Client> PurchaseCornAsync(Guid clientId);
}
