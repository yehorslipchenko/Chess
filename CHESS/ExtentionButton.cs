using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHESS
{
    static class ExtentionButton
    {
        public static bool HaveImage(this Button button)
        {
            bool res = false;
            if (button.BackgroundImage != null)
                res = true;
            return res;
        }
        
    }
}
