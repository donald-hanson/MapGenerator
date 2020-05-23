using System;
using System.Threading.Tasks;
using MapGenerator.Wang;

namespace MapGenerator.Render
{
    public class ConsoleRenderer : BaseRenderer<WangBlobTile>
    {
        public ConsoleRenderer(IWangMap<WangBlobTile> map, in int width, in int height) : base(map, width, height)
        {
            
        }

        public override async Task Render()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var tile = Map.GetTileAt(x, y);
                    var id = tile.Index;

                    if (y > 0)
                    {
                        Console.Write(",");
                    }

                    Console.Write("{0:D3}", id);
                }

                Console.WriteLine();
            }
        }
    }
}