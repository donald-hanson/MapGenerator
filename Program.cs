using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using MapGenerator.Q3;
using MapGenerator.Wang;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MapGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var w = 8;
            var h = 8;
            var m = new Wang.WangMazeMap(w, h, 5);
            m.Generate();

            RenderConsole(m, w, h);
            await RenderBitmap(m, w, h);
            var q3map = await RenderQuake3(m, w, h);

            using (var writer = new StringWriter())
            {
                q3map.WriteTo(writer);
                var result = writer.ToString();

                File.WriteAllText(@"C:\quake3\baseq3\maps\wang.map", result);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static async Task<Q3.Map> RenderQuake3(IWangMap<WangBlobTile> m, int w, int h)
        {
            var worldspawn = new Q3.Entity();
            worldspawn["classname"] = "worldspawn";

            float step = 256.0f;
            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var tile = m.GetTileAt(x, y);
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

        private static class Quake3TileMap
        {
            private static readonly IDictionary<int, int[]> TileMap = new Dictionary<int, int[]>
            {
                {0, new[] {0, 0}},

                {1, new[] {1, 0}},
                {4, new[] {1, -90}},
                {16, new[] {1, -180}},
                {64, new[] {1, -270}},

                {5, new[] {5, 0}},
                {20, new[] {5, -90}},
                {80, new[] {5, -180}},
                {65, new[] {5, -270}},

                {7, new[] {7, 0}},
                {28, new[] {7, -90}},
                {112, new[] {7, -180}},
                {193, new[] {7, -270}},

                {17, new[] {17, 0}},
                {68, new[] {17, -90}},

                {21, new[] {21, 0}},
                {84, new[] {21, -90}},
                {81, new[] {21, -180}},
                {69, new[] {21, -270}},

                {23, new[] {23, 0}},
                {92, new[] {23, -90}},
                {113, new[] {23, -180}},
                {197, new[] {23, -270}},

                {29, new[] {29, 0}},
                {116, new[] {29, -90}},
                {209, new[] {29, -180}},
                {71, new[] {29, -270}},

                {31, new[] {31, 0}},
                {124, new[] {31, -90}},
                {241, new[] {31, -180}},
                {199, new[] {31, -270}},

                {85, new[] {85, 0}},

                {87, new[] {87, 0}},
                {93, new[] {87, -90}},
                {117, new[] {87, -180}},
                {213, new[] {87, -270}},

                {95, new[] {95, 0}},
                {125, new[] {95, -90}},
                {245, new[] {95, -180}},
                {215, new[] {95, -270}},

                {119, new[] {119, 0}},
                {221, new[] {119, -90}},

                {127, new[] {127, 0}},
                {253, new[] {127, -90}},
                {247, new[] {127, -180}},
                {223, new[] {127, -270}},

                {255, new[] {255, 0}},
            };

            public static async Task<Map> GetTileMap(int id)
            {
                if (id == 0)
                {
                    return null;
                }

                if (!TileMap.TryGetValue(id, out var rules))
                {
                    return null;
                }

                return await LoadMap(rules[0], rules[1]);
            }

            private static async Task<Map> LoadMap(int id, int rotation = 0)
            {
                var fileName = string.Format(@"C:\quake3\baseq3\maps\tile_{0}.map", id);
                var text = await File.ReadAllTextAsync(fileName);
                var m = Map.Parse(new Tokenizer(text));

                if (rotation != 0)
                {
                    Vector3 vRotation = new Vector3(0, 0, rotation);
                    Vector3 vOrigin = new Vector3();

                    foreach (var entity in m.Entities)
                    {
                        foreach (var brush in entity.Brushes)
                        {
                            foreach (var face in brush.Faces)
                            {
                                face.RotateOrigin(vRotation, vOrigin);
                            }
                        }
                    }
                }

                return m;
            }
        }
        
        private static void RenderConsole(IWangMap<WangBlobTile> m, int w, int h)
        {
            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var tile = m.GetTileAt(x, y);
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

        private static async Task RenderBitmap(IWangMap<WangBlobTile> m, int w, int h)
        {
            // http://www.cr31.co.uk/stagecast/art/blob/wang-2e2c/0.gif
            using (var img = new Image<Rgba32>(Configuration.Default, 32 * w, 32 * h))
            {
                for (var x = 0; x < w; x++)
                {
                    for (var y = 0; y < h; y++)
                    {
                        var tile = m.GetTileAt(x, y);
                        var id = tile.Index;

                        var left = x * 32;
                        var top = y * 32;

                        var tileBytes = await GetTileBytes(id);
                        using (var tileImage = Image.Load(tileBytes))
                        {
                            var location = new Point(left, top);
                            img.Mutate(z => { z.DrawImage(tileImage, location, 1); });
                        }
                    }
                }

                using (var outputStream = new FileStream("output.png", FileMode.Create))
                    img.SaveAsPng(outputStream);
            }
        }

        private static async Task<byte[]> GetTileBytes(int id)
        {
            var file = string.Format("{0}.gif", id);
            if (File.Exists(file))
            {
                return await File.ReadAllBytesAsync(file);
            }

            var url = string.Format("http://www.cr31.co.uk/stagecast/art/blob/wang-2e2c/{0}.gif", id);
            var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(file, bytes);
            return bytes;
        }
    }
}
