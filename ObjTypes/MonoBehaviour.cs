using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAssetsLib.ObjTypes
{
    public class MonoBehaviour : KnownType
    {
        private bool mEnabled;
        private uint mGameObjectFileIndex;
        private uint mGameObjectLocalIndex;
        private uint mScriptFileIndex;
        private uint mScriptLocalIndex;
        private string mScriptName;

        public MonoBehaviour()
        {

        }

        public MonoBehaviour(uint goFileIndex, uint goLocalIndex, uint scriptFileIndex, uint scriptLocalIndex, string scriptName)
        {
            mEnabled = true;
            mGameObjectFileIndex = goFileIndex;
            mGameObjectLocalIndex = goLocalIndex;
            mScriptFileIndex = scriptFileIndex;
            mScriptLocalIndex = scriptLocalIndex;
            mScriptName = scriptName;
        }

        public override uint CalculateSize()
        {
            uint size = 24;
            size += UnityHelper.ByteAlign((uint)mScriptName.Length, 4);
            return size;
        }

        public override void Write(SwappableEndianBinaryWriter writer)
        {
            writer.Write(mGameObjectFileIndex);
            writer.Write(mGameObjectLocalIndex);
            writer.Write(mEnabled);
            for (int i = 0; i < 3; i++) { writer.Write((byte)0); }
            writer.Write(mScriptFileIndex);
            writer.Write(mScriptLocalIndex);
            writer.WriteUnityString(mScriptName);

            base.Write(writer);
        }

        public override void Read(SwappableEndianBinaryReader reader)
        {
            mGameObjectFileIndex = reader.ReadUInt32();
            mGameObjectLocalIndex = reader.ReadUInt32();
            mEnabled = (reader.ReadUInt32() != 0);
            mScriptFileIndex = reader.ReadUInt32();
            mScriptLocalIndex = reader.ReadUInt32();
            mScriptName = reader.ReadUnityString();

            base.Read(reader);
        }
    }
}
