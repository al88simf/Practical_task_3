using System;

namespace PracticalTask3
{
    /// <summary>
    /// Интерфейс обеспечивает ряд констант, методов и свойств, необходимых 
    /// для рассчета выходных данных.
    /// </summary>
    interface IFlyable
    {
        const int CoeffHour = 3600;         // коэффициент секунд в 1 часе
        const int CoeffMin = 60;            // коэффициент минут в 1 часе
        const double CoeffMtrPerMin = 16.667;    // коэффициент метры в минуту
        const int SemiRound = 180;          // полукруг
        const int Round = 360;              // круг
        const int CoeffMtrs = 1000;         // коэффициент метров в 1 километре
        static double destin;               // расстояние в метрах
        static double newDest;              // новое расстояние в метрах
        
        /// <summary>
        /// Свойство Name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Свойство Speed.
        /// </summary>
        int Speed { get; set; }

        /// <summary>
        /// Свойство Kilometres.
        /// </summary>
        int Kilometres { get; set; }

        /// <summary>
        /// Свойство Metres.
        /// </summary>
        int Metres { get; set; }

        /// <summary>
        /// Свойство Hours.
        /// </summary>
        int Hours { get; set; }

        /// <summary>
        /// Свойство Minutes.
        /// </summary>
        int Minutes { get; set; }

        /// <summary>
        /// Свойство Seconds.
        /// </summary>
        int Seconds { get; set; }

        /// <summary>
        /// Свойство Limit.
        /// </summary>
        int Limit { get; }

        /// <summary>
        /// Свойство Angle.
        /// </summary>
        int Angle { get; set; }

        /// <summary>
        /// Свойство Message.
        /// </summary>
        string Message { get; set; }

        
        /// <summary>
        /// Выполняет расчет скорости.
        /// </summary>
        /// <returns>Возвращает значение скорости в км/ч.</returns>
        int GetSpeed();

        
        /// <summary>
        /// Выполняет расчет скорости для расчета текущих координат.
        /// </summary>
        /// <param name="hour">Часы.</param>
        /// <param name="minute">Минуты.</param>
        /// <returns>Возвращает новое значение скорости в км/ч.</returns>
        int GetNewSpeed(double hour, double minute);
        
        
        /// <summary>
        /// Выводит сообщение о соответствии введенного значения ограничению 
        /// после соответствующих расчетов.
        /// </summary>
        /// <returns>Возвращает сообщение для определенного объекта.</returns>
        string GetMessage();
        
        
        /// <summary>
        /// Выполняет расчет пройденного расстояния от начальной к новой точке.
        /// </summary>
        /// <remarks>
        /// Расчет выполняется с помощью теоремы Пифагора. От точек проводятся 
        /// перпендикуляры к осям координат, которые считаются катетами 
        /// прямоугольного треугольника. Зная квадраты катетов можно найти
        /// гипотенузу, которая и является расстоянием между точками.
        /// </remarks>
        /// <param name="startX">Начальная координата X.</param>
        /// <param name="startY">Начальная координата Y.</param>
        /// <param name="startZ">Начальная координата Z/</param>
        /// <param name="newX">Новая координата X.</param>
        /// <param name="newY">Новая координата Y.</param>
        /// <param name="newZ">Новая координата Z.</param>
        /// <param name="metres">Выходной параметр метры.</param>
        /// <param name="kilometres">Выходной параметр километры.</param>
        void FlyTo(double startX, double startY, double startZ, 
            double newX, double newY, double newZ, 
            out int metres, out int kilometres)
        {
            double cathetusA = GetSquare(newX, startX);
            double cathetusB = GetSquare(newY, startY);
            double cathetusC = GetSquare(newZ, startZ);
            double hypotenuse = Math.Sqrt(cathetusA + cathetusB + cathetusC);

            destin = Math.Round(hypotenuse, 2);     // расстояние

            kilometres = Convert.ToInt32(destin) / CoeffMtrs;
            metres = Convert.ToInt32(destin % CoeffMtrs);


            // Расчитывает квадрат катета.
            static double GetSquare(double coord1, double coord2)
            {
                return Math.Pow(coord1 - coord2, 2);
            }
        }
        
        
        /// <summary>
        /// Выполняет расчет нового пройденного расстояния до текущих координат.
        /// </summary>
        /// <param name="hour">Часы.</param>
        /// <param name="minute">Минуты.</param>
        /// <param name="velocity">Скорость.</param>
        /// <param name="metres">Выходной параметр метры.</param>
        /// <param name="kilometres">Выходной параметр километры.</param>
        void FlyToNew(double hour, double minute, 
            double velocity, 
            out int metres, out int kilometres);


        /// <summary>
        /// Выполняет расчет времени за пройденное расстояние.
        /// </summary>
        /// <param name="velocity">Скорость</param>
        /// <returns>Возвращает время в часах, минутах и секундах.</returns>
        (int hours, int minutes, int seconds) GetFlyTime(double velocity);


        /// <summary>
        /// Выполняет расчет угла полета объекта.
        /// </summary>
        /// <remarks>
        /// Расчет выполняется по формуле нахождения угла прямоугольного 
        /// треугольника по известным катету и гипотенузе.
        /// </remarks>
        /// <param name="startY">Начальная координата X.</param>
        /// <param name="newY">Новая координата Y.</param>
        /// <returns>Возвращает значение угла beta (угла полета).</returns>
        int GetAngle(double startY, double newY)
        {
            double angle = Math.Sin((newY - startY) / destin);  // угол beta
            angle *= SemiRound / Math.PI;                       // в градусах

            // Проверка на отрицательное значение.
            return Math.Sign(angle) == -1 ?
                Convert.ToInt32(angle += Round) :
                Convert.ToInt32(angle);
        }


        /// <summary>
        /// Выполняет расчет текущих координат.
        /// </summary>
        /// <remarks>
        /// Расчет выполняется следующим образом: сначала находит углы alpha и 
        /// beta прямоугольного треугольника по гипотенузе и противолежащим 
        /// катетам, далее, находит новые катеты по новой гипотенузе и углам 
        /// (которые остаются одинаковыми на всем протяжении полета). Найденные
        /// катеты - это текущие координаты объекта.
        /// </remarks>
        /// <param name="startX">Начальная координата X.</param>
        /// <param name="newX">Новая координата X.</param>
        /// <param name="startY">Начальная координата Y.</param>
        /// <param name="newY">Новая координата Y.</param>
        /// <param name="startZ">Начальная координата Z.</param>
        /// <param name="newZ">Навая координата Z.</param>
        /// <returns>Возвращает текущие координаты объекта в метрах.</returns>
        (int currentX, int currentY, int currentZ) 
            GetCurrPosit(double startX, double newX, 
            double startY, double newY, 
            double startZ, double newZ)
        {
            double alpha = GetAngle(startX, newX);
            double beta = GetAngle(startY, newY);

            double coorX = GetCoord(beta);
            double coorY = GetCoord(alpha);
            double coorZ = coorX;           // катеты по осям X и Z совпадают

            // Проверка на необходимость добавления отрезков координат.
            if (newDest <= destin)
            {
                // Если новое расстояние меньше основного - добавляет отрезки
                // координат до начальной точки.
                coorX += startX;
                coorY += startY;
                coorZ += startZ;
            }
            else
            {
                coorX += startX + newX;
                coorY += startY + newY;
                coorZ += startZ + newZ;
            }
            return (Convert.ToInt32(coorX), 
                Convert.ToInt32(coorY), 
                Convert.ToInt32(coorZ));

            
            // Расчитывает угол.
            double GetAngle(double coord1, double coord2)
            {
                double angle = Math.Sin((coord2 - coord1) / destin);
                return Math.Round(angle, 2);
            }

            // Расчитывает текущие координаты.
            double GetCoord(double angle)
            {
                double coordinate = Math.Cos(angle) * newDest;
                return Math.Round(coordinate, 2);
            }
        }
        
        
        /// <summary>
        /// Добавляет грамматически правильные окончания.
        /// </summary>
        /// <param name="part">Часть текста.</param>
        /// <returns>Возвращает грамматичеки правильные окончания.</returns>
        string GetClause(int part)
        {
            int suffix = GetSuffix(Angle);
            
            switch (part)
            {
                case 1:
                    if (Kilometres >= 1) return $"{Kilometres}км {Metres}м";
                    else return $"{Metres}м";
                case 2:
                    if (Hours >= 1) return $"{Hours}ч.{Minutes}мин.,";
                    else return $"{Minutes}мин.{Seconds}с.,";
                case 3:
                    if (Hours >= 1) return $"{Hours}ч.{Minutes}мин.,";
                    else return $"{Minutes}мин.,";
                default:
                    if (suffix == 0 || suffix >= 5) return $"{Angle} градусов.";
                    else if (suffix == 1) return $"{Angle} градус.";
                    else return $"{Angle} градуса.";
            }

            // Сверяет значение свойства Angle для выбора правильного окончания.
            int GetSuffix(int ang) => ang % 10;
        }
    }
}
