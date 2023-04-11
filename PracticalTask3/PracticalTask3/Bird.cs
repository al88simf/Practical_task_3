using System;

namespace PracticalTask3
{
    /// <summary>
    /// Класс обеспечивает вычисление данных полета птицы. 
    /// </summary>
    class Bird : IFlyable
    {
        private const double CoeffMtrPerMin = IFlyable.CoeffMtrPerMin;
        private const int CoeffHour = IFlyable.CoeffHour;
        private const int CoeffMin = IFlyable.CoeffMin;
        private const int CoeffMtrs = IFlyable.CoeffMtrs;
        
        /// <summary>
        /// Хранилище значения свойства Speed.
        /// </summary>
        private int speed;
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
        /// Ограничение представляет собой невозможность птиц находиться в полете
        /// без посадки более недели.
        /// </remarks>
        public int Limit { get; }

        /// <summary>
        /// Свойство Angle.
        /// </summary>
        public int Angle { get; set; }

        /// <summary>
        /// Свойство Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Свойство Speed.
        /// </summary>
        public int Speed
        {
            get => speed;
            set
            {
                if (value == 0) speed = 1;
                else speed = value;
            }
        }
        
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="name">Имя объекта.</param>
        /// <param name="limit">Ограничение.</param>
        public Bird(string name, int limit)
        {
            Name = name;
            Limit = limit;
        }
        
        /// <summary>
        /// Задает случайное значение скорости.
        /// </summary>
        /// <returns>Случайное значение скорости от 0 до 20 км/ч.</returns>
        public int GetSpeed()
        {
            Random random = new();
            return random.Next(20);
        }
        
        /// <summary>
        /// Задает новое значение скорости.
        /// </summary>
        /// <remarks>
        /// В данном случае (для птицы) скорость одинакова.
        /// </remarks>
        /// <param name="hour">Часы с начала полета.</param>
        /// <param name="minute">Минуты с начала полета.</param>
        /// <returns>Возвращает значение скорости спустя время после начала 
        /// полета в км/ч.</returns>
        public int GetNewSpeed(double hour, double minute) => Speed;


        /// <summary>
        /// Выполняет расчет нового пройденного расстояния до текущих координат.
        /// </summary>
        /// <remarks>
        /// Расчет нового расстояния выполняется по формуле нахождения 
        /// расстояния при равномерном движении d = v * t.
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
            // Для вычисления необходимо перевести км/ч в метр/мин.
            velocity *= CoeffMtrPerMin;
            double time = minute / CoeffMin + hour;     // время - в минуты
            double dest = velocity * time;              // расстояние в метрах

            newDest = Math.Round(dest, 2);

            kilometres = Convert.ToInt32(newDest) / CoeffMtrs;
            metres = Convert.ToInt32(newDest % CoeffMtrs);

            IFlyable.newDest = newDest;
        }

        /// <summary>
        /// Выполняет расчет времени за пройденное расстояние.
        /// </summary>
        /// <param name="velocity">Скорость</param>
        /// <returns>Возвращает время в часах, минутах и секундах.</returns>
        public (int hours, int minutes, int seconds) GetFlyTime(double velocity)
        {
            velocity *= CoeffMtrPerMin;     // км/ч в метр/мин.
            double destin = IFlyable.destin;
            double tm = destin / velocity;  // время в минутах

            // Для расчета выходных величин времени необходимо перевести время
            // в секунды.
            int time = Convert.ToInt32(Math.Round(tm, 2) * CoeffHour);
            int hr = Convert.ToInt32(time) / CoeffHour;
            int min = Convert.ToInt32(time % CoeffHour / CoeffMin);
            int sec = Convert.ToInt32(time % CoeffMin);
            return (hr, min, sec);
        }

        /// <summary>
        /// Выводит сообщение о соответствии введенного значения ограничению 
        /// после соответствующих расчетов.
        /// </summary>
        /// <returns>Возвращает сообщение для определенного объекта.</returns>
        public string GetMessage()
        {
            return $"{Name} не может находиться в полете более недели:" +
                $"\nсогласно введенным координатам, она пролетит расстояние " +
                $"{Kilometres}км {Metres}м, со скоростью {Speed}км/ч, в течение" +
                $" времени {Hours}ч., что больше {Limit}ч. - одной недели.";
        }
    }
}
