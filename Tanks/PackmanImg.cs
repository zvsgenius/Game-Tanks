using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tanks
{
    class PackmanImg
    {
        Image[] up = new Image[] { Properties.Resources.Packman0_1, Properties.Resources.Packman0_1I, Properties.Resources.Packman0_1II, Properties.Resources.Packman0_1III };
        Image[] down = new Image[] { Properties.Resources.Packman01, Properties.Resources.Packman01I, Properties.Resources.Packman01II, Properties.Resources.Packman01III };
        Image[] left = new Image[] { Properties.Resources.Packman_10, Properties.Resources.Packman_10I, Properties.Resources.Packman_10II, Properties.Resources.Packman_10III };
        Image[] right = new Image[] { Properties.Resources.Packman10, Properties.Resources.Packman10I, Properties.Resources.Packman10II, Properties.Resources.Packman10III };

        public Image[] Up
        {
            get
            {
                return up;
            }

        }

        public Image[] Down
        {
            get
            {
                return down;
            }

        }

        public Image[] Left
        {
            get
            {
                return left;
            }

        }

        public Image[] Right
        {
            get
            {
                return right;
            }

        }
    }
}
