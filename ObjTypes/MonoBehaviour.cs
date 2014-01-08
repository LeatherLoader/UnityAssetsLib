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
        public uint GameObjectFileIndex { get; private set; }
        public uint GameObjectLocalIndex { get; private set; }
        public uint ScriptFileIndex { get; private set; }
        public uint ScriptLocalIndex { get; private set; }
        private string mScriptName;

        public MonoBehaviour()
        {

        }

        public MonoBehaviour(uint goFileIndex, uint goLocalIndex, uint scriptFileIndex, uint scriptLocalIndex, string scriptName)
        {
            mEnabled = true;
            GameObjectFileIndex = goFileIndex;
            GameObjectLocalIndex = goLocalIndex;
            ScriptFileIndex = scriptFileIndex;
            ScriptLocalIndex = scriptLocalIndex;
            mScriptName = scriptName;
        }

        public override uint CalculateSize()
        {
            uint size = 24;
            size += UnityHelper.ByteAlign((uint)mScriptName.Length, 4);
            return size + base.CalculateSize();
        }

        public override void Write(SwappableEndianBinaryWriter writer)
        {
            writer.Write(GameObjectFileIndex);
            writer.Write(GameObjectLocalIndex);
            writer.Write(mEnabled);
            for (int i = 0; i < 3; i++) { writer.Write((byte)0); }
            writer.Write(ScriptFileIndex);
            writer.Write(ScriptLocalIndex);
            writer.WriteUnityString(mScriptName);

            base.Write(writer);
        }

        public override void Read(SwappableEndianBinaryReader reader)
        {
            GameObjectFileIndex = reader.ReadUInt32();
            GameObjectLocalIndex = reader.ReadUInt32();
            mEnabled = (reader.ReadUInt32() != 0);
            ScriptFileIndex = reader.ReadUInt32();
            ScriptLocalIndex = reader.ReadUInt32();
            mScriptName = reader.ReadUnityString();

            base.Read(reader);
        }
    }
}
