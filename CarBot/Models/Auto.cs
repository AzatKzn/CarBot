using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.Models
{
    class Auto
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Доступна для показа
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// Признак доступности в магазине
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Необходимая характеристика для покупки
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Значение необходимой характеристики для покупки
        /// </summary>
        public int PropertyValue { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Скорость
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Маневренность
        /// </summary>
        public int Mobility { get; set; }

        /// <summary>
        /// Разгон
        /// </summary>
        public int Overclocking { get; set; }

        /// <summary>
        /// Торможение
        /// </summary>
        public int Braking { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public int Cost { get; set; }
    }
}
