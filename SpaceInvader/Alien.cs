using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvader
{
    class alien
    {
        public Texture2D sprite { get; set; } // sprite texture, read-only property
        public Vector2 position { get; set; } // sprite position on screen
        public Vector2 size { get; set; } // sprite size in pixels
        public Vector2 velocity { get; set; } // sprite velocity
        private Vector2 screenSize { get; set; } // screen size
        public Vector2 center { get { return size / 2; } } // sprite center
        public Vector2 direction { get; set; }
        public float changeDirection = -10f;
        public bool alienAlive;
        public bool touchLeft = false;
        public bool touchRight = false;
        public alien(Texture2D newTexture, Vector2 newPosition, Vector2 newSize, int ScreenWidth,
          int ScreenHeight)
        {
            sprite = newTexture;
            position = newPosition;
            size = newSize;
            screenSize = new Vector2(ScreenWidth, ScreenHeight);
            alienAlive = true;
        }
        //Detect bound
     
        public void Move()
        {
            //Touch left
            if (position.X + size.X <= 0)
            {
                touchLeft = true;
                touchRight = false;
            }
            //Touch right
            else if (position.X >= screenSize.X + 20f)
            {
                touchLeft = false;
                touchRight = true;
            }
            //Just for start the game
            if (!touchLeft && !touchRight)
                direction = new Vector2(changeDirection, 0f);
            //Touch left border
            else if (touchLeft)
            {
                direction = new Vector2(10f, 30f);
                touchLeft = false;
                changeDirection = -changeDirection;
            }
            //Touch right border
            else if (touchRight)
            {
                direction = new Vector2(-10f, 30f);
                touchRight = false;
                changeDirection = -changeDirection;
            }
            velocity = direction;

            position += velocity;
        }
        //Ship being destroied
        public bool ShipDestroy(Rectangle objectBall, Rectangle objectAlien)
        {
            if(objectBall.Intersects(objectAlien))
            {
                return true;
            }
            return false;
        }
        //Free fall 
        public void FreeFall()
        {
            velocity = new Vector2(10f, 10f);
            position += velocity;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Try rotation here
            //Center
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
