using System.IO;
using System.Numerics;

namespace MapGenerator.Q3
{
    public class PatchDrawVertex
    {
        public Vector3 Vertex { get; set; }
        public float S { get; set; }
        public float T { get; set; }

        private PatchDrawVertex(Vector3 vertex, float s, float t)
        {
            Vertex = vertex;
            S = s;
            T = t;
        }

        public void RotateOrigin(Vector3 vRotation, Vector3 vOrigin)
        {
            VectorHelper.VectorRotateOrigin(Vertex, vRotation, vOrigin, out var vertex);

            Vertex = vertex;
        }

        public void Move(float x, float y, float z)
        {
            Vector3 adjustment = new Vector3(x, y, z);
            VectorHelper.VectorAdd(Vertex, adjustment, out var vertex);
            Vertex = vertex;
        }

        public static PatchDrawVertex Parse(Tokenizer tokenizer)
        {
            tokenizer.GetNextType(Tokenizer.TokenType.StartParen);

            var x = float.Parse(tokenizer.GetNextValue());
            var y = float.Parse(tokenizer.GetNextValue());
            var z = float.Parse(tokenizer.GetNextValue());
            var vertex = new Vector3(x, y, z);
            var s = float.Parse(tokenizer.GetNextValue());
            var t = float.Parse(tokenizer.GetNextValue());

            tokenizer.GetNextType(Tokenizer.TokenType.EndParen);


            return new PatchDrawVertex(vertex, s, t);
        }

        public void WriteTo(TextWriter writer)
        {
            writer.Write(Tokenizer.Token.StartParen.Contents);
            writer.Write(" ");
            writer.Write(Vertex.X.ToString("F6"));
            writer.Write(" ");
            writer.Write(Vertex.Y.ToString("F6"));
            writer.Write(" ");
            writer.Write(Vertex.Z.ToString("F6"));
            writer.Write(" ");
            writer.Write(S.ToString("F6"));
            writer.Write(" ");
            writer.Write(T.ToString("F6"));
            writer.Write(" ");
            writer.Write(Tokenizer.Token.EndParen.Contents);
        }
    }
}