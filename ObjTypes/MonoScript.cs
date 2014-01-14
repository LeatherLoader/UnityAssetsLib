using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAssetsLib.ObjTypes
{
    public class MonoScript : KnownType
    {
        private string mScriptName;
        private int mExecutionOrder;
        private uint mPropertiesHash;
        private string ClassName;
        private string mNamespace;
        public string Assembly { get; set; }
        private byte mIsEditorScript;

        public MonoScript()
        {

        }

        public MonoScript(string scriptName, int executionOrder, uint propertiesHash, string className, string nmSpace, string assembly, byte isEditorScript)
        {
            mScriptName = scriptName;
            mExecutionOrder = executionOrder;
            mPropertiesHash = propertiesHash;
            ClassName = className;
            mNamespace = nmSpace;
            Assembly = assembly;
            mIsEditorScript = isEditorScript;
            this.Info = new FileTypes.ObjectInfo() { ClassId = 115, TypeId = 115 };
        }

        public override uint CalculateSize()
        {
            //Size of static fields
            uint size = 9;

            size += 4 + UnityHelper.ByteAlign((uint)mScriptName.Length, 4);
            size += 4 + UnityHelper.ByteAlign((uint)ClassName.Length, 4);
            size += 4 + UnityHelper.ByteAlign((uint)mNamespace.Length, 4);
            size += 4 + UnityHelper.ByteAlign((uint)Assembly.Length, 4);

            return size + base.CalculateSize();
        }

        public override void Write(SwappableEndianBinaryWriter writer)
        {
            writer.WriteUnityString(this.mScriptName);
            writer.Write(this.mExecutionOrder);
            writer.Write(this.mPropertiesHash);
            writer.WriteUnityString(this.ClassName);
            writer.WriteUnityString(this.mNamespace);
            writer.WriteUnityString(this.Assembly);
            writer.Write(this.mIsEditorScript);

            base.Write(writer);
        }

        public override void Read(SwappableEndianBinaryReader reader)
        {
            this.mScriptName = reader.ReadUnityString();
            this.mExecutionOrder = reader.ReadInt32();
            this.mPropertiesHash = reader.ReadUInt32();
            this.ClassName = reader.ReadUnityString();
            this.mNamespace = reader.ReadUnityString();
            this.Assembly = reader.ReadUnityString();
            this.mIsEditorScript = reader.ReadByte();

            base.Read(reader);
        }
    }
}
