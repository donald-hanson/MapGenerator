using System.Threading.Tasks;
using MapGenerator.Wang;

namespace MapGenerator.Render
{
    public abstract class BaseRenderer<T, TOptions>
    {
        private readonly IWangMap<T> _map;
        private readonly int _width;
        private readonly int _height;

        protected int Width => _width;
        protected int Height => _height;
        protected IWangMap<T> Map => _map;

        protected BaseRenderer(IWangMap<T> map, in int width, in int height)
        {
            _map = map;
            _width = width;
            _height = height;
        }

        public abstract Task Render(TOptions options);
    }
}
