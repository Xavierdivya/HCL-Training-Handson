using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Interfaces;

public interface ITrainingSessionService
{
    void DisplaySessions(IReadOnlyList<Course> courses);
}