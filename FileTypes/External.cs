using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAssetsLib.FileTypes
{
    public class External
    {
        private int mTypeId;
        private Guid mExternalGuid;
        public string Name { get; set; }

        public void Read(SwappableEndianBinaryReader reader)
        {
            mTypeId = reader.ReadInt32();
            byte[] guidBytes = reader.ReadBytes(16);
            mExternalGuid = new Guid(guidBytes);
            reader.ReadByte();
            this.Name = reader.ReadNullTerminatedString();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(mTypeId);
            byte[] guidBytes = mExternalGuid.ToByteArray();
            writer.Write(guidBytes);
            writer.Write((byte)0);

            byte[] nameBytes = Encoding.ASCII.GetBytes(this.Name);
            writer.Write(nameBytes);
            writer.Write((byte)0);
        }
    }
}
