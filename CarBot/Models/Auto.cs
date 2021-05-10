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
        
        public bool IsEnabled { get; set; }

        public string Property { get; set; }

        public int PropertyValue { get; set; }

        public string Name { get; set; }

        public int Speed { get; set; }

        public int Mobility { get; set; }

        public int Overclocking { get; set; }

        public int Braking { get; set; }

        public int Cost { get; set; }
    }
}
