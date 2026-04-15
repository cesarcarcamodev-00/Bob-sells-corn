using System.ComponentModel.DataAnnotations;

namespace Bob_sells_corn.Data.DTOs;

public class LoginRequest
{
    [Required]
    public string ClientName { get; set; } = string.Empty;
}
