using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Sessions;

public class OnlineSession : CourseSession
{
    public string Platform { get; }

    public OnlineSession(
        string courseCode,
        DateTime startDate,
        SessionDuration duration,
        string platform)
        : base(courseCode, startDate, duration)
    {
        Platform = platform;
    }

    public override decimal CalculateSessionCharge()
    {
        return 200m;
    }

    public override string GetDetails()
    {
        return $"Online via {Platform}; " +
               $"charge ₹{CalculateSessionCharge():F2}";
    }
}