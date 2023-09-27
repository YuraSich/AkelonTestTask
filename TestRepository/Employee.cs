namespace TestRepository;

public class Employee
{
    public string Name { get; set; }
    public List<Vacation> Vacations { get; set; }

    public int TotalVacationDuration => Vacations.Select(x => x.Duration).Sum();

    public bool IsVacationPossible(DateTime date, int duration)
    {
        if (date.AddDays(duration).Year != date.Year)
        {
            return false;
        }
        foreach (var vacation in Vacations)
        {
            if (date >= vacation.Date && (date - vacation.Date.AddDays(vacation.Duration)).TotalDays < 30)
            {
                return false;
            }
            else if (vacation.Date >= date && (vacation.Date - date.AddDays(duration)).TotalDays < 30)
            {
                return false;
            }
        }
        return true;



    }

    public Employee(string name)
    {
        Name = name;
        Vacations = new();
    }

}
