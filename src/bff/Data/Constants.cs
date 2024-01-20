namespace BFF.Data;

public static class Constants
{
    public const string HttpMediaType = "application/json";
    
    public const string ConfigIamUrl = "ApiConfigs:Auth:Uri";
    public const string ConfigAuthority = "IAMAuthority";
    public const string ConfigAudience = "IAMAudience";

    public const string ConfigCategoryUrl = "ApiConfigs:Category:Uri";
    public const string ConfigPeopleUrl = "ApiConfigs:People:Uri";

    public const string ConfigSwaggerEnabled = "SwaggerEnabled";
    
    public const string HttpUrlAuthenticate = "api/account/authenticate";
    public const string HttpUrlRegister = "api/account/register";
    public const string HttpUrlRefresh = "api/account/refresh";
    
    public const string DevFrontendUrl = "http://localhost:3000";
    
}