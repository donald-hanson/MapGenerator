using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MapGenerator.Q3;
using MapGenerator.Wang;

namespace MapGenerator.Render
{
    public class Quake3Renderer : BaseRenderer<WangBlobTile>
    {
        public Quake3Renderer(IWangMap<WangBlobTile> map, in int width, in int height) : base(map, in width, in height)
        {
        }

        public override async Task Render()
        {
            var m = await CreateWorld();

            await using var writer = new StringWriter();
            m.WriteTo(writer);
            var result = writer.ToString();

            await File.WriteAllTextAsync(@"wang.map", result);
        }

        private async Task<Q3.Map> CreateWorld()
        {
            var worldspawn = new Entity();
            worldspawn["classname"] = "worldspawn";

            float step = 256.0f;
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var tile = Map.GetTileAt(x, y);
                    var id = tile.Index;

                    var tileMap = await Quake3TileMap.GetTileMap(id);

                    var tileWorldspawn = tileMap?.Entities.FirstOrDefault(e => e["classname"] == "worldspawn");
                    if (tileWorldspawn == null)
                    {
                        continue;
                    }

                    foreach (var brush in tileWorldspawn.Brushes)
                    {
                        foreach (var face in brush.Faces)
                        {
                            face.Move(x * step, y * -1 * step, 0);
                        }

                        worldspawn.AddBrush(brush);
                    }
                }
            }

            var map = new Map();
            map.AddEntity(worldspawn);
            return map;
        }
    }
}