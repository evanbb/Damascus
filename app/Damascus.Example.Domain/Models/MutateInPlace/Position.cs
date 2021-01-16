using Damascus.Core;

namespace Damascus.Example.Domain
{
    public class Position
    {
        public Position(int nonZeroIndex)
        {
            nonZeroIndex.BetterBe(i => i > 0, "Position index must be greater than zero");
            NonZeroIndex = nonZeroIndex;
        }

        public int NonZeroIndex { get; }
    }
}
