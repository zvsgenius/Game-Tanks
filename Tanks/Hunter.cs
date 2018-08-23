using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class Hunter:Tank
    {
        HunterImg hunterImg = new HunterImg();

        void PutImg()
        {
            if (direct_x == 1)
                img = hunterImg.Right;
            if (direct_x == -1)
                img = hunterImg.Left;
            if (direct_y == 1)
                img = hunterImg.Down;
            if (direct_y == -1)
                img = hunterImg.Up;
        }

        public Hunter(int sizeFild,int x, int y):base(sizeFild,x,y) { PutImg(); PutCurrentImage(); }

        public void Turn(int target_x, int target_y)
        {
            Direct_x = Direct_y = 0;

            if (X > target_x)
                Direct_x = -1;
            if (X < target_x)
                Direct_x = 1;
            if (Y > target_y)
                Direct_y = -1;
            if (Y < target_y)
                Direct_y = 1;

            if(Direct_x != 0 && Direct_y != 0)
            {
                if (Math.Abs(X - target_x) > Math.Abs(Y - target_y))
                    Direct_y = 0;
                if (Math.Abs(X - target_x) <= Math.Abs(Y - target_y))
                    Direct_x = 0;
            }

            PutImg();
        }

        public void Run(int target_x, int target_y)
        {
            x += direct_x;
            y += direct_y;

            if (Math.IEEERemainder(x, 40) == 0 && Math.IEEERemainder(y, 40) == 0)
                Turn(target_x, target_y);

            PutCurrentImage();

            Transparent();
        }
        new public void TurnAround()
        {
            Direct_x = -1 * Direct_x;
            Direct_y = -1 * Direct_y;
            PutImg();
        }
    }
}
