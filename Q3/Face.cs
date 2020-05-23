using System;
using System.IO;
using System.Numerics;

namespace MapGenerator.Q3
{
    public class Face
    {
        public Vector3 V1 { get; set; }
        public Vector3 V2 { get; set; }
        public Vector3 V3 { get; set; }
        public string TextureName { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int Rotation { get; set; }
        public float XScale { get; set; }
        public float YScale { get; set; }
        public int Flag1 { get; set; }
        public int Flag2 { get; set; }
        public int Flag3 { get; set; }

        public Face(Vector3 v1, Vector3 v2, Vector3 v3, string textureName, int xOffset, int yOffset, int rotation, float xScale, float yScale, int flag1, int flag2, int flag3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            TextureName = textureName;
            XOffset = xOffset;
            YOffset = yOffset;
            Rotation = rotation;
            XScale = xScale;
            YScale = yScale;
            Flag1 = flag1;
            Flag2 = flag2;
            Flag3 = flag3;
        }

        public static Face Parse(Tokenizer tokenizer)
        {
            Vector3 v1 = Vector3Extensions.Parse(tokenizer);
            Vector3 v2 = Vector3Extensions.Parse(tokenizer);
            Vector3 v3 = Vector3Extensions.Parse(tokenizer);
            string textureName = tokenizer.GetNextValue();
            int xOffset = Convert.ToInt32(tokenizer.GetNextValue());
            int yOffset = Convert.ToInt32(tokenizer.GetNextValue());
            int rotation = Convert.ToInt32(tokenizer.GetNextValue());
            float xScale = Convert.ToSingle(tokenizer.GetNextValue());
            float yScale = Convert.ToSingle(tokenizer.GetNextValue());

            int flag1 = Convert.ToInt32(tokenizer.GetNextValue());
            int flag2 = Convert.ToInt32(tokenizer.GetNextValue());
            int flag3 = Convert.ToInt32(tokenizer.GetNextValue());

            return new Face(v1, v2, v3, textureName, xOffset, yOffset, rotation, xScale, yScale, flag1, flag2, flag3);
        }

        public void WriteTo(TextWriter writer)
        {
            V1.WriteTo(writer);
            writer.Write(" ");
            V2.WriteTo(writer);
            writer.Write(" ");
            V3.WriteTo(writer);
            writer.Write(" ");
            writer.Write(TextureName);
            writer.Write(" ");
            writer.Write(XOffset);
            writer.Write(" ");
            writer.Write(YOffset);
            writer.Write(" ");
            writer.Write(Rotation);
            writer.Write(" ");
            writer.Write(XScale.ToString("F6"));
            writer.Write(" ");
            writer.Write(YScale.ToString("F6"));
            writer.Write(" ");
            writer.Write(Flag1);
            writer.Write(" ");
            writer.Write(Flag2);
            writer.Write(" ");
            writer.Write(Flag3);
            writer.WriteLine();
        }

        public void RotateOrigin(Vector3 vRotation, Vector3 vOrigin)
        {
            VectorHelper.VectorRotateOrigin(V1, vRotation, vOrigin, out var v1);
            VectorHelper.VectorRotateOrigin(V2, vRotation, vOrigin, out var v2);
            VectorHelper.VectorRotateOrigin(V3, vRotation, vOrigin, out var v3);

            V1 = v1;
            V2 = v2;
            V3 = v3;
        }

        public void Move(float x, float y, float z)
        {
            Vector3 adjustment = new Vector3(x, y, z);
            VectorHelper.VectorAdd(V1, adjustment, out var v1);
            VectorHelper.VectorAdd(V2, adjustment, out var v2);
            VectorHelper.VectorAdd(V3, adjustment, out var v3);
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }
    }
}
