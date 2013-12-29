using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAssetsLib
{
    public class SwappableEndianBinaryReader : BinaryReader
    {
        public bool IsBigEndian { get; set; }

        public SwappableEndianBinaryReader(Stream stream)
            :base(stream)
        {
            IsBigEndian = true;
        }

        public void SwapEndianness()
        {
            IsBigEndian = !IsBigEndian;
        }

        public override uint ReadUInt32()
        {
            if (!IsBigEndian)
            {
                return base.ReadUInt32();
            }
            else
            {
                byte[] bytes = base.ReadBytes(4);
                Array.Reverse(bytes);
                return BitConverter.ToUInt32(bytes, 0);
            }
        }

        public override int ReadInt32()
        {
            if (!IsBigEndian)
            {
                return base.ReadInt32();
            }
            else
            {
                byte[] bytes = base.ReadBytes(4);
                Array.Reverse(bytes);
                return BitConverter.ToInt32(bytes, 0);
            }
        }

        public override ushort ReadUInt16()
        {
            if (!IsBigEndian)
            {
                return base.ReadUInt16();
            }
            else
            {
                byte[] bytes = base.ReadBytes(2);
                Array.Reverse(bytes);
                return BitConverter.ToUInt16(bytes, 0);
            }
        }

        public override short ReadInt16()
        {
            if (!IsBigEndian)
            {
                return base.ReadInt16();
            }
            else
            {
                byte[] bytes = base.ReadBytes(2);
                Array.Reverse(bytes);
                return BitConverter.ToInt16(bytes, 0);
            }
        }

        public string ReadNullTerminatedString()
        {
            StringBuilder builder = new StringBuilder();

            char lastChar = 'A';
            do
            {
                lastChar = base.ReadChar();

                if (lastChar != 0)
                {
                    builder.Append(lastChar);
                }
            } while (lastChar != 0);

            return builder.ToString();
        }

        public string ReadUnityString()
        {
            int length = (int)this.ReadUInt32();

            if (length <= 0)
                return string.Empty;

            int lengthAligned = (int)UnityHelper.ByteAlign((uint)length, 4);

            byte[] strBytes = this.ReadBytes(length);

            if (lengthAligned > length)
                this.ReadBytes(lengthAligned - length);

            return Encoding.ASCII.GetString(strBytes);
        }
    }
}
