using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_TD
{
    public class TowerSpike : Tower
    {
        private Vector2[] directions = new Vector2[8];
        private List<Enemy> targets = new List<Enemy>();

        public override bool HasTarget
        {
            get { return false; }
        }

        public TowerSpike(Texture2D texture, Texture2D bulletTexture, Vector2 position)
            : base(texture, bulletTexture, position)
        {
            this.damage = 20;
            this.cost = 40;
            this.radius = 48;

            directions = new Vector2[]
            {
                new Vector2(-1, -1), // North West
                new Vector2( 0, -1), // North
                new Vector2( 1, -1), // North East
                new Vector2(-1,  0), // West
                new Vector2( 1,  0), // East
                new Vector2(-1,  1), // South West
                new Vector2( 0,  1), // South
                new Vector2( 1,  1), // South East
            };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (bulletTimer >= 1.0f && targets.Count != 0)
            {
                for (int i = 0; i < directions.Length; i++)
                {
                    Bullet bullet = new Bullet(bulletTexture, Vector2.Subtract(center, 
                        new Vector2(bulletTexture.Width / 2)), directions[i], 6, damage);

                    bulletList.Add(bullet);
                }

                bulletTimer = 0;
            }

            for(int i = 0; i < bulletList.Count; i++)
            {
                Bullet bullet = bulletList[i];
                bullet.Update(gameTime);

                if (!IsInRange(bullet.Center))
                {
                    bullet.Kill();
                }

                for (int t = 0; t < targets.Count; t++)
                {
                    if (targets[t] != null && Vector2.Distance(bullet.Center, targets[t].Center) < 12)
                    {
                        targets[t].CurrentHealth -= bullet.Damage;
                        bullet.Kill();

                        break;
                    }
                }

                if (bullet.IsDead())
                {
                    bulletList.Remove(bullet);
                    i--;
                }
            }
        }

        public override void GetClosestEnemy(List<Enemy> enemies)
        {
            targets.Clear();

            foreach (Enemy enemy in enemies)
            {
                if (IsInRange(enemy.Center))
                {
                    targets.Add(enemy);
                }
            }
        }
    }
}
