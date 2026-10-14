
using System;
using System.Collections.Generic;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services;

public class CourseService
{
    public static void Display(List<Course> courses)
    {
        Console.WriteLine("\n===== COURSE LIST =====");

        if (courses.Count == 0)
        {
            Console.WriteLine("No courses available.");
            return;
        }

        foreach (Course course in courses)
        {
            Console.WriteLine(
                $"{course.CourseCode}: {course.Title} | " +
                $"Fee: ₹{course.Fee:F2} | " +
                $"Start: {course.StartDate:dd-MM-yyyy} | " +
                $"Mode: {(course.IsOnline ? "Online" : "Classroom")} | " +
                $"Seats: {course.TotalSeats - course.EnrolledCount}/{course.TotalSeats} available");
        }
    }

    public static void Search(List<Course> courses)
    {
        Console.Write("Enter course code or title: ");
        string input = (Console.ReadLine() ?? "").Trim();

        foreach (Course course in courses)
        {
            if (string.Equals(course.CourseCode, input,
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(course.Title, input,
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Course Code: {course.CourseCode}");
                Console.WriteLine($"Title: {course.Title}");
                Console.WriteLine($"Fee: ₹{course.Fee:F2}");
                Console.WriteLine($"Start Date: {course.StartDate:dd-MM-yyyy}");
                Console.WriteLine($"Duration: {course.DurationDays} days");
                Console.WriteLine($"Mode: {(course.IsOnline ? "Online" : "Classroom")}");
                Console.WriteLine(
                    $"Available Seats: {course.TotalSeats - course.EnrolledCount}");
                return;
            }
        }

        Console.WriteLine("Course not found.");
    }

    public static void Sorting(List<Course> courses)
    {
        List<Course> sorted = new List<Course>(courses);

        // Sort by fee using loops, without LINQ.
        for (int i = 0; i < sorted.Count - 1; i++)
        {
            for (int j = i + 1; j < sorted.Count; j++)
            {
                if (sorted[i].Fee > sorted[j].Fee)
                {
                    Course temp = sorted[i];
                    sorted[i] = sorted[j];
                    sorted[j] = temp;
                }
            }
        }

        Console.WriteLine("\n===== COURSES SORTED BY FEE =====");

        foreach (Course course in sorted)
        {
            Console.WriteLine($"{course.Title} - ₹{course.Fee:F2}");
        }
    }

    public static void AvailableSeats(List<Course> courses)
    {
        Console.WriteLine("\n===== AVAILABLE SEATS =====");

        foreach (Course course in courses)
        {
            Console.WriteLine(
                $"{course.Title}: " +
                $"{course.TotalSeats - course.EnrolledCount} seats available");
        }
    }
}
