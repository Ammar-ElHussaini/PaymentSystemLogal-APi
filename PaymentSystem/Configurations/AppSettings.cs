public class AppSettings
{
    public LoggingSettings Logging { get; set; }
    public string AllowedHosts { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public WebhookSettings WebhookSettings { get; set; }
    public Jwt Jwt { get; set; }
    public TelegramSettings TelegramSettings { get; set; }
    public List<string> AllowedOrigins { get; set; }

}

public class LoggingSettings
{
    public LogLevelSettings LogLevel { get; set; }
}

public class WebhookSettings
{
    public string WebhookUrl { get; set; }
}
public class Jwt
{
    public string Issuer { get; set; }
    public string Key { get; set; }
    public string Audience { get; set; }
}

public class LogLevelSettings
{
    public string Default { get; set; }
    public string MicrosoftAspNetCore { get; set; }
}

public class TelegramSettings
{
    public string BotToken { get; set; }
    public string ChatId { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}
