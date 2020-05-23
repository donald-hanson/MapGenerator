using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MapGenerator.Wang;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MapGenerator.Render
{
    public enum BitmapTheme
    {
        Bridge,
        Commune,
        Dungeon,
        Islands,
        Trench,
        Wang
    }

    public class BitmapRenderer : BaseRenderer<WangBlobTile>
    {
        private readonly BitmapTheme _theme;

        public BitmapRenderer(IWangMap<WangBlobTile> map, in int width, in int height, BitmapTheme theme) : base(map, in width, in height)
        {
            _theme = theme;
        }

        public override async Task Render()
        {
            using (var img = new Image<Rgba32>(Configuration.Default, 32 * Width, 32 * Height))
            {
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        var tile = Map.GetTileAt(x, y);
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

                using (var outputStream = new FileStream(GetThemeName() + ".png", FileMode.Create))
                    img.SaveAsPng(outputStream);
            }
        }

        private async Task<byte[]> GetTileBytes(int id)
        {
            var themeName = GetThemeName();

            var baseDir = string.Format(@"Assets\bmp\blob\{0}\", themeName);
            Directory.CreateDirectory(baseDir);

            var file = string.Format(@"{0}{1}.gif", baseDir, id);
            if (File.Exists(file))
            {
                return await File.ReadAllBytesAsync(file);
            }
            
            var url = string.Format("http://www.cr31.co.uk/stagecast/art/blob/{0}/{1}.gif", themeName, id);
            Console.WriteLine("Downloading tile from {0}", url);
            var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(file, bytes);
            return bytes;
        }

        private string GetThemeName()
        {
            return _theme switch
            {
                BitmapTheme.Bridge => "bridge",
                BitmapTheme.Commune => "commune",
                BitmapTheme.Dungeon => "dungeon",
                BitmapTheme.Islands => "islands",
                BitmapTheme.Trench => "trench",
                BitmapTheme.Wang => "wang-2e2c",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}