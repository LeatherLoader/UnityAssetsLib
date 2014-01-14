using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityAssetsLib.ObjTypes;

namespace UnityAssetsLib.FileTypes
{
    public class AssetHeader
    {
        private uint mHeaderSize;
        public uint FileSize { get; set; }
        private uint mVersion;
        public uint OldDataStart { get; set; }
        public uint NewDataStart { get; set; }
        private bool mIsBigEndian;
        private byte[] mReserved;

        List<ObjectInfo> mObjectInfos = new List<ObjectInfo>();
        public List<External> Externals = new List<External>();

        public AssetHeader()
        {
            mIsBigEndian = false;
            mVersion = 9;
            mReserved = new byte[0x17] { 0, 0, 0, 0x34, 0x2E, 0x33, 0x2E, 0x31, 0x66, 0x31, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        public IEnumerable<ObjectInfo> FileData
        {
            get
            {
                return mObjectInfos.OrderBy(o => o.Offset);
            }
        }

        public void ReIndex(List<UnityType> objects)
        {
            this.mObjectInfos.Clear();

            uint offset = 0;
            uint index = 1;
            foreach (UnityType type in objects)
            {
                ObjectInfo info = new ObjectInfo() { ClassId = type.Info.ClassId, TypeId = type.Info.TypeId, Index = index, Offset = offset, Size = type.CalculateSize() };
                index++;
                offset += info.Size;
                offset = UnityHelper.ByteAlign(offset, 8);

                type.Info = info;
                this.mObjectInfos.Add(info);
            }
        }

        public void Read(SwappableEndianBinaryReader reader)
        {
            if (!reader.IsBigEndian) { reader.SwapEndianness(); }

            //Read header info
            mHeaderSize = reader.ReadUInt32();
            FileSize = reader.ReadUInt32();
            mVersion = reader.ReadUInt32();
            OldDataStart = reader.ReadUInt32();
            NewDataStart = OldDataStart;
            byte endiannessByte = reader.ReadByte();

            mIsBigEndian = (endiannessByte != 0);

            //Garbage reserved header data
            mReserved = reader.ReadBytes(0x17);

            if (reader.IsBigEndian != mIsBigEndian) { reader.SwapEndianness();  }

            //Read object infos
            uint objectInfoCount = reader.ReadUInt32();

            for (uint i = 0; i < objectInfoCount; i++)
            {
                ObjectInfo info = new ObjectInfo();
                info.Read(reader);
                mObjectInfos.Add(info);
            }

            //Read externals
            uint externalsCount = reader.ReadUInt32();

            for (uint i = 0; i < externalsCount; i++)
            {
                External external = new External();
                external.Read(reader);
                this.Externals.Add(external);
            }

            while (reader.BaseStream.Position < OldDataStart) { reader.ReadByte(); }
        }

        public void Write(SwappableEndianBinaryWriter writer)
        {
            //Add up size in bytes of externals
            int externalsSize = 0;

            foreach (External e in this.Externals)
            {
                externalsSize += 22 + e.Name.Length;
            }

            //Add up total header size
            uint newHeaderSize = (uint)(4 + (20*mObjectInfos.Count) + 4 + externalsSize + 40);
            //Get next 16-byte-aligned address after end of header
            NewDataStart = UnityHelper.ByteAlign(newHeaderSize, 16);

            //Don't start less than 4k into the file
            if (NewDataStart < 0x1000) { NewDataStart = 0x1000; }

            ObjectInfo lastObject = mObjectInfos.OrderByDescending(o => o.Offset).FirstOrDefault();
            uint dataSize = UnityHelper.ByteAlign(lastObject.Offset + lastObject.Size, 8);

            //Calculate new file size & also cut 20 bytes off the header size because I saw a thing that said to (???)
            uint newFileSize = NewDataStart + dataSize;
            newHeaderSize -= 19;

            //Start writing in big endian always, and then swap to little if the file demands it
            if (!writer.IsBigEndian) { writer.SwapEndianness(); }

            //Write basic header data
            writer.Write(newHeaderSize);
            writer.Write(newFileSize);
            writer.Write(mVersion);
            writer.Write(NewDataStart);

            if (mIsBigEndian)
                writer.Write((byte)1);
            else
                writer.Write((byte)0);

            //Write reserved garbage data
            writer.Write(mReserved);

            //Swap to little endian if we need to
            if (writer.IsBigEndian != mIsBigEndian) { writer.SwapEndianness(); }

            //Write object infos
            writer.Write((uint)mObjectInfos.Count);

            foreach (ObjectInfo info in mObjectInfos)
            {
                info.Write(writer);
            }

            //Write externals
            writer.Write((uint)this.Externals.Count);

            foreach (External e in this.Externals)
            {
                e.Write(writer);
            }

            //Write the garbage data between the end of the header & beginning of object data
            while (writer.BaseStream.Position < NewDataStart)
            {
                writer.Write((byte)0);
            }
        }
    }
}
