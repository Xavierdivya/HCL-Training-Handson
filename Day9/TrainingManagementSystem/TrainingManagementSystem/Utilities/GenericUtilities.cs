namespace TrainingManagementSystem.Utilities;

public static class GenericUtilities
{
    public static void DisplayItems<T>(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            Console.WriteLine(item);
        }
    }

    public static T? FindItem<T>(
        IEnumerable<T> items,
        Predicate<T> match)
    {
        foreach (T item in items)
        {
            if (match(item))
            {
                return item;
            }
        }

        return default;
    }
}