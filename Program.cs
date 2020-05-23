using System;
using System.Threading.Tasks;
using MapGenerator.Render;

namespace MapGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var w = 16;
            var h = 16;
            
            var m = new Wang.WangMazeMap(w, h, 5, 0, true);
            m.Generate();


            var themes = new[]
            {
                BitmapTheme.Wang,
                BitmapTheme.Bridge, 
                BitmapTheme.Commune,
                BitmapTheme.Dungeon, 
                BitmapTheme.Islands,
                BitmapTheme.Trench,
            };

            foreach (var t in themes)
            {
                var b = new BitmapRenderer(m, w, h, t);
                await b.Render();
            }
            
            var q = new Quake3Renderer(m,w,h);
            await q.Render();

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
