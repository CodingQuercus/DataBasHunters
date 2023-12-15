using Microsoft.AspNetCore.Http;

namespace DataBasHunters.Shared;

public class User
{
    public int Id { get; set; }

    public int Funds { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Profilepicture { get; set; }

    public IFormFile? imageFile { get; set; }
}

