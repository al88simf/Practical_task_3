using System;

namespace PracticalTask3
{
    /// <summary>
    /// Класс обеспечивает вычисление данных полета самолета.
    /// </summary>
    class Plane : IFlyable
    {
        private const int CoeffMin = IFlyable.CoeffMin;
        private const int CoeffSec = 60;                // секунд в минуте
        private const double InitVel = 55.5;            // начальная скорость, м/с
        private const double MaxVel = 253;              // максимальная скорость м/с
        private const double CoeffMtrPerSec = 0.00027;  // коэффициент м/с
        private const double CoeffKMPerHour = 3.6;      // коэффициент км/ч
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
        /// Ограничение представляет собой невозможность находиться в полете 
        /// без посадки более 18 ч.
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
        /// <param name="name">Имя объекта.</param>
        /// <param name="limit">Ограничение.</param>
        public Plane(string name, int limit)
        {
            Name = name; 
            Limit = limit;
        }

        /// <summary>
        /// Выполняет расчет скорости.
        /// </summary>
        /// <returns>Возвращает значение скорости в км/ч.</returns>
        public int GetSpeed()
        {
            double dest = IFlyable.destin;
            double velocity = InitVel;

            for (int i = 0; i < dest; i++)
            {
                // Начальная скорость увеличивается с каждым метром, пока не
                // достигнет максимальной, далее не изменяется.
                if (velocity <= MaxVel) velocity += CoeffMtrPerSec;
            }
            return Convert.ToInt32(velocity * CoeffKMPerHour);
        }

        /// <summary>
        /// Выполняет расчет скорости для расчета текущих координат.
        /// </summary>
        /// <param name="hour">Часы.</param>
        /// <param name="minute">Минуты.</param>
        /// <returns>Возвращает новое значение скорости в км/ч.</returns>
        public int GetNewSpeed(double hour, double minute)
        {
            double tm = (minute / CoeffMin + hour) * CoeffMin;   // время - в минуты
            double time = Math.Round(tm, 2);
            double velocity = InitVel;

            // Начальная скорость увеличивается с каждой минутой, пока не
            // достигнет полного времени и/или максимальной скорости.
            for (int i = 0; i < time; i++)
            {
                for (int j = 0; j < CoeffSec; j++)
                {
                    if (velocity <= MaxVel) velocity += CoeffMtrPerSec;
                }
            }
            return Convert.ToInt32(velocity * CoeffKMPerHour);
        }

        /// <summary>
        /// Выполняет расчет нового пройденного расстояния до текущих координат.
        /// </summary>
        /// <remarks>
        /// Расчет нового расстояния выполняется по формуле нахождения расстояния 
        /// при равноускоренном движении S = V * t / 2.
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


            // Расчитывает новое расстояние.
            double GetDest(double time)
            {
                double velocity = InitVel;
                int tm = 0;     // промежуточное время
                double dest = 0;

                for (int i = 0; i < time; i++)
                {
                    // Расчет расстояния происходит каждую секунду времени полета,
                    // учитывая текущее значение скорости и времени, пока она не
                    // достигнет максимальной скорости и полного времени.
                    for (int j = 0; j < CoeffSec; j++)
                    {
                        tm++;
                        if (velocity <= MaxVel) velocity += CoeffMtrPerSec;
                        dest = velocity * tm / 2;
                    }
                }
                return dest;
            }
        }

        /// <summary>
        /// Выполняет расчет времени за пройденное расстояние.
        /// </summary>
        /// <remarks>
        /// Расчет происходит по формуле нахождения времени при равноускоренном 
        /// движении t = 2S / v.
        /// </remarks>
        /// <param name="velocity">Скорость</param>
        /// <returns>Возвращает время в часах, минутах и секундах.</returns>
        public (int hours, int minutes, int seconds) GetFlyTime(double velocity)
        {
            const int CoeffHour = IFlyable.CoeffHour;
            double destin = IFlyable.destin;
            double time = GetTime(destin);
            int hr = Convert.ToInt32(time) / CoeffHour;
            int min = Convert.ToInt32(time % CoeffHour / CoeffMin);
            int sec = Convert.ToInt32(time % CoeffMin);

            return (hr, min, sec);


            // Расчитывает время полета.
            double GetTime(double destin)
            {
                int dest = 0;       // промежуточное расстояние
                double vel = InitVel;
                double tm = 0;      // промежуточное время

                // Расчет времени происходит каждый метр, учитывая текущее
                // значение расстояния и скорости, пока скорость не достигнет 
                // максимальной, а расстояние полного.
                for (int i = 0; i < destin; i++)
                {
                    dest++;
                    if (vel <= MaxVel) vel += CoeffMtrPerSec;
                    tm = dest * 2 / vel;
                }
                return tm;
            }
        }

        /// <summary>
        /// Выводит сообщение о соответствии введенного значения ограничению 
        /// после соответствующих расчетов.
        /// </summary>
        /// <returns>Возвращает сообщение для определенного объекта.</returns>
        public string GetMessage()
        {
            return $"{Name} не может находиться в полете более {Limit}ч.:" +
                $"\nсогласно введенным координатам, он пролетит расстояние " +
                $"{Kilometres}км {Metres}м, со скоростью {Speed}км/ч, в течение " +
                $"времени {Hours}ч.{Minutes}мин.";
        }
    }
}
