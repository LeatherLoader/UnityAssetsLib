using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAssetsLib.ObjTypes
{
    public class GameObject : KnownType
    {
        public List<Component> Components { get; private set; }
        private uint mLayer;
        public string Name { get; private set; }

        public GameObject()
        {
            this.Components = new List<Component>();
        }

        public override uint CalculateSize()
        {
            uint size = 12 + ((uint)this.Components.Count * 12);
            size += UnityHelper.ByteAlign((uint)Name.Length, 4);
            return size + base.CalculateSize();
        }

        public override void Write(SwappableEndianBinaryWriter writer)
        {
            writer.Write((uint)this.Components.Count);

            foreach (Component comp in this.Components)
            {
                comp.Write(writer);
            }

            writer.Write(mLayer);
            writer.WriteUnityString(this.Name);

            base.Write(writer);
        }

        public override void Read(SwappableEndianBinaryReader reader)
        {
            uint componentCount = reader.ReadUInt32();

            for (uint i = 0; i < componentCount; i++)
            {
                Component comp = new Component();
                comp.Read(reader);
                Components.Add(comp);
            }

            mLayer = reader.ReadUInt32();

            this.Name = reader.ReadUnityString();

            base.Read(reader);
        }

        public class Component
        {
            public int TypeId { get; set; }
            public uint FileIndex { get; set; }
            public uint LocalIndex { get; set; }

            public Component()
            {

            }

            public void Write(SwappableEndianBinaryWriter writer)
            {
                writer.Write(this.TypeId);
                writer.Write(this.FileIndex);
                writer.Write(this.LocalIndex);
            }

            public void Read(SwappableEndianBinaryReader reader)
            {
                this.TypeId = reader.ReadInt32();
                this.FileIndex = reader.ReadUInt32();
                this.LocalIndex = reader.ReadUInt32();
            }
        }
    }
}
