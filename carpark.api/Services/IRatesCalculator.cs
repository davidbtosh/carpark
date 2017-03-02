using carpark.api.Models;

namespace carpark.api.Services
{
    public interface IRatesCalculator
    {
        Rate CalculateFlatRate(UserData ud);

        Rate CalculateHourlyRate(UserData ud);

        Rate CalculateRate(UserUI userEntry);

        string FilePath { get; set; }
    }
}
