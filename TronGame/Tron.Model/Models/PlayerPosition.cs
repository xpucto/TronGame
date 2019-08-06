using Tron.Model.Models.Enums;

namespace Tron.Model.Models
{
    public class PlayerPosition
    {
        public PlayerPosition(int x, int y, Direction direction)
        {
            this.PositionX = x;
            this.PositionY = y;
            this.Direction = direction;
        }
        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public Direction Direction { get; set; }
    }
}
