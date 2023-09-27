namespace TestRepository;

public class Department
{
    private readonly List<DateTime> _availableDays = new();
    private readonly int _year;


    public required List<Employee> Employees;

    public Department(int? year = null)
    {
        _year = year ?? DateTime.Now.Year;
        DateTime dt = new(_year, 1, 1);
        while (dt.Year == _year)
        {
            if (IsWorkingDay(dt))
            {
                _availableDays.Add(dt);
            }
            dt = dt.AddDays(1);
        }
    }

    internal void DistributeVacations(int maxVacationDuration)
    {
        foreach (var employee in Employees)
        {
            while (employee.TotalVacationDuration < maxVacationDuration)
            {
                ReserveVacationFor(employee);
            }
        }
    }
    internal void PrintVacationByPerson()
    {
        foreach (var employee in Employees)
        {
            Console.WriteLine("Отпуска " + employee.Name + " : ");
            foreach (var v in employee.Vacations)
            {
                Console.WriteLine($" с {v.Date:d} по {v.Date.AddDays(v.Duration):d}");
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

    private DateTime GetRandomAvailableDay(int duration)
    {
        int range = _availableDays.Count - duration;
        return _availableDays[Random.Shared.Next(range)];
    }
    private DateTime GetNextAvailableDay(DateTime date, int duration)
    {
        if (date.AddDays(duration) >= _availableDays.Max(x => x.Date))
        {
            return _availableDays.First();
        }
        var d = _availableDays.First(x => x.Date > date);
        if (d.Year > date.Year)
        {
            d.AddYears(-1);
        }
        return d;
    }

    private void ReserveDays(DateTime start, int duration)
    {
        _availableDays.RemoveAll(x => x.Date >= start && x.Date <= start.AddDays(duration+1));
    }

    private void ReserveVacationFor(Employee employee, DateTime start, int duration)
    {
        var _veryStart = start;
        while (!employee.IsVacationPossible(start, duration))
        {
            start = GetNextAvailableDay(start, duration);
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
        ReserveDays(start, duration);
    }

    private void ReserveVacationFor(Employee employee)
    {
        var duration = Random.Shared.Next(1, 3) * 7;
        var day = GetRandomAvailableDay(duration);
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

}