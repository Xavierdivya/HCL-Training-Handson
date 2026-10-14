using TrainingManagementSystem.Models;
using TrainingManagementSystem.Utilities;

namespace TrainingManagementSystem.Services;

public class TrainingService
{
    public void DisplayCourses(List<Course> courses)
    {
        Console.WriteLine("\n===== COURSE CATALOGUE =====");

        Console.WriteLine(
            $"{"Code",-8} {"Title",-30} {"Fee",12} " +
            $"{"Duration",10} {"Seats",12} {"Mode",12} {"Start",12}");

        foreach (Course c in courses)
        {
            Console.WriteLine(
                $"{c.CourseCode,-8} {c.Title,-30} " +
                $"{c.Fee,12:F2} {c.DurationDays,8} days " +
                $"{c.AvailableSeats,4}/{c.TotalSeats,-5} " +
                $"{(c.IsOnline ? "Online" : "Classroom"),-12} " +
                $"{c.StartDate:dd-MM-yyyy}");
        }
    }

    public void SearchCourse(List<Course> courses)
    {
        string term = InputValidator.ReadRequiredText(
            "Enter course code or title: ");

        foreach (Course c in courses)
        {
            if (c.CourseCode.Equals(
                    term, StringComparison.OrdinalIgnoreCase)
                || c.Title.Equals(
                    term, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(
                    $"{c.CourseCode} | {c.Title} | ₹{c.Fee:F2} | " +
                    $"{c.DurationDays} days | " +
                    $"{c.AvailableSeats}/{c.TotalSeats} seats | " +
                    $"{(c.IsOnline ? "Online" : "Classroom")}");

                return;
            }
        }

        Console.WriteLine("Course not found.");
    }

    public void SortCourses(List<Course> courses)
    {
        Console.WriteLine("1. Ascending fee");
        Console.WriteLine("2. Descending fee");

        int choice = InputValidator.ReadPositiveInteger("Sort order: ");

        if (choice != 1 && choice != 2)
        {
            Console.WriteLine("Invalid sort order.");
            return;
        }

        List<Course> sorted = new(courses);

        for (int i = 0; i < sorted.Count - 1; i++)
        {
            for (int j = i + 1; j < sorted.Count; j++)
            {
                bool shouldSwap =
                    (choice == 1 && sorted[i].Fee > sorted[j].Fee)
                    || (choice == 2 && sorted[i].Fee < sorted[j].Fee);

                if (shouldSwap)
                {
                    (sorted[i], sorted[j]) =
                        (sorted[j], sorted[i]);
                }
            }
        }

        foreach (Course c in sorted)
        {
            Console.WriteLine(
                $"{c.CourseCode} | {c.Title} | ₹{c.Fee:F2}");
        }
    }

    public void DisplayAvailableSeats(List<Course> courses)
    {
        foreach (Course c in courses)
        {
            Console.WriteLine(
                $"{c.CourseCode} {c.Title}: " +
                $"{c.AvailableSeats} seats available");
        }
    }

    public void DisplayCourseSummaries(List<Course> courses)
    {
        Console.WriteLine("\n===== COURSE SUMMARY RECORDS =====");

        foreach (Course c in courses)
        {
            CourseSummary summary = new(
                c.CourseCode, c.Title, c.Fee, c.AvailableSeats);

            CourseSummary equalSummary = new(
                c.CourseCode, c.Title, c.Fee, c.AvailableSeats);

            Console.WriteLine(
                $"{summary} | Equal-value record test: " +
                $"{summary == equalSummary}");

            CourseSummary changed = summary with
            {
                AvailableSeats = Math.Max(
                    0, summary.AvailableSeats - 1)
            };

            Console.WriteLine($"Modified copy using with: {changed}");
        }

        SessionDuration durationA = new(30);
        SessionDuration durationB = durationA;

        Console.WriteLine(
            $"SessionDuration value-type equality: " +
            $"{durationA.Equals(durationB)} ({durationA})");
    }
}