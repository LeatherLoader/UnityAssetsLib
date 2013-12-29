using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAssetsLib
{
    public class UnityHelper
    {
        public static uint ByteAlign(uint size, uint alignment)
        {
            return ((size + alignment - 1) & ~(alignment - 1));
        }
    }
}
