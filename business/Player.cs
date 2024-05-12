using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using MG_SplaTank.states;
using MG_SplaTank.view.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MG_SplaTank.business
{
    public class Player
    {
		#region variables
        public string Name { get; set; }
        public bool IsPlayer1;
        public int Points { get; set; }
        public int Record { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesLost { get; set; }
        [JsonIgnore]
        public Tank Tank { get; set; }
        [JsonIgnore]
        public Player Enemy { get; set; }
        public static int OffsetCounter { get; set; }
        public List<Bullet> Bullets;
		private readonly Keys upKey, downKey, leftKey, rightKey, boostKey, shootKey;
        private int shootingCounter;
		#endregion
        public Player() { }
        public Player(string name, bool playerType)
        {
            Name = name;
            IsPlayer1 = playerType;
            Tank = new Tank(playerType);
            Bullets = new List<Bullet>();

			if (IsPlayer1)
			{
				upKey = Keys.W;
				downKey = Keys.S;
				leftKey = Keys.A;
				rightKey = Keys.D;
				boostKey = Keys.LeftShift;
				shootKey = Keys.Space;
			}
			else
			{
				upKey = Keys.Up;
				downKey = Keys.Down;
				leftKey = Keys.Left;
				rightKey = Keys.Right;
				boostKey = Keys.RightShift;
				shootKey = Keys.Enter;
			}
        }

		public void MoveTank()
		{
			KeyboardState keyState = Keyboard.GetState();

			if (keyState.IsKeyDown(leftKey)) { Tank.Rotation -= Tank.DELTA_ROTATION; }
			else if (keyState.IsKeyDown(rightKey)) { Tank.Rotation += Tank.DELTA_ROTATION; }

			Tank.MaxVel = keyState.IsKeyDown(boostKey) ?
				Tank.MAX_VELOCITY : Tank.STD_VELOCITY;

			if (keyState.IsKeyDown(upKey))
			{
				Tank.Accelerate();
				Tank.IsMovinForward = true;
			}
			else if (keyState.IsKeyDown(downKey))
			{
				Tank.Accelerate();
				Tank.IsMovinForward = false;
			}
			else
			{
				Tank.Decelerate();
			}

			if (Tank.IsColliding())
			{
				Tank.Decelerate(3.5f);
			}

			if (Tank.Velocity > 0)
			{
				Tank.Position = Tank.Move();
			}

			if (keyState.IsKeyDown(shootKey)) { Shoot(); }
		}

		private void Shoot()
        {
			const int SHOOTING_RATE = 8;
            if (shootingCounter < 0)
            {
                Bullets.Add(new Bullet(this));
                shootingCounter = SHOOTING_RATE;
            }
        }

        public void MoveBullets()
        {
            shootingCounter--;
            for (int i = 0; i < Bullets.Count(); i++)
            {
                float distanceTravelled = Vector2.Distance(Bullets[i].OriginPosition, Bullets[i].Position);

                if (distanceTravelled < Bullet.MAX_DISTANCE && !Bullets[i].IsColliding())
                {
                    Bullets[i].Position = Bullets[i].Move();
                }
                else
                {
                    PaintCells(Bullets[i]);
                    Bullets.RemoveAt(i);
                    i--;
                }
            }
        }

		public void PaintCells(Bullet bullet)
		{
			// Obtenemos, en base al vector de la bala (último punto antes de
			// ser eliminada), el área que debemos pintar
			Vector2[] area = GetPaintingArea(bullet.Position);

			// Pintamos las celdas acorde a un radio de 1 celda
			for (int i = (int)area[0].X; i <= (int)area[1].X; i++)
			{
				for (int j = (int)area[0].Y; j <= (int)area[1].Y; j++)
				{
					Match.CellsGrid[i, j].SetColor(this);
				}
			}
		}

		public Vector2[] GetPaintingArea(Vector2 dropPosition)
		{
			Vector2[] area = new Vector2[2];

			Vector2 epicenter = GetEpicenter(dropPosition);
			int radius = 1;

			int[] boundaryX = CalculateBoundary(
				(int)epicenter.X, radius, Match.CellsGrid.GetLength(0));

			int[] boundaryY = CalculateBoundary(
				(int)epicenter.Y, radius, Match.CellsGrid.GetLength(1));

			area[0] = new Vector2(boundaryX[0], boundaryY[0]);
			area[1] = new Vector2(boundaryX[1], boundaryY[1]);

			return area;
		}

		private int[] CalculateBoundary(int position, int radius, int maxPosition)
		{
			int[] boundary = new int[2];

			if (position < radius) { boundary[0] = 0; }
			else { boundary[0] = position - radius; }

			if (position > maxPosition - radius - 1) 
			{
				boundary[1] = maxPosition - 1;
			}
			else { boundary[1] = position + radius; }

			return boundary;
		}

		public Vector2 GetEpicenter(Vector2 dropPosition)
        {
            for (int i = 0; i < Match.CellsGrid.GetLength(0); i++)
            {
                for (int j = 0; j < Match.CellsGrid.GetLength(1); j++)
                {
                    if (Match.CellsGrid[i, j].HitBox.Contains(dropPosition))
                    {
                        return new Vector2(i, j);
                    }
                }
            }
            return new Vector2(0, 0);
		}
	}
}
