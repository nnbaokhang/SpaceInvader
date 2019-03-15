using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvader
{
    class cannon
    {
        public Texture2D sprite;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public Vector2 velocity;
        public bool cannonAlive;
       
        public cannon(Texture2D loadedTexture,Vector2 newPosition)
        {
            rotation = 0.0f;
            position = newPosition;
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            velocity = Vector2.Zero;
            cannonAlive = true;
        }

     

        public void Rotate()
        {
            //Create matrix

            KeyboardState keyboardState = Keyboard.GetState();
            //Rotate left
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                rotation -= 0.1f;
            }
            //Rotate to right   
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                rotation += 0.1f;
            }
        }
       
        public void FireBall(ball objectBall)
        {

            if (!objectBall.ballAlive)
            {
                //Set ball alive
                objectBall.ballAlive = true;
                //This allows the cannonball to be shot from the middle of the cannon.    
                objectBall.position = position - objectBall.center;
                //Set velocity and direction of the ball  
                objectBall.velocity = new Vector2(
                          (float)Math.Cos(rotation),
                          (float)Math.Sin(rotation)) * 10f;
            }
            //Move ball when alive
            if (objectBall.ballAlive)
            {
                objectBall.Move(objectBall.velocity);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Try rotation here
            //Center
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, center, 1.0f, SpriteEffects.None,0);
        }
        
    }

}