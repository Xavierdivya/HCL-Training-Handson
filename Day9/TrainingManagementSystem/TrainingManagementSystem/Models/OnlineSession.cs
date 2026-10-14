using System;

namespace TrainingManagementSystem.Models;

public class OnlineSession : TrainingSession
{
    public string Platform { get; set; }

    public OnlineSession(
        string courseCode,
        string title,
        int durationDays,
        DateTime startDate,
        string platform)
        : base(courseCode, title, durationDays, startDate)
    {
        Platform = platform;
    }

    public override string GetSessionDetails()
    {
        return $"Online training through {Platform}";
    }
}