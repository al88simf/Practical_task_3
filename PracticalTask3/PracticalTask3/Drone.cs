using System;

namespace PracticalTask3
{
    /// <summary>
    /// Класс обеспечивае вычисление данных полета дрона.
    /// </summary>
    class Drone : IFlyable
    {
        private const int CoeffMin = IFlyable.CoeffMin;
        private const int MtrPerMin = 50;       // метров в минуту
        private double newDest;

        /// <summary>
        /// Свойство Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Свойство Kilometres.
        /// </summary>
        public int Kilometres { get; set; }

        /// <summary>
        /// Свойство Metres.
        /// </summary>
        public int Metres { get; set; }

        /// <summary>
        /// Свойство Speed.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Свойство Hours.
        /// </summary>
        public int Hours { get; set; }

        /// <summary>
        /// Свойство Minutes.
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// Свойство Seconds.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Свойство Limit.
        /// </summary>
        /// <remarks>
        /// Ограничение представляет невозможность дрона летать на дальность 
        /// большую чем 1000 км.
        /// </remarks>
        public int Limit { get; set; }

        /// <summary>
        /// Свойство Angle.
        /// </summary>
        public int Angle { get; set; }

        /// <summary>
        /// Свойство Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="name">Имя объкта.</param>
        /// <param name="limit">Ограничение.</param>
        public Drone(string name, int limit)
        {
            Name = name; 
            Limit = limit;
        }

        /// <summary>
        /// Выполняет расчет скорости.
        /// </summary>
        /// <remarks>
        /// Скорость дрона постоянна. С учетом практически моментального 
        /// разгона и торможения.
        /// </remarks>
        /// <returns>Возвращает значение скорости в км/ч.</returns>
        public int GetSpeed()
        {
            const int Speed = 50;
            return Speed;
        }

        /// <summary>
        /// Выполняет расчет скорости для расчета текущих координат.
        /// </summary>
        /// <remarks>
        /// Скорость дрона постоянна.
        /// </remarks>
        /// <param name="hour">Часы.</param>
        /// <param name="minute">Минуты.</param>
        /// <returns>Возвращает новое значение скорости в км/ч.</returns>
        public int GetNewSpeed(double hour, double minute) => Speed;

        
        /// <summary>
        /// Выполняет расчет нового пройденного расстояния до текущих координат.
        /// </summary>
        /// <remarks>
        /// Расчет нового расстояния выполняется по формуле d = v * t.
        /// </remarks>
        /// <param name="hour">Часы.</param>
        /// <param name="minute">Минуты.</param>
        /// <param name="velocity">Скорость.</param>
        /// <param name="metres">Выходной параметр метры.</param>
        /// <param name="kilometres">Выходной параметр километры.</param>
        public void FlyToNew(double hour, double minute, 
            double velocity, 
            out int metres, out int kilometres)
        {
            const int CoeffMtrs = IFlyable.CoeffMtrs;
            double tm = (minute / CoeffMin + hour) * CoeffMin;  // время - в минуты
            double time = Math.Round(tm, 2);

            newDest = GetDest(time);

            kilometres = Convert.ToInt32(newDest) / CoeffMtrs;
            metres = Convert.ToInt32(newDest % CoeffMtrs);

            IFlyable.newDest = newDest;


            // Расчитывает новое расстояние полета.
            double GetDest(double time)
            {
                const double MinPerMtr = 0.0201;    // минут за 1 метр
                int dest = 0;                       // промежуточное расстояние
                int stop = 0;                       // остановка в полете
                double t = 0;                       // промежуточное время

                for (int i = 0; i < time; i++)
                {
                    // За 1 минуту дрон пролетает 50 метров.
                    for (int j = 0; j < MtrPerMin; j++)
                    {
                        dest++;
                        t += MinPerMtr;
                    }
                    // Каждые 10 мин. - делает остановку (1 мин.)
                    if (Convert.ToInt32(t) % 10 == 0) stop++;
                }
                return dest - (stop * MtrPerMin);
            }
        }

        /// <summary>
        /// Выполняет расчет времени за пройденное расстояние.
        /// </summary>
        /// <param name="velocity">Скорость</param>
        /// <returns>Возвращает время в часах, минутах и секундах.</returns>
        public (int hours, int minutes, int seconds) GetFlyTime(double velocity)
        {
            const int CoeffHour = IFlyable.CoeffHour;
            double destin = IFlyable.destin;
            double tm = GetTime(destin);
            double time = tm / CoeffMin * CoeffHour;
            int hr = Convert.ToInt32(time) / CoeffHour;
            int min = Convert.ToInt32(time % CoeffHour / CoeffMin);
            int sec = Convert.ToInt32(time % CoeffMin);

            return (hr, min, sec);


            // Расчитывает время полета.
            double GetTime(double destination)
            {
                const double CoeffMtrPerSec = 0.0139;   // метров в 1 секунду
                const double MinSec = 0.01667;          // единица от 72 сек
                const int SecPerMtr = 72;               // секунд за 1 метр
                int stop = 0;
                double dest = 0;
                double t = 0;

                for (int i = 0; i < destination; i++)
                {
                    // Каждые 72 секунды происходит подсчет расстояния за 1
                    // секунду, а также времени.
                    for (int j = 0; j < SecPerMtr; j++)
                    {
                        dest += CoeffMtrPerSec;
                        t += MinSec;
                    }
                    if (Convert.ToInt32(t / CoeffMin) % 10 == 0) stop++;
                }
                return (stop / MtrPerMin) + (t / CoeffMin);
            }
        }

        /// <summary>
        /// Выводит сообщение о соответствии введенного значения ограничению 
        /// после соответствующих расчетов.
        /// </summary>
        /// <returns>Возвращает сообщение для определенного объекта.</returns>
        public string GetMessage()
        {
            return $"{Name} не может лететь на дальность более {Limit}км:" +
                $"\nсогласно введенным координатам, со скростью {Speed}км/ч " +
                $"и в течение времени {Hours}ч.{Minutes}мин., он пролетит " +
                $"расстояние {Kilometres}км {Metres}м.";
        }
    }
}
