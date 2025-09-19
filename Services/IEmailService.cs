namespace star_events.Services;

public interface IEmailService
{
    Task SendUserCredentialsAsync(string email, string firstName, string lastName, string password);
    Task SendEmailAsync(string to, string subject, string body);
}
