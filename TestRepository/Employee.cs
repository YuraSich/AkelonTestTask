namespace TestRepository;

public class Employee
{
    public string Name { get; set; }
    public List<Vacation> Vacations { get; set; }
    public int MaxVacationDuration { get; set; }

    public int TotalVacationDuration => Vacations.Select(x => x.Duration).Sum();

    public bool IsVacationPossible(DateTime date, int duration)
    {
        if (date.AddDays(duration).Year != date.Year)
        {
            return false;
        }
        foreach (var vacation in Vacations)
        {
            if (date >= vacation.Date && !LessThenMonthBetween(date, vacation.Date.AddDays(vacation.Duration)))
            {
                return false;
            }
            else if (!LessThenMonthBetween(vacation.Date, date.AddDays(duration)))
            {
                return false;
            }
        }
        return true;
    }

    public Employee(string name, int maxVacationDuration = 28)
    {
        Name = name;
        Vacations = new();
        MaxVacationDuration = maxVacationDuration;
    }

    private static bool LessThenMonthBetween(DateTime lowerDate, DateTime higherDate)
    {
        if (lowerDate.AddDays(32) < higherDate)
        {
            return true;
        }
        else if (lowerDate.AddMonths(1).Day < higherDate.Day)
        {
            return true;
        }
        return false;
    }

}
