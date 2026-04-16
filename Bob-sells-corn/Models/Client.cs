using System.ComponentModel.DataAnnotations;

namespace Bob_sells_corn.Models;

public class Client
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = string.Empty;

    public DateTime? LastPurchaseDate { get; set; }

    public decimal TotalCornPurchased { get; set; }
}
