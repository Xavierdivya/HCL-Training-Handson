namespace TrainingManagementSystem.Models;

public class Course
{
    public string CourseCode { get; set; }
    public string Title { get; set; }
    public decimal Fee { get; set; }
    public int DurationDays { get; set; }
    public int TotalSeats { get; set; }
    public int EnrolledCount { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsOnline { get; set; }

    public int AvailableSeats => TotalSeats - EnrolledCount;

    public Course(
        string courseCode,
        string title,
        decimal fee,
        int durationDays,
        int totalSeats,
        int enrolledCount,
        DateTime startDate,
        bool isOnline)
    {
        CourseCode = courseCode;
        Title = title;
        Fee = fee;
        DurationDays = durationDays;
        TotalSeats = totalSeats;
        EnrolledCount = enrolledCount;
        StartDate = startDate;
        IsOnline = isOnline;
    }

    public override string ToString()
    {
        return $"{CourseCode} - {Title} - ₹{Fee:F2}";
    }
}