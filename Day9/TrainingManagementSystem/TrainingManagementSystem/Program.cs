
using System;
using System.Collections.Generic;
using TrainingManagementSystem.Interfaces;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Services;
using TrainingManagementSystem.Utilities;

namespace TrainingManagementSystem;

public class Program
{
    public static void Main(string[] args)
    {
        // Sample course data
        List<Course> courses = new()
        {
            new Course(
                "CS101",
                "C# Fundamentals",
                5000m,
                30,
                3,
                0,
                DateTime.Today.AddDays(30),
                true),

            new Course(
                "CS102",
                "Object Oriented Programming",
                6500m,
                45,
                2,
                0,
                DateTime.Today.AddDays(10),
                false),

            new Course(
                "CS103",
                "Database Management",
                4000m,
                25,
                5,
                0,
                DateTime.Today.AddDays(20),
                true),

            new Course(
                "CS104",
                "Web Development",
                5500m,
                35,
                4,
                0,
                DateTime.Today.AddDays(25),
                true),

            new Course(
                "CS105",
                "Software Testing",
                4500m,
                20,
                3,
                0,
                DateTime.Today.AddDays(18),
                false)
        };

        // Initialize services
        TrainingService trainingService = new();
        LearnerService learnerService = new();

        EnrollmentService enrollmentService =
            new EnrollmentService(learnerService);

        PaymentService paymentService = new();
        ReportService reportService = new();

        ITrainingSessionService sessionService =
            new TrainingSessionService();

        TrainingManager trainingManager =
            new TrainingManager(sessionService);

        int choice = 0;

        do
        {
            Console.WriteLine(
                "\n========== TRAINING MANAGEMENT SYSTEM ==========");

            Console.WriteLine("1. Display Courses");
            Console.WriteLine("2. Search Course");
            Console.WriteLine("3. Sort Courses by Fee");
            Console.WriteLine("4. Display Available Seats");
            Console.WriteLine("5. Register Learner");
            Console.WriteLine("6. Display Learners");
            Console.WriteLine("7. Search Learner");
            Console.WriteLine("8. Enroll in a Course");
            Console.WriteLine("9. Display All Enrollments");
            Console.WriteLine("10. Search Enrollment");
            Console.WriteLine("11. Cancel Enrollment");
            Console.WriteLine("12. Complete Enrollment");
            Console.WriteLine("13. Display Training Sessions");
            Console.WriteLine("14. Display Course Summaries");
            Console.WriteLine("15. Process Simulated Payment");
            Console.WriteLine("16. View Payment History");
            Console.WriteLine("17. Display Reports");
            Console.WriteLine("18. Demonstrate Generic Methods");
            Console.WriteLine("19. Exit");

            Console.Write("Enter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine(
                    "Invalid input. Please enter a number from 1 to 19.");

                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        trainingService.DisplayCourses(courses);
                        break;

                    case 2:
                        trainingService.SearchCourse(courses);
                        break;

                    case 3:
                        trainingService.SortCourses(courses);
                        break;

                    case 4:
                        trainingService.DisplayAvailableSeats(courses);
                        break;

                    case 5:
                        learnerService.RegisterLearner();
                        break;

                    case 6:
                        learnerService.DisplayLearners();
                        break;

                    case 7:
                        learnerService.SearchLearner();
                        break;

                    case 8:
                        enrollmentService.AddEnrollment(courses);
                        break;

                    case 9:
                        enrollmentService.DisplayEnrollments();
                        break;

                    case 10:
                        enrollmentService.SearchEnrollment();
                        break;

                    case 11:
                        enrollmentService.CancelEnrollment(courses);
                        break;

                    case 12:
                        enrollmentService.CompleteEnrollment();
                        break;

                    case 13:
                        trainingManager.ShowSessions(courses);
                        break;

                    case 14:
                        trainingService.DisplayCourseSummaries(courses);
                        break;

                    case 15:
                        paymentService.ProcessPayment(enrollmentService);
                        break;

                    case 16:
                        paymentService.DisplayHistory();
                        break;

                    case 17:
                        reportService.DisplayReports(
                            courses,
                            enrollmentService,
                            paymentService);
                        break;

                    case 18:
                        Console.WriteLine(
                            "\n===== GENERIC METHOD DEMONSTRATION =====");

                        Console.WriteLine("\nAvailable Courses:");

                        GenericUtilities.DisplayItems(courses);

                        string courseCode =
     TrainingManagementSystem.Utilities.InputValidator.ReadRequiredText(
         "\nEnter course code to search: ");

                        Course? foundCourse =
                            GenericUtilities.FindItem(
                                courses,
                                course =>
                                    course.CourseCode.Equals(
                                        courseCode,
                                        StringComparison.OrdinalIgnoreCase));

                        if (foundCourse != null)
                        {
                            Console.WriteLine(
                                $"Course Found: {foundCourse.CourseCode} - " +
                                $"{foundCourse.Title}");
                        }
                        else
                        {
                            Console.WriteLine("Course not found.");
                        }

                        break;

                    case 19:
                        Console.WriteLine(
                            "Thank you for using the Training Management System!");
                        break;

                    default:
                        Console.WriteLine(
                            "Invalid choice. Please enter a number from 1 to 19.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An unexpected error occurred: {ex.Message}");
            }

        } while (choice != 19);
    }
}
