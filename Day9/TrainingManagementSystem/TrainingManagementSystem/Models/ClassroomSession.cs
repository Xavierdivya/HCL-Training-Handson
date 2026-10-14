using System;

namespace TrainingManagementSystem.Models;

public class ClassroomSession : TrainingSession
{
    public string ClassroomLocation { get; set; }

    public ClassroomSession(
        string courseCode,
        string title,
        int durationDays,
        DateTime startDate,
        string classroomLocation)
        : base(courseCode, title, durationDays, startDate)
    {
        ClassroomLocation = classroomLocation;
    }

    public override string GetSessionDetails()
    {
        return $"Classroom training at {ClassroomLocation}";
    }
}