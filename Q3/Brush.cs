using System.Collections.Generic;
using System.IO;

namespace MapGenerator.Q3
{
    public class Brush
    {
        private readonly List<Face> _faces = new List<Face>();

        public IReadOnlyList<Face> Faces => _faces.AsReadOnly();

        public void AddFace(Face f)
        {
            _faces.Add(f);
        }

        public static Brush Parse(Tokenizer tokenizer)
        {
            var brush = new Brush();
            while (tokenizer.PeekNextToken().Type != Tokenizer.TokenType.EndBlock)
            {
                var face = Face.Parse(tokenizer);
                brush.AddFace(face);
            }

            tokenizer.GetNextToken(); // Brush end block

            return brush;
        }

        public void WriteTo(TextWriter writer)
        {
            writer.WriteLine(Tokenizer.Token.StartBlock.Contents);

            foreach (var face in _faces)
            {
                face.WriteTo(writer);
            }

            writer.WriteLine(Tokenizer.Token.EndBlock.Contents);
        }
    }
}
