using System;

namespace PracticalTask3
{

    /// <summary>
    /// Класс Program обеспечивает вывод данных летающих объектов.
    /// </summary>
    /// <remarks>
    /// Класс принимает объект, координаты его начальной точки полета и новой, 
    /// производит расчеты и выводит на консоль следующие данные: введенные координаты,
    /// дальность, скорость, время и угол полета объекта. Для определения текущих 
    /// координат принимает время полета от начальной точки, производит повторно
    /// расчеты и выводит новые данные полета и текущие координаты.
    /// </remarks>
    class Program
    {
        
        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        /// <param name="args">Список аргументов для командной строки.</param>
        static void Main(string[] args)
        {
            IFlyable flyer = GetFlyer();

            var axle = GetCoord(flyer.Name, flyer);
            Coordinate coord = new(axle.sX, axle.sY, axle.sZ, 
                axle.nX, axle.nY, axle.nZ);

            Console.WriteLine($"Согласно введенным начальным координатам: " +
                $"{coord.StartX}, {coord.StartY}, {coord.StartZ} и новым" +
                $" координатам {coord.NewX}, {coord.NewY}, {coord.NewZ}, " +
                $"{flyer.Name} пролетит {flyer.GetClause(1)} со скоростью " +
                $"{flyer.Speed}км/ч в течение времени {flyer.GetClause(2)} " +
                $"угол полета {flyer.GetClause(4)}\n");

            var newCrd = GetNewCoord(flyer, coord);
            (coord.NewX, coord.NewY, coord.NewZ) = 
                (newCrd.curX, newCrd.curY, newCrd.curZ);

            Console.Write($"Согласно введенного времени {flyer.GetClause(3)} " +
                $"{flyer.Name} пролетит {flyer.GetClause(1)} со скоростью " +
                $"{flyer.Speed}км/ч, угол полета {flyer.GetClause(4)} ");
            Console.WriteLine($"Текущие координаты: {coord.NewX}, {coord.NewY}, " +
                $"{coord.NewZ}.");

            Console.ReadKey();
        }
        
        /// <summary>
        /// Принимает выбор объекта.
        /// </summary>
        /// <returns>
        /// Возвращает ссылку на выбранный объект и передает некоторые начальные 
        /// значения.
        /// </returns>
        static IFlyable GetFlyer()
        {
            // Цикл проверяет введенное значение, если оно не корректно -
            // повторяется.
            while (true)
            {
                Console.Write("Выберите объект: " +
                "\n\tПтица - нажмите \"1\"" +
                "\n\tСамолет - нажмите \"2\"" +
                "\n\tДрон - нажмите \"3\"." +
                "\n\tВы выбрали: ", "\n");
                string? inputVal = Console.ReadLine();  // введенный объект

                if (int.TryParse(inputVal, out int choice) && 
                    (choice == 1 || choice == 2 || choice == 3))
                {
                    if (choice == 1) return new Bird("Птица", 168);
                    else if (choice == 2) return new Plane("Самолет", 18);
                    else return new Drone("Дрон", 1000);
                }
                else PrintWrong(1);
            }
        }

        /// <summary>
        /// Принимает начальные и новые координаты полета и, в соответствии с ними,
        /// расчитывает и назначает значения свойств.
        /// </summary>
        /// <param name="name">Имя объекта</param>
        /// <param name="fly">Объект</param>
        /// <returns>
        /// Возвращает проверенные координаты обоих точек.
        /// </returns>
        static (int sX, int sY, int sZ, 
            int nX, int nY, int nZ) 
            GetCoord(string name,
                          IFlyable fly)
        {
            // Цикл проверяет введенные координаты и в случае их корректности
            // назначает свойства. Если они не верны - цикл повторяется.
            while (true)
            {
                Console.WriteLine($"{name}: введите координаты начальной " +
                    $"точки полета (в метрах).");
                GetPoint(out int startX, 
                    out int startY, 
                    out int startZ);

                Console.WriteLine("Введите координаты новой точки полета" +
                    " (в метрах).");
                GetPoint(out int newX, 
                    out int newY, 
                    out int newZ);

                // Проверка на совпадение координат.
                if ((startX, startY, startZ) != 
                    (newX, newY, newZ))
                {
                    if (fly is Bird bird)
                    {
                        fly.FlyTo(startX, startY, startZ, 
                            newX, newY, newZ, 
                            out int mtrs, out int kmtrs);
                        bird.Metres = mtrs; 
                        bird.Kilometres = kmtrs;

                        bird.Speed = bird.GetSpeed();

                        var time = bird.GetFlyTime(bird.Speed);
                        (bird.Hours, bird.Minutes, bird.Seconds) = 
                            (time.hours, time.minutes, time.seconds);

                        bird.Angle = fly.GetAngle(startY, newY);

                        // Проверка на соответствие ограничению.
                        if (bird.Hours <= bird.Limit)
                        {
                            return (startX, startY, startZ, 
                                newX, newY, newZ);
                        }
                        else
                        {
                            bird.Message = bird.GetMessage();
                            Console.WriteLine(bird.Message);
                            PrintWrong(1);
                        }
                    }
                    if (fly is Plane plane)
                    {
                        fly.FlyTo(startX, startY, startZ, 
                            newX, newY, newZ, 
                            out int mtrs, out int kmtrs);
                        plane.Metres = mtrs; 
                        plane.Kilometres = kmtrs;

                        plane.Speed = plane.GetSpeed();

                        var time = plane.GetFlyTime(plane.Speed);
                        (plane.Hours, plane.Minutes, plane.Seconds) = 
                            (time.hours, time.minutes, time.seconds);
                        
                        plane.Angle = fly.GetAngle(startX, newY);

                        // Проверка на соответствие ограничению.
                        if (plane.Hours <= plane.Limit)
                        {
                            return (startX, startY, startZ, newX, newY, newZ);
                        }
                        else
                        {
                            plane.Message = plane.GetMessage();
                            Console.WriteLine(plane.Message);
                            PrintWrong(1);
                        }
                    }
                    if (fly is Drone drone)
                    {
                        fly.FlyTo(startX, startY, startZ, 
                            newX, newY, newZ, 
                            out int mtrs, out int kmtrs);
                        drone.Metres = mtrs; 
                        drone.Kilometres = kmtrs;

                        drone.Speed = drone.GetSpeed();

                        var time = drone.GetFlyTime(drone.Speed);
                        (drone.Hours, drone.Minutes, drone.Seconds) = 
                            (time.hours, time.minutes, time.seconds);

                        drone.Angle = fly.GetAngle(startX, newY);

                        // Проверка на соответствие ограничению.
                        if (drone.Kilometres <= drone.Limit)
                        {
                            return (startX, startY, startZ, newX, newY, newZ);
                        }
                        else
                        {
                            drone.Message = drone.GetMessage();
                            Console.WriteLine(drone.Message);
                            PrintWrong(1);
                        }
                    }
                }
                else PrintWrong(3);
            }

            // Получает координаты, введенные с консоли, и возвращает их.
            static void GetPoint(out int x, out int y, out int z)
            {
                while (true)
                {
                    Console.Write("По оси X: ");
                    string? inputValX = Console.ReadLine(); // введенная координата

                    if (int.TryParse(inputValX, out int axleX) && 
                        (Math.Sign(axleX) != -1))
                    {
                        x = axleX;                          // выходная координата
                        Console.Write("По оси Y: ");
                        string? inputValY = Console.ReadLine();
                        if (int.TryParse(inputValY, out int axleY) && 
                            (Math.Sign(axleY) != -1))
                        {
                            y = axleY;
                            Console.Write("По оси Z: ");
                            string? inputValZ = Console.ReadLine();
                            if (int.TryParse(inputValZ, out int axleZ) && 
                                (Math.Sign(axleZ) != -1))
                            {
                                z = axleZ;
                                break;                      // выход из цикла
                            }
                            else
                            {
                                // Проверка на отрицательное значение.
                                if (Math.Sign(axleZ) == -1) PrintWrong(2);
                                PrintWrong(1);
                            }
                        }
                        else
                        {
                            if (Math.Sign(axleY) == -1) PrintWrong(2);
                            PrintWrong(1);
                        }
                    }
                    else
                    {
                        if (Math.Sign(axleX) == -1) PrintWrong(2);
                        PrintWrong(1);
                    }
                }
            }
        }
        
        /// <summary>
        /// Принимает значение текущего времени полета, в соответствии с которым 
        /// расчитывает и назначает значения свойств.
        /// </summary>
        /// <param name="fly">Объект</param>
        /// <param name="coord">Координаты для проверки.</param>
        /// <returns>
        /// Возвращает проверенные текущие координаты новой точки.
        /// </returns>
        static (int curX, int curY, int curZ) 
            GetNewCoord(IFlyable fly,
                        Coordinate coord)
        {
            // Цикл проверяет введенные координаты и в случае их корректности
            // назначает свойства. Если они не верны - цикл повторяется.
            while (true)
            {
                if (fly is Bird bird)
                {
                    GetNewTime(bird, 
                        out int hrs, out int min);
                    bird.Hours = hrs; 
                    bird.Minutes = min;

                    bird.FlyToNew(bird.Hours, bird.Minutes, bird.Speed, 
                        out int mtrs, out int kmtrs);
                    bird.Metres = mtrs; 
                    bird.Kilometres = kmtrs;

                    var axle = fly.GetCurrPosit(coord.StartX, coord.NewX, coord.StartY, 
                        coord.NewY, coord.StartZ, coord.NewZ);

                    // Проверка на соответствие ограничению.
                    if (bird.Hours <= bird.Limit)
                    {
                        // Проверка на совпадение новых координат с начальными.
                        if ((axle.currentX != coord.StartX) && 
                            (axle.currentY != coord.StartY) && 
                            (axle.currentZ != coord.StartZ))
                        {
                            return (axle.currentX, axle.currentY, axle.currentZ);
                        }
                        else PrintWrong(3);
                    }
                    else
                    {
                        bird.Message = bird.GetMessage();
                        Console.WriteLine(bird.Message);
                        PrintWrong(1);
                    }
                }
                if (fly is Plane plane)
                {
                    GetNewTime(plane, 
                        out int hrs, out int min);
                    plane.Hours = hrs; 
                    plane.Minutes = min;

                    plane.Speed = plane.GetNewSpeed(plane.Hours, plane.Minutes);

                    plane.FlyToNew(plane.Hours, plane.Minutes, plane.Speed, 
                        out int mtrs, out int kmtrs);
                    plane.Metres = mtrs; 
                    plane.Kilometres = kmtrs;

                    var axle = fly.GetCurrPosit(coord.StartX, coord.NewX, coord.StartY, 
                        coord.NewY, coord.StartZ, coord.NewZ);

                    // Проверка на соответствие ограничению.
                    if (plane.Hours <= plane.Limit)
                    {
                        if ((axle.currentX != coord.StartX) &&
                            (axle.currentY != coord.StartY) &&
                            (axle.currentZ != coord.StartZ))
                        {
                            return (axle.currentX, axle.currentY, axle.currentZ);
                        }
                        else PrintWrong(3);
                    }
                    else
                    {
                        plane.Message = plane.GetMessage();
                        Console.WriteLine(plane.Message);
                        PrintWrong(1);
                    }
                }
                if (fly is Drone drone)
                {
                    GetNewTime(drone, 
                        out int hrs, out int min);
                    drone.Hours = hrs; 
                    drone.Minutes = min;

                    drone.FlyToNew(drone.Hours, drone.Minutes, drone.Speed, 
                        out int mtrs, out int kmtrs);
                    drone.Metres = mtrs; 
                    drone.Kilometres = kmtrs;

                    var axle = fly.GetCurrPosit(coord.StartX, coord.NewX, coord.StartY, 
                        coord.NewY, coord.StartZ, coord.NewZ);

                    // Проверка на соответствие ограничению.
                    if (drone.Kilometres <= drone.Limit)
                    {
                        if ((axle.currentX != coord.StartX) &&
                            (axle.currentY != coord.StartY) &&
                            (axle.currentZ != coord.StartZ))
                            return (axle.currentX, axle.currentY, axle.currentZ);
                        else PrintWrong(3);
                    }
                    else
                    {
                        drone.Message = drone.GetMessage();
                        Console.WriteLine(drone.Message);
                        PrintWrong(1);
                    }
                }
            }

            // Получает значение времени, введенное с консоли и возвращает его.
            static void GetNewTime(IFlyable fly, 
                out int hours, out int minutes)
            {
                while (true)
                {
                    Console.Write($"{fly.Name} летит от начальной точки к новой. " +
                        $"Чтобы узнать текущие координаты, введите значение " +
                        $"времени с момента начала полета, часы: ");
                    string? inputHrs = Console.ReadLine();  // введенные часы

                    if (int.TryParse(inputHrs, out int hrs))
                    {
                        Console.Write("минуты: ");          // введенные минуты
                        string inputMin = Console.ReadLine();
                        if (int.TryParse(inputMin, out int min))
                        {
                            if ((hrs != 0) || (min != 0))
                            {
                                hours = hrs;
                                minutes = min;
                                break;
                            }
                            else PrintWrong(4);
                        }
                        else PrintWrong(1);
                    }
                    else PrintWrong(1);
                }
            }
        }

        /// <summary>
        /// Выводит на консоль сообщение об ошибке.
        /// </summary>
        /// <param name="mesNum">Номер сообщения.</param>
        static void PrintWrong(int mesNum)
        {
            switch (mesNum)
            {
                case 1:
                    Console.WriteLine("Не верное значение. Введите значение корректно!\n");
                    break;
                case 2:
                    Console.WriteLine("Числа должны быть положительными.");
                    goto case 1;
                case 3:
                    Console.WriteLine("Координаты обоих точек не должны совпадать.");
                    goto case 1;
                default:
                    Console.WriteLine("Время не может равняться нулю.");
                    goto case 1;
            }
        }
    }
}
