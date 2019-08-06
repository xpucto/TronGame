using System.Collections.Generic;
using Tron.Model.Models.Enums;

namespace Tron.Model.Models
{
    public class Player
    {
        public string Name { get; set; }

        public string ConnectionId { get; set; }

        public PlayerPosition Position { get; set; }

        public bool IsAlive { get; set; }

        public IList<PlayerPosition> PlayerPath { get; set; }

        public int Score { get; set; }

        public Player(string name)
        {
            this.IsAlive = true;
            this.Name = name;
        }

        public void SetPosition(PlayerPosition position)
        {
            this.Position = position;
            this.PlayerPath = new List<PlayerPosition>
            {
                new PlayerPosition(position.PositionX, position.PositionY, position.Direction)
            };
        }

        public void ChangeDirection(Direction keyCode)
        {
            if (keyCode == Direction.Up && this.Position.Direction != Direction.Down)
            {
                this.Position.Direction = Direction.Up;
            }
            else if (keyCode == Direction.Left && this.Position.Direction != Direction.Right)
            {
                this.Position.Direction = Direction.Left;
            }
            else if (keyCode == Direction.Right && this.Position.Direction != Direction.Left)
            {
                this.Position.Direction = Direction.Right;
            }
            else if (keyCode == Direction.Down && this.Position.Direction != Direction.Up)
            {
                this.Position.Direction = Direction.Down;
            }
        }

        public void Move()
        {
            switch (this.Position.Direction)
            {
                case Direction.Right:
                    this.Position.PositionX++;
                    break;
                case Direction.Left:
                    this.Position.PositionX--;
                    break;
                case Direction.Up:
                    this.Position.PositionY--;
                    break;
                case Direction.Down:
                    this.Position.PositionY++;
                    break;
            }

            this.PlayerPath.Add(new PlayerPosition(this.Position.PositionX, this.Position.PositionY, this.Position.Direction));
        }
    }
}
