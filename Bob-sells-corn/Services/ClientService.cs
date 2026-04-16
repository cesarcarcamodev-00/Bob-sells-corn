using Bob_sells_corn.Data;
using Bob_sells_corn.Models;
using Microsoft.EntityFrameworkCore;

namespace Bob_sells_corn.Services;

public class ClientService : IClientService
{
    private readonly AppDbContext _context;

    public ClientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Client> GetOrCreateClientAsync(string clientName)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Name.Equals(clientName, StringComparison.CurrentCultureIgnoreCase));

        if (client == null)
        {
            client = new Client
            {
                Name = clientName,
                TotalCornPurchased = 0
            };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        return client;
    }

    public async Task<Client?> GetClientByIdAsync(Guid clientId)
    {
        return await _context.Clients.FindAsync(clientId);
    }

    public async Task<Client> PurchaseCornAsync(Guid clientId)
    {
        var client = await _context.Clients.FindAsync(clientId) ?? throw new KeyNotFoundException($"Client {clientId} not found");
        var purchase = new Purchase
        {
            ClientId = clientId,
            Timestamp = DateTime.UtcNow,
            Quantity = 1
        };

        client.LastPurchaseDate = DateTime.UtcNow;
        client.TotalCornPurchased += 1;

        _context.Purchases.Add(purchase);
        await _context.SaveChangesAsync();

        return client;
    }

    public async Task<decimal> GetTotalCornPurchasedAsync(Guid clientId)
    {
        var client = await _context.Clients.FindAsync(clientId) ?? throw new KeyNotFoundException($"Client {clientId} not found");

        return client.TotalCornPurchased;

    }
}
