namespace star_events.Services;

public interface IPasswordService
{
    string GenerateRandomPassword(int length = 8);
}
