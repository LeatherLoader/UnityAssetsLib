using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAssetsLib.ObjTypes
{
    public class MonoManager : KnownType
    {
        public List<MonoManager.Script> Scripts { get; private set; }
        public List<string> Assemblies { get; private set; }

        public MonoManager()
        {
            this.Scripts = new List<Script>();
            this.Assemblies = new List<string>();
        }

        public override uint CalculateSize()
        {
            uint size = 8 + ((uint)Scripts.Count * 8);

            foreach (string assembly in Assemblies)
            {
                size += 4 + UnityHelper.ByteAlign((uint)assembly.Length, 4);
            }

            return size + base.CalculateSize();
        }

        public override void Write(SwappableEndianBinaryWriter writer)
        {
            writer.Write((uint)this.Scripts.Count);
            foreach (Script script in this.Scripts)
            {
                script.Write(writer);
            }

            writer.Write((uint)this.Assemblies.Count);
            foreach (string assembly in this.Assemblies)
            {
                writer.WriteUnityString(assembly);
            }

            base.Write(writer);
        }

        public override void Read(SwappableEndianBinaryReader reader)
        {
            uint scriptCount = reader.ReadUInt32();

            for (uint i = 0; i < scriptCount; i++)
            {
                Script script = new Script();
                script.Read(reader);
                this.Scripts.Add(script);
            }

            uint assemblies = reader.ReadUInt32();

            for (uint i = 0; i < assemblies; i++)
            {
                this.Assemblies.Add(reader.ReadUnityString());
            }

            base.Read(reader);
        }

        public class Script
        {
            public uint ScriptFileIndex { get; set; }
            public uint ScriptLocalIndex { get; set; }

            public Script()
            {

            }

            public void Write(SwappableEndianBinaryWriter writer) 
            {
                writer.Write(this.ScriptFileIndex);
                writer.Write(this.ScriptLocalIndex);
            }

            public void Read(SwappableEndianBinaryReader reader)
            {
                this.ScriptFileIndex = reader.ReadUInt32();
                this.ScriptLocalIndex = reader.ReadUInt32();
            }
        }
    }
}
