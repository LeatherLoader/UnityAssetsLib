using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAssetsLib.FileTypes
{
    public class ObjectInfo
    {
        public uint Index { get; set; }
        public uint Offset { get; set; }
        public uint Size { get; set; }
        public int TypeId { get; set; }
        public int ClassId { get; set; }

        public ObjectInfo()
        {

        }

        public void Read(SwappableEndianBinaryReader reader)
        {
            this.Index = reader.ReadUInt32();
            this.Offset = reader.ReadUInt32();
            this.Size = reader.ReadUInt32();
            this.TypeId = reader.ReadInt32();
            this.ClassId = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.Index);
            writer.Write(this.Offset);
            writer.Write(this.Size);
            writer.Write(this.TypeId);
            writer.Write(this.ClassId);
        }
    }
}
