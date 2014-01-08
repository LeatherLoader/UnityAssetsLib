using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAssetsLib.ObjTypes
{
    public class KnownType : UnityType
    {
        private byte[] mAdditionalData;

        public KnownType()
        {
            mAdditionalData = new byte[0];
        }

        public override uint CalculateSize()
        {
            return (uint)mAdditionalData.Length;
        }

        public override void Write(SwappableEndianBinaryWriter writer)
        {
            if (mAdditionalData.Length > 0)
                writer.Write(mAdditionalData);

            uint remainingSize = UnityHelper.ByteAlign(this.Info.NewSize, 8) - this.Info.NewSize;

            for (uint i = 0; i < remainingSize; i++) { writer.Write((byte)0); }
        }

        public override void Read(SwappableEndianBinaryReader reader)
        {
            if (this.Info.OldSize > this.CalculateSize())
            {
                mAdditionalData = reader.ReadBytes((int)(this.Info.OldSize - this.CalculateSize()));
            }

            uint remainingSize = UnityHelper.ByteAlign(this.Info.OldSize, 8) - this.Info.OldSize;

            for (uint i = 0; i < remainingSize; i++) { reader.ReadByte(); }
        }
    }
}
