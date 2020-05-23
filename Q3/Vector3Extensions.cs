using System;
using System.IO;
using System.Numerics;

namespace MapGenerator.Q3
{
    public static class Vector3Extensions
    {
        public static Vector3 SetIndex(this Vector3 vector, int index, float value)
        {
            switch (index)
            {
                case 0:
                    return new Vector3(value, vector.Y, vector.Z);
                case 1:
                    return new Vector3(vector.X, value, vector.Z);
                case 2:
                    return new Vector3(vector.X, vector.Y, value);
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
