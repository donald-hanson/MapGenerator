using System;
using System.IO;
using System.Numerics;

namespace MapGenerator.Q3
{
    public static class Vector3Extensions
    {
        /*
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vertex3(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        public Vertex3()
        {
        }

        public void RotateZ(int degrees)
        {
            RotateZ((float)(degrees * Math.PI / 180.0));
        }

        public void RotateZ(float radians)
        {
            System.Numerics.Vector3 rotAxis = System.Numerics.Vector3.UnitZ;
            System.Numerics.Quaternion q = System.Numerics.Quaternion.CreateFromAxisAngle(rotAxis,  radians);
            System.Numerics.Vector3 aVector = new System.Numerics.Vector3(X, Y, Z);

            // rotate
            System.Numerics.Quaternion resultQ = q * (new System.Numerics.Quaternion(aVector, 0)) / q;

            X = resultQ.X;
            Y = resultQ.Y;
            Z = resultQ.Z;
        }
        */

        public static Vector3 SetIndex(this Vector3 vector, int index, float value)
        {
            switch (index)
            {
                case 0:
                    return new Vector3(value, vector.Y, vector.Z);
                case 1:
                    return new Vector3(vector.X, value, vector.Z);
                    break;
                case 2:
                    return new Vector3(vector.X, vector.Y, value);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static float GetIndex(this Vector3 vector, int index)
        {
            return index switch
            {
                0 => vector.X,
                1 => vector.Y,
                2 => vector.Z,
                _ => throw new InvalidOperationException()
            };
        }

        public static Vector3 Parse(Tokenizer tokenizer)
        {
            var v = new Vector3();
            var token = tokenizer.GetNextToken();
            if (token.Type != Tokenizer.TokenType.StartParen)
            {
                throw new FormatException("Expected an open paren, received a " + token);
            }
            v.X = Convert.ToSingle(tokenizer.GetNextValue());
            v.Y = Convert.ToSingle(tokenizer.GetNextValue());
            v.Z = Convert.ToSingle(tokenizer.GetNextValue());
            token = tokenizer.GetNextToken();
            if (token.Type != Tokenizer.TokenType.EndParen)
            {
                throw new FormatException("Expected an close paren, received a " + token);
            }

            return v;
        }

        public static void WriteTo(this Vector3 v, TextWriter writer)
        {
            writer.Write(Tokenizer.Token.StartParen.Contents);
            writer.Write(" ");
            writer.Write(v.X);
            writer.Write(" ");
            writer.Write(v.Y);
            writer.Write(" ");
            writer.Write(v.Z);
            writer.Write(" ");
            writer.Write(Tokenizer.Token.EndParen.Contents);
        }
    }
}
