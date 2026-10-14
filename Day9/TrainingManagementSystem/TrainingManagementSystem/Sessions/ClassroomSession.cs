using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Sessions;

public class ClassroomSession : CourseSession
{
    public string Location { get; }

    public ClassroomSession(
        string courseCode,
        DateTime startDate,
        SessionDuration duration,
        string location)
        : base(courseCode, startDate, duration)
    {
        Location = location;
    }

    public override decimal CalculateSessionCharge()
    {
        return 800m;
    }

    public override string GetDetails()
    {
        return $"Classroom at {Location}; " +
               $"charge ₹{CalculateSessionCharge():F2}";
    }
}