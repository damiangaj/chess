namespace ChessCore
{
    public class CountPieces
    {
        private readonly Dictionary<PieceVariant, int> wCounter = new();
        private readonly Dictionary<PieceVariant, int> bCounter = new();

        public int Counter { get; private set; }

        public CountPieces() 
        {
            foreach (PieceVariant variant in Enum.GetValues(typeof(PieceVariant)))
            {
                wCounter[variant] = 0;
                bCounter[variant] = 0;
            }
        }

        public void Add(Player player, PieceVariant variant)
        {
            if (player == Player.White)
            {
                wCounter[variant]++;
            }else if (player == Player.Black)
            {
                bCounter[variant]++;
            }
            Counter++;
        }

        public int NumberOfWhite(PieceVariant variant)
        {
            return wCounter[variant];
        }

        public int NumberOfBlack(PieceVariant variant)
        {
            return bCounter[variant];
        }
    }
}
