using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telematics.Simulator.Core.Helpers
{
    public static class RandomHelper
    {
        public static int NegativePositive(this Random r)
        {
            int iNum;

            iNum = r.Next(-30, 50); //put whatever range you want in here from negative to positive 
            if (iNum == 0)
            {
                iNum++; //to avoid divide by zero
            }

            return iNum / (int)Math.Abs(iNum);

        }
    }
}
