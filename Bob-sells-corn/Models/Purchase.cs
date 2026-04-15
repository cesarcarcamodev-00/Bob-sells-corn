using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bob_sells_corn.Models;

public class Purchase
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ClientId { get; set; }

    [ForeignKey(nameof(ClientId))]
    public Client? Client { get; set; }

    public DateTime Timestamp { get; set; }

    public decimal Quantity { get; set; }
}
