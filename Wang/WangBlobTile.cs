namespace MapGenerator.Wang
{
    public class WangBlobTile : WangTile
    {
        public static readonly WangBlobTile Null = new WangBlobTile(-1, true);

        public WangBlobTile() : this(0, false)
        {

        }

        public WangBlobTile(int index, bool readOnly)
        {
            Index = index;
            ReadOnly = readOnly;
        }

        public bool IsNull => Index == -1;
        
        public int Index { get; private set; }
        public bool ReadOnly { get; }
        
        public bool NorthWest
        {
            get => HasFlag(128);
            set => SetFlag(128, value);
        }

        public bool North
        {
            get => HasFlag(1);
            set => SetFlag(1, value);
        }

        public bool NorthEast
        {
            get => HasFlag(2);
            set => SetFlag(2, value);
        }

        public bool East
        {
            get => HasFlag(4);
            set => SetFlag(4, value);
        }

        public bool SouthEast
        {
            get => HasFlag(8);
            set => SetFlag(8, value);
        }

        public bool South
        {
            get => HasFlag(16);
            set => SetFlag(16, value);
        }

        public bool SouthWest
        {
            get => HasFlag(32);
            set => SetFlag(32, value);
        }

        public bool West
        {
            get => HasFlag(64);
            set => SetFlag(64, value);
        }

        private bool HasFlag(int value)
        {
            return (Index & value) == value;
        }

        private void SetFlag(int value, bool set)
        {
            if (IsNull || ReadOnly)
            {
                return;
            }

            if (set)
            {
                Index |= value;
            }
            else
            {
                Index ^= value;
            }
        }

        public override string ToString()
        {
            return string.Format("Index: {0} IsNull: {1} ReadOnly: {2}", Index, IsNull, ReadOnly);
        }
    }
}