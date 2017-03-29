using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNA_TD
{
    public class Player
    {
        private string newTowerType;

        private int money = 50;
        private int lives = 30;
        private int cellX;
        private int cellY;
        private int tileX;
        private int tileY;

        private List<Tower> towers = new List<Tower>();

        private MouseState mouseState;
        private MouseState oldState;

        private Texture2D[] towerTexture;
        private Texture2D bulletTexture;

        private Level level;

        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public string NewTowerType
        {
            set { newTowerType = value; }
        }

        public Player(Level level, Texture2D[] towerTexture, Texture2D bulletTexture)
        {
            this.level = level;
            this.towerTexture = towerTexture;
            this.bulletTexture = bulletTexture;
        }

        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            mouseState = Mouse.GetState();

            cellX = (int)(mouseState.X / 32);
            cellY = (int)(mouseState.Y / 32);

            tileX = cellX * 32;
            tileY = cellY * 32;

            if (mouseState.LeftButton == ButtonState.Released && 
                oldState.LeftButton == ButtonState.Pressed)
            {
                if (string.IsNullOrEmpty(newTowerType) == false)
                {
                    AddTower();
                }
            }

            foreach (Tower tower in towers)
            {
                if (tower.HasTarget == false)
                {
                    tower.GetClosestEnemy(enemies);
                }

                tower.Update(gameTime);
            }

            oldState = mouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tower tower in towers)
            {
                tower.Draw(spriteBatch);
            }
        }

        private bool IsCellClear()
        {
            bool inBounds = cellX >= 0 && cellY >= 0 && 
                cellX < level.Width && cellY < level.Height;

            bool spaceClear = true;

            foreach (Tower tower in towers)
            {
                spaceClear = (tower.Position != new Vector2(tileX, tileY));

                if (!spaceClear)
                    break;
            }

            bool onPath = (level.GetIndex(cellX, cellY) != 1);

            return inBounds && spaceClear && onPath;
        }

        public void AddTower()
        {
            Tower towerToAdd = null;

            switch (newTowerType)
            {
                case "Arrow Tower":
                    {
                        towerToAdd = new TowerArrow(towerTexture[0], bulletTexture, 
                            new Vector2(tileX, tileY));
                        break;
                    }
                case "Spike Tower":
                    {
                        towerToAdd = new TowerSpike(towerTexture[1], bulletTexture, 
                            new Vector2(tileX, tileY));
                        break;
                    }
            }

            if (IsCellClear() == true && towerToAdd.Cost <= money)
            {
                towers.Add(towerToAdd);
                money -= towerToAdd.Cost;

                newTowerType = string.Empty;
            }
        }
    }
}
