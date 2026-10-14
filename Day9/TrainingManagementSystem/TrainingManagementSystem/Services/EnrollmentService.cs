
using System;
using System.Collections.Generic;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Services;
using TrainingManagementSystem.Utilities;

namespace TrainingManagementSystem.Services;

public class EnrollmentService
{
    private readonly List<Enrollment> _enrollments = new();
    private readonly LearnerService _learners;

    public IReadOnlyList<Enrollment> Enrollments => _enrollments;

    public EnrollmentService(LearnerService learners)
    {
        _learners = learners;
    }

    public void AddEnrollment(List<Course> courses)
    {
        Console.WriteLine("\n===== ENROLL IN COURSE =====");

        int id = InputValidator.ReadPositiveInteger("Enrollment ID: ");

        if (FindEnrollment(id) != null)
        {
            Console.WriteLine("Enrollment ID already exists.");
            return;
        }

        string learnerId =
            InputValidator.ReadRequiredText("Learner ID: ");

        if (!_learners.LearnerExists(learnerId))
        {
            Console.WriteLine("Learner not found. Register first.");
            return;
        }

        string code =
            InputValidator.ReadRequiredText("Course code: ");

        Course? course = FindCourse(courses, code);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        foreach (Enrollment existing in _enrollments)
        {
            if (existing.LearnerId.Equals(
                    learnerId,
                    StringComparison.OrdinalIgnoreCase)
                && existing.CourseCode.Equals(
                    course.CourseCode,
                    StringComparison.OrdinalIgnoreCase)
                && existing.Status == EnrollmentStatus.Active)
            {
                Console.WriteLine(
                    "Learner already has an active enrollment in this course.");
                return;
            }
        }

        if (course.AvailableSeats <= 0)
        {
            Console.WriteLine("No seats available.");
            return;
        }

        int daysUntilStart =
            (course.StartDate.Date - DateTime.Today).Days;

        if (daysUntilStart < 0)
        {
            Console.WriteLine("Course start date has passed.");
            return;
        }

        int groupSize = InputValidator.ReadPositiveInteger(
            "Group size (1 or more): ");
            try
            {
                var fee = FeeCalculatorService.CalculateFee(
                    course.Fee,
                    daysUntilStart,
                    groupSize,
                    course.IsOnline);

                FeeSummary summary = FeeCalculatorService.CreateSummary(
                    fee.BaseFee,
                    fee.TotalDiscount,
                    fee.FinalFee);

                Enrollment enrollment = new Enrollment(
                    id,
                    learnerId,
                    course.CourseCode,
                    DateTime.Now,
                    fee.BaseFee,
                    fee.TotalDiscount,
                    fee.FinalFee,
                    EnrollmentStatus.Active);

                _enrollments.Add(enrollment);
                course.EnrolledCount++;

                Console.WriteLine("\nEnrollment successful.");
                Console.WriteLine($"Enrollment ID: {id}");
                Console.WriteLine($"Learner ID: {learnerId}");
                Console.WriteLine($"Course: {course.Title}");

                FeeCalculatorService.DisplayFeeBreakdown(summary);

                Console.WriteLine($"Available seats: {course.AvailableSeats}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Could not calculate fee: {ex.Message}");
            }
    }

    public Enrollment? FindEnrollment(int id)
    {
        foreach (Enrollment enrollment in _enrollments)
        {
            if (enrollment.EnrollmentId == id)
            {
                return enrollment;
            }
        }

        return null;
    }

    public void DisplayEnrollments()
    {
        Console.WriteLine("\n===== ENROLLMENTS =====");

        if (_enrollments.Count == 0)
        {
            Console.WriteLine("No enrollments found.");
            return;
        }

        foreach (Enrollment enrollment in _enrollments)
        {
            DisplayEnrollment(enrollment);
        }
    }

    public void SearchEnrollment()
    {
        int id = InputValidator.ReadPositiveInteger(
            "Enrollment ID: ");

        Enrollment? enrollment = FindEnrollment(id);

        if (enrollment == null)
        {
            Console.WriteLine("Enrollment not found.");
        }
        else
        {
            DisplayEnrollment(enrollment);
        }
    }

    public void CancelEnrollment(List<Course> courses)
    {
        int id = InputValidator.ReadPositiveInteger(
            "Enrollment ID to cancel: ");

        Enrollment? enrollment = FindEnrollment(id);

        if (enrollment == null)
        {
            Console.WriteLine("Enrollment not found.");
            return;
        }

        if (enrollment.Status != EnrollmentStatus.Active)
        {
            Console.WriteLine(
                $"Cannot cancel an enrollment with status {enrollment.Status}.");
            return;
        }

        Course? course = FindCourse(courses, enrollment.CourseCode);

        if (course == null || course.EnrolledCount <= 0)
        {
            Console.WriteLine(
                "Course or seat count is invalid; cancellation aborted.");
            return;
        }

        enrollment.Status = EnrollmentStatus.Cancelled;
        course.EnrolledCount--;

        Console.WriteLine($"Enrollment {id} cancelled.");
        Console.WriteLine(
            $"Available seats: {course.AvailableSeats}");
    }

    public void CompleteEnrollment()
    {
        int id = InputValidator.ReadPositiveInteger(
            "Enrollment ID: ");

        Enrollment? enrollment = FindEnrollment(id);

        if (enrollment == null)
        {
            Console.WriteLine("Enrollment not found.");
            return;
        }

        if (enrollment.Status != EnrollmentStatus.Active)
        {
            Console.WriteLine(
                $"Cannot complete enrollment with status {enrollment.Status}.");
            return;
        }

        enrollment.Status = EnrollmentStatus.Completed;

        Console.WriteLine("Enrollment marked completed.");
    }

    private static Course? FindCourse(
        List<Course> courses,
        string code)
    {
        foreach (Course course in courses)
        {
            if (course.CourseCode.Equals(
                code,
                StringComparison.OrdinalIgnoreCase))
            {
                return course;
            }
        }

        return null;
    }

    private static void DisplayEnrollment(Enrollment enrollment)
    {
        Console.WriteLine(
            $"ID {enrollment.EnrollmentId} | " +
            $"Learner {enrollment.LearnerId} | " +
            $"Course {enrollment.CourseCode} | " +
            $"Fee ₹{enrollment.FinalFee:F2} | " +
            $"Enrollment {enrollment.Status} | " +
            $"Payment {enrollment.PaymentStatus}");
    }
}
