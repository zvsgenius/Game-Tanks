using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class Tank: IRun, ITurn, ITransparent, ICurrentPicture, ITurnAround
    {
        TankImg tankImg = new TankImg();

        void PutImg()
        {
            if (direct_x == 1)
                img = tankImg.Right;
            if (direct_x == -1)
                img = tankImg.Left;
            if (direct_y == 1)
                img = tankImg.Down;
            if (direct_y == -1)
                img = tankImg.Up;
        }

        protected Image[] img;
        protected Image currentImg;
        protected int sizeField;
        protected int x, y, direct_x, direct_y;
        protected int k;
        protected static Random r;

        protected void PutCurrentImage()
        {
            currentImg = img[k];
            k++;
            if (k == 4)
                k = 0;
        }

        public Tank(int sizeField, int x, int y)
        {
            this.sizeField = sizeField;
            r = new Random();


            if (r.Next(5000) < 2500)
            {
                Direct_y = 0;
            looop:
                Direct_x = r.Next(-1, 2);
                if (Direct_x == 0)
                    goto looop;
            }
            else
            {
                Direct_x = 0;
            looop:
                Direct_y = r.Next(-1, 2);
                if (Direct_y == 0)
                    goto looop;
            }

            PutImg();
            PutCurrentImage();

            this.x = x;
            this.y = y;
        }

        public Image CurrentImg
        {
            get
            {
                return currentImg;
            }
        }

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

        public int Direct_x
        {
            get
            {
                return direct_x;
            }

            set
            {
                if (value == 0 || value == -1 || value == 1)
                    direct_x = value;
                else direct_x = 0;
            }
        }
        public int Direct_y
        {
            get
            {
                return direct_y;
            }

            set
            {
                if (value == 0 || value == -1 || value == 1)
                    direct_y = value;
                else direct_y = 0;
            }
        }

        public void Run()
        {
            x += direct_x;
            y += direct_y;

            if (Math.IEEERemainder(x, 40) == 0 && Math.IEEERemainder(y, 40) == 0)
                Turn();

            PutCurrentImage();

            Transparent();
        }
        public void Turn() // Поворот на перекрёстке
        {

                if (r.Next(5000) < 2500)//двигаемся по вертикали
                {
                    if (Direct_y == 0)
                    {
                        Direct_x = 0;
                        while (Direct_y == 0)
                            Direct_y = r.Next(-1, 2);
                    }
                    
                }
                else//двигаемся по горизонтали
                {
                    if (Direct_x == 0)
                    {
                        Direct_y = 0;
                        while (Direct_x == 0)
                            Direct_x = r.Next(-1, 2);
                    }
                }
            PutImg();
        }
        public void Transparent() // Пересечение контуров поля и перемещение на противоположную сторону
        {
            if (x == -1)
                x = sizeField - 21;
            if (x == sizeField-19)
                x = 1;

            if (y == -1)
                y = sizeField - 21;
            if (y == sizeField - 19)
                y = 1;
        }
        public void TurnAround()
        {
            Direct_x = -1 * Direct_x;
            Direct_y = -1 * Direct_y;
            PutImg();
        }
    }
}
