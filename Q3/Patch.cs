using System;
using System.IO;
using System.Numerics;

namespace MapGenerator.Q3
{
    public class Patch
    {
        public string Texture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PatchDrawVertex[,] Vertexes { get; set; }

        public Patch(string texture, int width, int height, PatchDrawVertex[,] vertexes)
        {
            Texture = texture;
            Width = width;
            Height = height;
            Vertexes = vertexes;
        }
        
        public static Patch Parse(Tokenizer tokenizer)
        {
            var type = tokenizer.GetNextValue();
            if (type != "patchDef2")
            {
                throw new FormatException(string.Format("Expected a patchDef2, received a {0}", type));
            }

            tokenizer.GetNextType(Tokenizer.TokenType.StartBlock);

            var textureName = tokenizer.GetNextValue();

            tokenizer.GetNextType(Tokenizer.TokenType.StartParen);
            int width = int.Parse(tokenizer.GetNextValue());
            int height = int.Parse(tokenizer.GetNextValue());
            tokenizer.GetNextValue(); // ignored
            tokenizer.GetNextValue(); // ignored
            tokenizer.GetNextValue(); // ignored
            tokenizer.GetNextType(Tokenizer.TokenType.EndParen);

            PatchDrawVertex[,] vertexes = new PatchDrawVertex[width,height];

            tokenizer.GetNextType(Tokenizer.TokenType.StartParen);
            for (var i = 0; i < width; i++)
            {
                tokenizer.GetNextType(Tokenizer.TokenType.StartParen);
                for (var j = 0; j < height; j++)
                {
                    vertexes[i, j] = PatchDrawVertex.Parse(tokenizer);
                }
                tokenizer.GetNextType(Tokenizer.TokenType.EndParen);
            }
            tokenizer.GetNextType(Tokenizer.TokenType.EndParen);

            tokenizer.GetNextType(Tokenizer.TokenType.EndBlock);

            return new Patch(textureName, width, height, vertexes);
        }

        public void WriteTo(TextWriter writer)
        {
            writer.WriteLine("patchDef2");
            writer.WriteLine(Tokenizer.Token.StartBlock.Contents);
            writer.WriteLine(Texture);
            writer.Write(Tokenizer.Token.StartParen.Contents);
            writer.Write(" ");
            writer.Write(Width);
            writer.Write(" ");
            writer.Write(Height);
            writer.Write(" ");
            writer.Write(0);
            writer.Write(" ");
            writer.Write(0);
            writer.Write(" ");
            writer.Write(0);
            writer.Write(" ");
            writer.Write(Tokenizer.Token.EndParen.Contents);
            writer.WriteLine();

            writer.WriteLine(Tokenizer.Token.StartParen.Contents);
            for (var i = 0; i < Width; i++)
            {
                writer.Write(Tokenizer.Token.StartParen.Contents);
                writer.Write(" ");
                for (var j = 0; j < Height; j++)
                {
                    Vertexes[i, j].WriteTo(writer);
                    writer.Write(" ");
                }
                writer.WriteLine(Tokenizer.Token.EndParen.Contents);
            }
            writer.WriteLine(Tokenizer.Token.EndParen.Contents);

            writer.WriteLine(Tokenizer.Token.EndBlock.Contents);

        }

        public void RotateOrigin(Vector3 vRotation, Vector3 vOrigin)
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    Vertexes[i, j].RotateOrigin(vRotation, vOrigin);
                }
            }
        }

        public void Move(float x, float y, float z)
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    Vertexes[i,j].Move(x, y, z);
                }
            }
        }
    }
}