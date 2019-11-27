using System;

namespace BLL
{
    public class Predefined
    {
        private static readonly Random random = new Random();

        public static int RandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

    }
}