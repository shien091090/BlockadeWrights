namespace GameCore
{
    public struct InGameMapCell
    {
        public InGameMapCell(int x, int y)
        {
            GridPosition = new IntVector2(x, y);
            IsEmpty = x < 0 || y < 0;
        }

        public bool IsEmpty { get; }
        public IntVector2 GridPosition { get; }

        public static InGameMapCell GetEmptyCell()
        {
            return new InGameMapCell(-1, -1);
        }
    }
}