using TrainingManagementSystem.Interfaces;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services;

public class TrainingManager
{
    private readonly ITrainingSessionService _sessionService;

    public TrainingManager(
        ITrainingSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public void ShowSessions(IReadOnlyList<Course> courses)
    {
        _sessionService.DisplaySessions(courses);
    }
}