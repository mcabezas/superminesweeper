using System;

namespace BLL
{
    public class Predefined
    {
        public static int RandomInt(int min, int max)
        {
            var rnd = new Random();
            return rnd.Next(min, max);
        }

    }
}