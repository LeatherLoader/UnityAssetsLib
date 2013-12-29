using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityAssetsLib.FileTypes;

namespace UnityAssetsLib.ObjTypes
{
    public interface IUnityType
    {
        ObjectInfo Info { get; set; }
        uint CalculateSize();
        void Write(SwappableEndianBinaryWriter writer);
        int GetClassId();
        int GetTypeId();
    }
}
