using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityAssetsLib.ObjTypes;

namespace UnityAssetsLib.FileTypes
{
    public class AssetsFile
    {
        public AssetHeader Header { get; private set; }
        public List<UnityType> Objects { get; private set; }

        public AssetsFile()
        {
            this.Header = new AssetHeader();
            this.Objects = new List<UnityType>();
        }

        public void Read(SwappableEndianBinaryReader reader)
        {
            this.Header.Read(reader);

            foreach (ObjectInfo info in Header.FileData)
            {
                UnityType type = null;
                switch (info.ClassId)
                {
                    case 114:
                        type = new MonoBehaviour();
                        break;
                    case 115:
                        type = new MonoScript();
                        break;
                    default:
                        type = new UnknownType();
                        break;
                }
                type.Info = info;
                type.Read(reader);
            }
        }

        public void Write(SwappableEndianBinaryWriter writer)
        {
            this.Header.Write(writer);

            foreach (UnityType data in this.Objects)
            {
                data.Write(writer);
            }
        }
    }
}
