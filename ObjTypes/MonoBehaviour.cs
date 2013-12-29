using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAssetsLib.ObjTypes
{
    public class MonoBehaviour : IUnityType
    {
        private bool mEnabled;
        private uint mGameObjectFileIndex;
        private uint mGameObjectLocalIndex;
        private uint mScriptFileIndex;
        private uint mScriptLocalIndex;
        private string mScriptName;
        private FileTypes.ObjectInfo mInfo;

        public MonoBehaviour(uint goFileIndex, uint goLocalIndex, uint scriptFileIndex, uint scriptLocalIndex, string scriptName)
        {
            mEnabled = true;
            mGameObjectFileIndex = goFileIndex;
            mGameObjectLocalIndex = goLocalIndex;
            mScriptFileIndex = scriptFileIndex;
            mScriptLocalIndex = scriptLocalIndex;
            mScriptName = scriptName;
        }

        public FileTypes.ObjectInfo Info
        {
            get
            {
                return mInfo;
            }
            set
            {
                mInfo = value;
            }
        }

        public uint CalculateSize()
        {
            uint size = 24;
            size += UnityHelper.ByteAlign((uint)mScriptName.Length, 4);
            return size;
        }

        public void Write(SwappableEndianBinaryWriter writer)
        {
            writer.Write(mGameObjectFileIndex);
            writer.Write(mGameObjectLocalIndex);
            writer.Write(mEnabled);
            for (int i = 0; i < 3; i++) { writer.Write((byte)0); }
            writer.Write(mScriptFileIndex);
            writer.Write(mScriptLocalIndex);
            writer.WriteUnityString(mScriptName);

            uint remainingSize = UnityHelper.ByteAlign(CalculateSize(), 8) - CalculateSize();

            for (uint i = 0; i < remainingSize; i++) { writer.Write((byte)0); }
        }

        public int GetClassId()
        {
            return 114;
        }

        public int GetTypeId()
        {
            return -1;
        }
    }
}
