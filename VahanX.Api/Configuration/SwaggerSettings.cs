namespace VahanX.Api.Configuration;

/// <summary>
/// Swagger configuration settings.
/// </summary>
public class SwaggerSettings
{
    public const string SectionName = "Swagger";

    public bool Enabled { get; set; } = true;

    public string Title { get; set; } = "VahanX API";

    public string Version { get; set; } = "v1";

    public string Description { get; set; } = "VahanX - Nepal's Automotive Marketplace Platform API";
}
