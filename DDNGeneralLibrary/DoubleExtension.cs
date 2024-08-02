using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDNGeneralLibrary
{
    public static class DoubleExtension
    {
        public static double ExtGetAverageTo(this double number1, double number2)
        {
            return (number1 + number2) / 2;
        }
        public static double ExtRadianToAngle(this double radian)
        {
            return radian / Math.PI * 180;
        }

    }
}