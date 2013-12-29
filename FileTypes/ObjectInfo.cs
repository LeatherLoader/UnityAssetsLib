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
        public uint OldOffset { get; set; }
        public uint OldSize { get; set; }
        public uint NewOffset { get; set; }
        public uint NewSize { get; set; }
        public int TypeId { get; set; }
        public int ClassId { get; set; }

        public ObjectInfo()
        {

        }

        public void Read(SwappableEndianBinaryReader reader)
        {
            this.Index = reader.ReadUInt32();
            this.OldOffset = reader.ReadUInt32();
            this.OldSize = reader.ReadUInt32();
            this.TypeId = reader.ReadInt32();
            this.ClassId = reader.ReadInt32();

            this.NewOffset = OldOffset;
            this.NewSize = OldSize;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.Index);
            writer.Write(this.NewOffset);
            writer.Write(this.NewSize);
            writer.Write(this.TypeId);
            writer.Write(this.ClassId);
        }
    }
}
