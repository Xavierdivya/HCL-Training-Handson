using TrainingManagementSystem.Models;
using TrainingManagementSystem.Utilities;

namespace TrainingManagementSystem.Services;

public class LearnerService
{
    private readonly Dictionary<string, Learner> _learners =
        new(StringComparer.OrdinalIgnoreCase);

    public bool LearnerExists(string id)
    {
        return _learners.ContainsKey(id);
    }

    public void RegisterLearner()
    {
        Console.WriteLine("\n===== REGISTER LEARNER =====");

        string id = InputValidator.ReadRequiredText("Learner ID: ");

        if (_learners.ContainsKey(id))
        {
            Console.WriteLine("That learner ID already exists.");
            return;
        }

        string name = InputValidator.ReadRequiredText("Name: ");

        string contact = InputValidator.ReadRequiredText(
            "Contact number (10-15 digits): ");

        if (!InputValidator.IsValidContactNumber(contact))
        {
            Console.WriteLine("Invalid contact number.");
            return;
        }

        string email = InputValidator.ReadRequiredText("Email: ");

        if (!InputValidator.IsValidEmail(email))
        {
            Console.WriteLine("Invalid email address.");
            return;
        }

        _learners.Add(
            id,
            new Learner(id, name, contact, email));

        Console.WriteLine("Learner registered successfully.");
    }

    public void DisplayLearners()
    {
        Console.WriteLine("\n===== LEARNERS =====");

        if (_learners.Count == 0)
        {
            Console.WriteLine("No learners registered.");
            return;
        }

        foreach (Learner learner in _learners.Values)
        {
            Console.WriteLine(
                $"{learner.LearnerId} | {learner.Name} | " +
                $"{learner.ContactNumber} | {learner.Email}");
        }
    }

    public void SearchLearner()
    {
        string id = InputValidator.ReadRequiredText("Learner ID: ");

        if (_learners.TryGetValue(id, out Learner? learner))
        {
            Console.WriteLine(
                $"{learner.LearnerId} | {learner.Name} | " +
                $"{learner.ContactNumber} | {learner.Email}");
        }
        else
        {
            Console.WriteLine("Learner not found.");
        }
    }
}