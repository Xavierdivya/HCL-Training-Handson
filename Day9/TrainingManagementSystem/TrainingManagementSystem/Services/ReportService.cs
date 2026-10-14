
using System;
using System.Collections.Generic;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services;

public class ReportService
{
    public void DisplayReports(
        List<Course> courses,
        EnrollmentService enrollmentService,
        PaymentService paymentService)
    {
        Console.WriteLine("\n========== TRAINING MANAGEMENT REPORTS ==========");

        // Course Report
        Console.WriteLine("\n--- COURSE REPORT ---");

        Console.WriteLine($"Total Courses: {courses.Count}");

        int totalSeats = 0;
        int availableSeats = 0;
        decimal totalCourseFees = 0;

        foreach (Course course in courses)
        {
            totalSeats += course.TotalSeats;
            availableSeats += course.AvailableSeats;
            totalCourseFees += course.Fee;
        }

        Console.WriteLine($"Total Seats: {totalSeats}");
        Console.WriteLine($"Available Seats: {availableSeats}");
        Console.WriteLine($"Occupied Seats: {totalSeats - availableSeats}");
        Console.WriteLine($"Sum of Course Fees: {totalCourseFees:C}");

        // Enrollment Report
        Console.WriteLine("\n--- ENROLLMENT REPORT ---");

        int totalEnrollments = 0;
        int activeEnrollments = 0;
        int cancelledEnrollments = 0;
        int completedEnrollments = 0;

        foreach (Enrollment enrollment in enrollmentService.Enrollments)
        {
            totalEnrollments++;

            switch (enrollment.Status)
            {
                case EnrollmentStatus.Active:
                    activeEnrollments++;
                    break;

                case EnrollmentStatus.Cancelled:
                    cancelledEnrollments++;
                    break;

                case EnrollmentStatus.Completed:
                    completedEnrollments++;
                    break;
            }
        }

        Console.WriteLine($"Total Enrollments: {totalEnrollments}");
        Console.WriteLine($"Active Enrollments: {activeEnrollments}");
        Console.WriteLine($"Cancelled Enrollments: {cancelledEnrollments}");
        Console.WriteLine($"Completed Enrollments: {completedEnrollments}");

        // Payment Report
        Console.WriteLine("\n--- PAYMENT REPORT ---");

        paymentService.DisplayHistory();

        Console.WriteLine("\n========== END OF REPORT ==========");
    }
}
