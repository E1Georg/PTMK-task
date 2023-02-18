using ConsoleAppTask;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.Text;

var param = new List<string>();
Environment.GetCommandLineArgs().ToList().ForEach(arg => param.Add(arg));

//TASK 1
void createTable()
{
    try
    {
        using (MyDbContext db = new MyDbContext(true))
        {
            var temp = db.Persons.ToList();
        }
        Console.WriteLine("Таблица была создана!");
    }
    catch (Exception exc)
    {
        Console.WriteLine("Ошибка при создании таблицы: " + exc.Message);        
    }

}

//TASK 2 
void uploadData()
{
    try
    {
        if(param.Count != 7)
        {
            Console.WriteLine("Для добавления данных в табдицу недостаточно аргументов!");
            return;
        }

        using (MyDbContext db = new MyDbContext())
        {
            DateOnly tmpDate = DateOnly.ParseExact(param.ElementAt(5), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Person tmp = new Person { firstName = param.ElementAt(3), lastName = param.ElementAt(2), patronymic = param.ElementAt(4), wasBorn = tmpDate, gender = param.ElementAt(6) };
            db.Persons.AddRange(tmp);
            db.SaveChanges();
        }
        Console.WriteLine("Запись была успешно добавлена!");
    }
    catch (Exception exc)
    {
        Console.WriteLine("Ошибка при добавлении данных в таблицу: " + exc.Message);
    }
   
}

//TASK 3
void getDataWithFilter()
{
    try
    {
        using (MyDbContext db = new MyDbContext())
        {

            List<Person> persons = db.Persons
                .GroupBy(m => new { m.lastName, m.firstName, m.patronymic, m.wasBorn })
                .Select(group => group.First())
                .ToList();

            Console.WriteLine("======= Лист данных =======");          
            foreach (var item in persons)
            {                
                Console.WriteLine("Фамилия: " + item.lastName);
                Console.WriteLine("Имя: " + item.firstName);
                Console.WriteLine("Отчество: " + item.patronymic);
                Console.WriteLine("Дата рождения: " + item.wasBorn);
                Console.WriteLine("Пол: " + item.gender);                
                Console.WriteLine("Возраст: " + (DateTime.Now.Year - item.wasBorn.Year));          
                Console.WriteLine("================================");
            }
        }
    }
    catch (Exception exc)
    {
        Console.WriteLine("Ошибка при получении данных из БД: " + exc.Message);
    }    
}

//TASK 4 
string GenerateName(int len)
{
    Random r = new Random();
    string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
    string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
    string Name = "";

    if(r.Next(0, 2) == 1)
    {
        Name += consonants[r.Next(consonants.Length)].ToUpper();
        Name += vowels[r.Next(vowels.Length)];
    }
    else 
    {
        Name += vowels[r.Next(vowels.Length)].ToUpper();
        Name += consonants[r.Next(consonants.Length)];        
    }

    int step = 2;
    while (step < len)
    {
        Name += consonants[r.Next(consonants.Length)];
        step++;
        Name += vowels[r.Next(vowels.Length)];
        step++;
    }
    return Name;
}

void uploadBigData()
{
    try
    {
        using (MyDbContext db = new MyDbContext())
        {
            Person tmpPerson;
            Random randomForName = new Random();
            Random randomForGender = new Random();
            Random randomForDate = new Random();

            // 1 million random persons
            for (int i = 0; i < 1000000; i++)
            {
                tmpPerson = new Person {
                    firstName = GenerateName(randomForName.Next(3, 8)),
                    lastName = GenerateName(randomForName.Next(3, 8)),                    
                    patronymic = GenerateName(randomForName.Next(3, 8)), 
                    wasBorn = new DateOnly(
                        randomForDate.Next(1924, 2024),
                        randomForDate.Next(1, 13),
                        randomForDate.Next(1, 29)
                        ), 
                    gender = randomForGender.Next(2) == 0 ? "male" : "female"
                };

                db.Persons.AddRange(tmpPerson);
            }

            // 100 persons with lastname[0] = "F" and gender = "male"           
            for (int i = 0; i < 100; i++)
            {
                tmpPerson = new Person
                {
                    firstName = GenerateName(randomForName.Next(3, 8)),
                    lastName = "F" + GenerateName(randomForName.Next(3, 8)).Substring(1),
                    patronymic = GenerateName(randomForName.Next(3, 8)),
                    wasBorn = new DateOnly(
                        randomForDate.Next(1924, 2024),
                        randomForDate.Next(1, 13),
                        randomForDate.Next(1, 29)
                        ),
                    gender = "male"
                };

                db.Persons.AddRange(tmpPerson);
            }

            db.SaveChanges();
            Console.WriteLine("Данные были успешно добавлены!");
        }
    }
    catch (Exception exc)
    {
        Console.WriteLine("Ошибка: " + exc.Message);
    }
    
}

//TASK 5
void checkTimelineTask5()
{
    try
    {
        Stopwatch stopwatch = new Stopwatch();
        using (MyDbContext db = new MyDbContext())
        {
            stopwatch.Start();
            IQueryable<Person> data = db.Persons;
            var persons = data.Where(x => x.gender == "male").Where(y => y.lastName.StartsWith("F")).ToList();
            stopwatch.Stop();

            Console.WriteLine("======= Лист данных =======");
            foreach (var item in persons)
            {
                Console.WriteLine("Фамилия: " + item.lastName);
                Console.WriteLine("Имя: " + item.firstName);
                Console.WriteLine("Отчество: " + item.patronymic);
                Console.WriteLine("Дата рождения: " + item.wasBorn);
                Console.WriteLine("Пол: " + item.gender);                
                Console.WriteLine("================================");
            }

            Console.WriteLine($"Данные были успешно получены, время выполнения запроса(Task 5) составило: {stopwatch.ElapsedMilliseconds} ms");            
        }
    }
    catch (Exception exc)
    {
        Console.WriteLine("Ошибка: " + exc.Message);      
    }    
}

//TASK 6 
void checkTimelineTask6()
{
    try
    {
        Stopwatch stopwatch = new Stopwatch();
        using (MyDbContext db = new MyDbContext())
        {
            stopwatch.Start();           
            var persons = db.Persons.AsEnumerable().Select(x => new { x.firstName, x.lastName, x.patronymic, x.wasBorn, x.gender }).Where(x => x.gender == "male").Where(y => y.lastName.StartsWith("F")).ToList();
            stopwatch.Stop();
            Console.WriteLine($"Данные были успешно получены, время выполнения запроса(Task 6) составило: {stopwatch.ElapsedMilliseconds} ms");
        }        
    }
    catch (Exception exc)
    {
        Console.WriteLine("Ошибка: " + exc.Message);
    }    
}


switch (param.ElementAt(1))
{
    case "1":
        createTable();
        break;

    case "2":
        uploadData();
        break;

    case "3":
        getDataWithFilter();
        break;

    case "4":
        uploadBigData();
        break;

    case "5":
        checkTimelineTask5();
        break;

    case "6":
        checkTimelineTask6();
        break;

    default:  
        Console.WriteLine("Аргумент не соответствует возможному ключу");
        Console.WriteLine("Ключ \"1\" - Task 1, создание таблицы.");
        Console.WriteLine("Ключ \"2\" - Task 2, добавление в таблицу строки данных. Дата рождения додлжна быть в формате dd-MM-yyyy.");
        Console.WriteLine("Ключ \"3\" - Task 3, вывод отфильтрованных строк таблицы.");
        Console.WriteLine("Ключ \"4\" - Task 4, наполнение таблицы сгенерированными данными.");
        Console.WriteLine("Ключ \"5\" - Task 5, замер времени выполнения запроса к таблице.");
        Console.WriteLine("Ключ \"6\" - Task 6, замер времени выполнения оптимизированного запроса к таблице.");
        break;
}