using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_TD
{
    public class Enemy : Entity
    {
        protected float startHealth;
        protected float currentHealth;
        protected bool alive = true;
        protected float speed = 0.5f;
        protected int bountyGiven;
        private Queue<Vector2> waypoints = new Queue<Vector2>();

        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public bool IsDead
        {
            get { return !alive; }
        }

        public int BountyGiven
        {
            get { return bountyGiven; }
        }

        public void SetWaypoints(Queue<Vector2> waypoints)
        {
            foreach (Vector2 waypoint in waypoints)
            {
                this.waypoints.Enqueue(waypoint);
            }

            this.position = this.waypoints.Dequeue();
        }

        public float DistanceToDestination
        {
            get { return Vector2.Distance(position, waypoints.Peek()); }
        }

        public Enemy(Texture2D texture, Vector2 position, float health, int bountyGiven, float speed)
            : base(texture, position)
        {
            this.startHealth = health;
            this.currentHealth = startHealth;
            this.bountyGiven = bountyGiven;
            this.speed = speed;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentHealth <= 0)
                alive = false;

            if (waypoints.Count > 0)
            {
                if (DistanceToDestination < speed)
                {
                    position = waypoints.Peek();
                    waypoints.Dequeue();
                }
                else
                {
                    Vector2 direction = waypoints.Peek() - position;
                    direction.Normalize();

                    velocity = Vector2.Multiply(direction, speed);

                    position += velocity;
                }
            }
            else
            {
                alive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                float healthPercentage = (float)currentHealth / (float)startHealth;
                Color colour = new Color(new Vector3(1 - healthPercentage, 
                    1 - healthPercentage, 
                    1 - healthPercentage));
                base.Draw(spriteBatch, colour);
            }
        }
    }
}


// Created by: Howtech