using System;

namespace TrainingManagementSystem.Models;

public abstract class TrainingSession
{
    public string CourseCode { get; set; }
    public string Title { get; set; }
    public int DurationDays { get; set; }
    public DateTime StartDate { get; set; }

    protected TrainingSession(
        string courseCode,
        string title,
        int durationDays,
        DateTime startDate)
    {
        CourseCode = courseCode;
        Title = title;
        DurationDays = durationDays;
        StartDate = startDate;
    }

    public abstract string GetSessionDetails();

    public virtual void DisplayDetails()
    {
        Console.WriteLine($"\nCourse Code : {CourseCode}");
        Console.WriteLine($"Title       : {Title}");
        Console.WriteLine($"Duration    : {DurationDays} days");
        Console.WriteLine($"Start Date  : {StartDate:dd-MM-yyyy}");
        Console.WriteLine($"Session     : {GetSessionDetails()}");
    }
}