using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using MapGenerator.Q3;

namespace MapGenerator.Render
{
    internal static class Quake3TileMap
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
            var fileName = String.Format(@"Assets\Q3\tile_{0}.map", id);
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
}