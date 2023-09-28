namespace TestRepository;

public partial class Department
{
    public required List<Employee> Employees;

    private readonly WorkingDayPool _dayPool;
    private readonly int[] _vacationPosibleDurations;

    public Department()
    {
        _dayPool = new WorkingDayPool(year: DateTime.Now.Year);
        _vacationPosibleDurations =  new int[] { 7, 14 };
    }

    internal void DistributeVacations()
    {
        foreach (var employee in Employees)
        {
            while (employee.TotalVacationDuration < employee.MaxVacationDuration)
            {
                ReserveVacationFor(employee);
            }
        }
    }
    internal void PrintVacationByPerson()
    {
        foreach (var employee in Employees)
        {
            Console.WriteLine($"Отпуска {employee.Name} ({employee.TotalVacationDuration} сут.): ");
            foreach (var v in employee.Vacations)
            {
                Console.WriteLine($" с {v.Date:d} по {v.Date.AddDays(v.Duration):d} ({v.Duration} сут.)");
            }
        }
    }
    internal void PrintVacationByDate()
    {
        var vacations = new List<Vacation>();
        foreach (var employee in Employees)
        {
            vacations.AddRange(employee.Vacations);
        }
        foreach (var v in vacations.OrderBy(x => x.Date))
        {
            Console.WriteLine($" с {v.Date:d} по {v.Date.AddDays(v.Duration):d}");
        }

    }

    private void ReserveVacationFor(Employee employee, DateTime start, int duration)
    {
        var _veryStart = start;
        while (!employee.IsVacationPossible(start, duration))
        {
            start = _dayPool.GetNextAvailableDay(start, duration);
            if (start == _veryStart)
            {
                throw new Exception("Невозможно уместить отпуск");
            }
        }
        employee.Vacations.Add(new Vacation
        {
            Date = start,
            Duration = duration
        });
        _dayPool.ReserveDays(start, duration);
    }

    private void ReserveVacationFor(Employee employee)
    {
        var duration = GetRandomPosibleVacationDuration(employee);
        var day = _dayPool.GetRandomAvailableDay(duration);
        ReserveVacationFor(employee, day, duration);
    }

    private static bool IsWorkingDay(DateTime date)
    {
        return date.DayOfWeek switch
        {
            DayOfWeek.Saturday => false,
            DayOfWeek.Sunday => false,
            _ => true
        };
    }
    private int GetRandomPosibleVacationDuration(Employee employee)
    {
        int daysLeft = employee.MaxVacationDuration - employee.TotalVacationDuration;
        var pool = _vacationPosibleDurations.Where(x => x < daysLeft).ToList();
        if (!pool.Any())
        {
            return daysLeft;
        }
        int range = pool.Count;
        return pool[Random.Shared.Next(range)];
    }
}