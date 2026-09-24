namespace AliHussainPortfolio.Domain.Entities;

public class ProjectTechnology
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    public string TechnologyName { get; set; } = string.Empty;
}