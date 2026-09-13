namespace NewtonCatalog.Api.Domain;

public class VideoGame
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Developer { get; set; }

    public Platform Platform { get; set; }

    public Genre Genre { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int Rating { get; set; }

    public decimal Price { get; set; }
}
