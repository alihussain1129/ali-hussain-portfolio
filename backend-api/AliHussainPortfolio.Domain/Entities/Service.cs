namespace AliHussainPortfolio.Domain.Entities;

public class Service
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsPublished { get; set; } = true;
}