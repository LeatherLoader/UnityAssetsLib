using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAssetsLib
{
    public class SwappableEndianBinaryWriter : BinaryWriter
    {
        public bool IsBigEndian { get; set; }

        public SwappableEndianBinaryWriter(Stream stream)
            :base(stream)
        {
            IsBigEndian = true;
        }

        public void SwapEndianness()
        {
            IsBigEndian = !IsBigEndian;
        }

        public override void Write(uint value)
        {
            if (!IsBigEndian)
            {
                base.Write(value);
                return;
            }
            else
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                base.Write(bytes);
            }
        }

        public override void Write(int value)
        {
            if (!IsBigEndian)
            {
                base.Write(value);
                return;
            }
            else
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                base.Write(bytes);
            }
        }

        public override void Write(ushort value)
        {
            if (!IsBigEndian)
            {
                base.Write(value);
                return;
            }
            else
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                base.Write(bytes);
            }
        }

        public override void Write(short value)
        {
            if (!IsBigEndian)
            {
                base.Write(value);
                return;
            }
            else
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                base.Write(bytes);
            }
        }

        public void WriteUnityString(string str)
        {
            base.Write((uint)str.Length);
            byte[] allBytes = Encoding.ASCII.GetBytes(str);
            base.Write(allBytes);

            uint alignedBytes = UnityHelper.ByteAlign((uint)str.Length, 4);

            if (alignedBytes > str.Length)
            {
                for (int i = 0; i < (alignedBytes - str.Length); i++ )
                {
                    base.Write((byte)0);
                }
            }
        }
    }
}
