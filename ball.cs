using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvader
{
    class ball
    {
        public Texture2D texture { get; set; } // sprite texture, read-only property
        public Vector2 position { get; set; } // sprite position on screen
        public Vector2 size { get; set; } // sprite size in pixels
        public Vector2 velocity { get; set; } // sprite velocity
        private Vector2 screenSize { get; set; } // screen size
        public Vector2 center { get { return size/2; } } // sprite center
        public float radius { get { return size.X / 2; } } // sprite radius
        public bool ballAlive;
      

        public void Move(Vector2 direction)
        {
            // since we adjusted the velocity, just add it to the current position

                velocity = direction;
                //New position of the ball
                position += velocity;
            
            //*dt + new Vector2(0.2f * dt* dt/2, 0.2f * dt * dt / 2);
        }
        public bool detectOutSide()
        {
            if(this.position.Y + 20f <= 0 || this.position.X >= screenSize.X + 20f) 
            {
                return true;
            }
            return false;
        }

        public ball(Texture2D newTexture, Vector2 newPosition, Vector2 newSize, int ScreenWidth,
          int ScreenHeight)
        {
            texture = newTexture;
            position = newPosition;
            size = newSize;
            screenSize = new Vector2(ScreenWidth, ScreenHeight);
            ballAlive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
       
    }
}
