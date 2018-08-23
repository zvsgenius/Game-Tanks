using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class FireTank
    {
        FireTankImg fireTankImg = new FireTankImg();
        Image curentImg;

        public Image CurentImg
        {
            get
            {
                return curentImg;
            }
        }

        Image[] img;

        int x, y;
        int k;

        public int X
        {
            get
            {
                return x;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
        }

        public FireTank(int x, int y)
        {
            this.x = x;
            this.y = y;

            img = fireTankImg.Img;

            PutCurrentImage();
        }

        void PutCurrentImage()
        {
            curentImg = img[k];
            k++;
            if (k == 6)
                k = 0;
        }

        public void Fire()
        {
            PutCurrentImage();
        }
    }
}
