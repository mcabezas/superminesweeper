using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Resources
{
    internal static class ImageResource
    {
        public static Image GetUnreveledBlankSquare(Size size) {
            return GetImage(@"Resources\\Images\\UnreveledBlankCell.PNG",
                size);
        }

        public static Image GetReveledBlankSquare(Size size)
        {
            return GetImage(@"Resources\\Images\\ReveledBlankCell.PNG",
                size);
        }

        public static Image GetReveledMineSquare(Size size)
        {
            return GetImage(@"Resources\\Images\\ReveledMine.PNG",
                size);
        }

        private static Image GetImage(string imagePath, Size size)
        {
            Image rawImage = new Bitmap(imagePath);
            return new Bitmap(rawImage, size);
        }

    }
}
