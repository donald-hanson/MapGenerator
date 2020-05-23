using System;
using System.Collections.Generic;
using System.IO;

namespace MapGenerator.Q3
{
    public class Entity
    {
        private readonly List<Brush> _brushes = new List<Brush>();
        private readonly Dictionary<string, string> _keyValues = new Dictionary<string, string>();

        public IReadOnlyList<Brush> Brushes => _brushes.AsReadOnly();

        public void AddBrush(Brush b)
        {
            _brushes.Add(b);
        }

        public string this[string key]
        {
            get
            {
                _keyValues.TryGetValue(key, out var temp);
                return temp;
            }
            set => _keyValues[key] = value;
        }

        public static Entity Parse(Tokenizer tokenizer)
        {
            var entity = new Entity();

            var token = tokenizer.GetNextToken();
            while (token.Type != Tokenizer.TokenType.EndBlock)
            {
                switch (token.Type)
                {
                    case Tokenizer.TokenType.Value: // Key/value pair
                        var value = tokenizer.GetNextToken();
                        if (value.Type == Tokenizer.TokenType.Value)
                        {
                            entity[token.Contents] = value.Contents;
                        }
                        else
                        {
                            throw new FormatException(string.Format("Expected a value, received a {0}", value));
                        }

                        break;
                    case Tokenizer.TokenType.StartBlock: // Brush
                        Brush b = Brush.Parse(tokenizer);
                        entity.AddBrush(b);
                        break;
                    default:
                        throw new FormatException(string.Format("Expected either a block start or a value, received a {0}", token));
                }

                token = tokenizer.GetNextToken();
            }

            return entity;
        }

        public void WriteTo(TextWriter writer)
        {
            writer.WriteLine(Tokenizer.Token.StartBlock.Contents);
            
            foreach (var kvp in _keyValues)
            {
                writer.WriteLine("\"{0}\" \"{1}\"", kvp.Key, kvp.Value);
            }

            foreach (var brush in _brushes)
            {
                brush.WriteTo(writer);
            }

            writer.WriteLine(Tokenizer.Token.EndBlock.Contents);
        }
    }
}
