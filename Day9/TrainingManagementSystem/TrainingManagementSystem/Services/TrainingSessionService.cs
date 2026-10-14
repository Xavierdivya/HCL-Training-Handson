
using System;
using System.Collections.Generic;
using TrainingManagementSystem.Interfaces;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Sessions;

namespace TrainingManagementSystem.Services;

public class TrainingSessionService : ITrainingSessionService
{
    public void DisplaySessions(IReadOnlyList<Course> courses)
    {
        Console.WriteLine("\n========== TRAINING SESSIONS ==========");

        if (courses.Count == 0)
        {
            Console.WriteLine("No courses are available.");
            return;
        }

        foreach (Course course in courses)
        {
            SessionDuration duration =
                new SessionDuration(course.DurationDays);

            CourseSession session;

            if (course.IsOnline)
            {
                session = new TrainingManagementSystem.Sessions.OnlineSession(
                    course.CourseCode,
                    course.StartDate,
                    duration,
                    "Microsoft Teams");
            }
            else
            {
                session = new TrainingManagementSystem.Sessions.ClassroomSession(
                    course.CourseCode,
                    course.StartDate,
                    duration,
                    "Training Center");
            }

            Console.WriteLine(session.GetDetails());
            Console.WriteLine(
                $"Session Charge: {session.CalculateSessionCharge():C}");
            Console.WriteLine("---------------------------------------");
        }
    }
}
