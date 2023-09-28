namespace TestRepository;

public partial class Department
{
    public class WorkingDayPool
    {
        private readonly int _year;
        private readonly List<DateTime> _availableDays = new();
        public WorkingDayPool(int? year = null)
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

        internal DateTime GetRandomAvailableDay(int duration)
        {
            int range = _availableDays.Count - duration;
            return _availableDays[Random.Shared.Next(range)];
        }


        internal DateTime GetNextAvailableDay(DateTime date, int duration)
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
        internal void ReserveDays(DateTime start, int duration)
        {
            _availableDays.RemoveAll(x => x.Date >= start && x.Date <= start.AddDays(duration + 1));
        }
    }
}