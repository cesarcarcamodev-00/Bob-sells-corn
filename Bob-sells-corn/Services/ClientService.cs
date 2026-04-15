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

    public async Task<Client?> GetClientByIdAsync(int clientId)
    {
        return await _context.Clients.FindAsync(clientId);
    }
}
