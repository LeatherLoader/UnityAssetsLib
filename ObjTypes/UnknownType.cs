using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAssetsLib.ObjTypes
{
    public class UnknownType : UnityType
    {
        private byte[] mData;

        public UnknownType()
        {
        }

        public UnknownType(byte[] data)
        {
            this.mData = data;
        }

        public override uint CalculateSize()
        {
            return (uint)mData.Length;
        }

        public override void Write(SwappableEndianBinaryWriter writer)
        {
            writer.Write(mData);

           uint padding = UnityHelper.ByteAlign((uint)mData.Length, 8) - (uint)mData.Length;

            for (uint i = 0; i < padding; i++) { writer.Write((byte)0); }
        }

        public override void Read(SwappableEndianBinaryReader reader)
        {
            mData = reader.ReadBytes((int)this.Info.Size);

            uint padding = UnityHelper.ByteAlign((uint)mData.Length, 8) - (uint)mData.Length;

            for (uint i = 0; i < padding; i++) { reader.ReadByte(); }
        }
    }
}
