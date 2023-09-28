using TestRepository;

class Program
{
    public static void Main()
    {
        Department department = new()
        {
            Employees = new() {
            new("Иванов Иван Иванович"),
            new("Петров Петр Петрович"),
            new("Юлина Юлия Юлиановна"),
            new("Сидоров Сидор Сидорович"),
            new("Павлов Павел Павлович"),
            new("Георгиев Георг Георгиевич", maxVacationDuration: 35)
            }
        };

        department.DistributeVacations();
        department.PrintVacationByPerson();
        Console.WriteLine("Отпуска по времени");
        department.PrintVacationByDate();
        Console.ReadKey();
    }
}
