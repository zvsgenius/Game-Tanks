﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class FireTankImg
    {
        Image[] img = new Image[] { Properties.Resources.FireTank1,
                                    Properties.Resources.FireTank2,
                                    Properties.Resources.FireTank3,
                                    Properties.Resources.FireTank4,
                                    Properties.Resources.FireTank5,
                                    Properties.Resources.FireTank6 };

        public Image[] Img
        {
            get
            {
                return img;
            }

        }
    }
}
