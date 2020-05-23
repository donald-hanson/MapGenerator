using System.Collections.Generic;
using System.IO;

namespace MapGenerator.Q3
{
    public class Map
    {
        private readonly List<Entity> _entities = new List<Entity>();

        public IReadOnlyList<Entity> Entities => _entities.AsReadOnly();

        public void AddEntity(Entity e)
        {
            _entities.Add(e);
        }

        public static Map Parse(Tokenizer tokenizer)
        {
            var map = new Map();

            while (tokenizer.PeekNextToken().Type != Tokenizer.TokenType.EndOfStream)
            {
                var token = tokenizer.GetNextToken();
                if (token.Type == Tokenizer.TokenType.StartBlock)
                {
                    Entity e = Entity.Parse(tokenizer);
                    map.AddEntity(e);
                }
            }

            return map;
        }

        public void WriteTo(TextWriter writer)
        {
            foreach (var e in _entities)
            {
                e.WriteTo(writer);
            }
        }
    }
}
