using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityAssetsLib.FileTypes;

namespace UnityAssetsLib.ObjTypes
{
    public abstract class UnityType
    {
        public ObjectInfo Info { get; set; }
        public abstract uint CalculateSize();
        public abstract void Write(SwappableEndianBinaryWriter writer);
        public abstract void Read(SwappableEndianBinaryReader reader);
    }
}
