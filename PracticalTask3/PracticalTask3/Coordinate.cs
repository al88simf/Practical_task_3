namespace PracticalTask3
{
    /// <summary>
    /// Структура предоставляет ряд координат для начальной и новой точек.
    /// </summary>
    struct Coordinate
    {
        /// <summary>
        /// Свойство StartX.
        /// </summary>
        public int StartX { get; }

        /// <summary>
        /// Свойство StartY.
        /// </summary>
        public int StartY { get; }
        
        /// <summary>
        /// Свойство StartZ
        /// </summary>
        public int StartZ { get; }

        /// <summary>
        /// Свойство NewX.
        /// </summary>
        public int NewX { get; set; }

        /// <summary>
        /// Свойство NewY.
        /// </summary>
        public int NewY { get; set; }

        /// <summary>
        /// Свойство NewZ.
        /// </summary>
        public int NewZ { get; set; }

        /// <summary>
        /// Конструктор структуры.
        /// </summary>
        /// <param name="sX">Координата начальной точки X.</param>
        /// <param name="sY">Координата начальной точки Y.</param>
        /// <param name="sZ">Координата начальной точки Z.</param>
        /// <param name="nX">Координата новой точки X.</param>
        /// <param name="nY">Координата новой точки Y.</param>
        /// <param name="nZ">Координата новой точки Z.</param>
        public Coordinate(int sX, int sY, int sZ, 
            int nX, int nY, int nZ)
        {
            (StartX, StartY, StartZ, 
                NewX, NewY, NewZ) = 
                (sX, sY, sZ, 
                nX, nY, nZ);
        }
    }
}
