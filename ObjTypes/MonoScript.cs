using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAssetsLib.ObjTypes
{
    public class MonoScript : IUnityType
    {
        private string mScriptName;
        private int mExecutionOrder;
        private uint mPropertiesHash;
        private string mClassName;
        private string mNamespace;
        public string Assembly { get; set; }
        private byte mIsEditorScript;
        private FileTypes.ObjectInfo mInfo;

        public MonoScript(string scriptName, int executionOrder, uint propertiesHash, string className, string nmSpace, string assembly, byte isEditorScript)
        {
            mScriptName = scriptName;
            mExecutionOrder = executionOrder;
            mPropertiesHash = propertiesHash;
            mClassName = className;
            mNamespace = nmSpace;
            Assembly = assembly;
            mIsEditorScript = isEditorScript;
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
            //Size of static fields
            uint size = 9;

            size += 4 + UnityHelper.ByteAlign((uint)mScriptName.Length, 4);
            size += 4 + UnityHelper.ByteAlign((uint)mClassName.Length, 4);
            size += 4 + UnityHelper.ByteAlign((uint)mNamespace.Length, 4);
            size += 4 + UnityHelper.ByteAlign((uint)Assembly.Length, 4);

            return size;
        }

        public void Write(SwappableEndianBinaryWriter writer)
        {
            writer.WriteUnityString(this.mScriptName);
            writer.Write(this.mExecutionOrder);
            writer.Write(this.mPropertiesHash);
            writer.WriteUnityString(this.mClassName);
            writer.WriteUnityString(this.mNamespace);
            writer.WriteUnityString(this.Assembly);
            writer.Write(this.mIsEditorScript);

            uint remainingSize = UnityHelper.ByteAlign(CalculateSize(), 8) - CalculateSize();

            for (uint i = 0; i < remainingSize; i++) { writer.Write((byte)0); }
        }


        public int GetClassId()
        {
            return 115;
        }

        public int GetTypeId()
        {
            return 115;
        }
    }
}
