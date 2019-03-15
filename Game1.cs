using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
namespace SpaceInvader
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D background;
        cannon cannon;
        const int maxAlien = 10;
        const int maxBullet = 3;
        alien[] AlienArmy = new alien[maxAlien];
        ball[] Bullet = new ball[maxBullet];
        //Keyboard state
        KeyboardState keyboardState;
        KeyboardState previousKeyboardState ;
        //Sound effect
        SoundEffect missleLaunch;
        SoundEffect explosion;
        //For position equation
        float dt = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //Initialize cannon . Need 5 of this.
          
            background = Content.Load<Texture2D>("background");
          
            //smallBall.velocity = new Vector2(10.1f, 10.1f);
            //Draw cannon
            cannon = new cannon(Content.Load<Texture2D>("cannon"), 
                new Vector2(graphics.GraphicsDevice.Viewport.Width/10, graphics.GraphicsDevice.Viewport.Height/1.15f));
            //Draw small ball
            for (int i = 0; i < maxBullet; i++)
            {
                Bullet[i] = new ball(Content.Load<Texture2D>("cannonball"),
                new Vector2(cannon.position.X, cannon.position.Y - 20f), new Vector2(54f, 54f),
                 graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            }
            //Draw Alien
            float height = 0f;
            int column = 0;
            for (int i = 0; i < maxAlien; i++)
            {

     
                AlienArmy[i] = new alien(Content.Load<Texture2D>("enemy"),
                new Vector2(graphics.PreferredBackBufferWidth / 2 - 150*(column+1), graphics.PreferredBackBufferHeight / 5 + height), new Vector2(121f, 40f),
                 graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                Console.WriteLine("{0},{1}", graphics.PreferredBackBufferWidth / 2 - 150 * (column + 1),
                    graphics.PreferredBackBufferHeight / 5 + height);
                //Set up grid for alien ship
                if (i >= 4 && column >= 4)
                {
                    column = 0;
                    height = 80f;
                    continue;
                }
                column++;
                
            }
           //Load sound effect
           missleLaunch = Content.Load<SoundEffect>("missilelaunch");
           explosion    = Content.Load<SoundEffect>("explosion");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            cannon.sprite.Dispose();
            spriteBatch.Dispose();
        }
        //Fire from cannon
        public void CannonFire()
        {
            //Fire bullets from cannon

            foreach (ball smallBall in Bullet)
            {
                //Detect if ball fall outside range
                if (smallBall.detectOutSide())
                {

                    smallBall.ballAlive = false;
                    //Move ball back to cannon
                    smallBall.position = cannon.position - smallBall.center;
                }
                //Press key to fire ball
                if ((keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space) && !smallBall.ballAlive))
                {
                        missleLaunch.Play();
                        cannon.FireBall(smallBall);
                        break;
                    
                }
              
            }
        }
        //Ball that hit ship fire from cannon
        public void BallHitShip()
        {
            //Check if ball hit ship
            foreach (ball smallBall in Bullet)
            {
                //Create a invisible rectangle for cannon ball detect collision
                Rectangle cannonBallRect = new Rectangle(
                    (int)smallBall.position.X,
                    (int)smallBall.position.Y,
                    smallBall.texture.Width,
                    smallBall.texture.Height);
                //Alien ship destroied;
                foreach (alien smallAlien in AlienArmy)
                {
                    //Create a invisible rectangle for alien ship detect collision
                    Rectangle enemyRect = new Rectangle(
                                  (int)smallAlien.position.X,
                                  (int)smallAlien.position.Y,
                                  smallAlien.sprite.Width,
                                  smallAlien.sprite.Height);
                    //If ship is destroied
                    if (smallAlien.ShipDestroy(cannonBallRect, enemyRect))
                    {
                        explosion.Play();
                        smallAlien.alienAlive = false;
                        smallBall.ballAlive = false;
                        //reset position of ball
                        smallBall.position = cannon.position - smallBall.center;
                        break;
                    }
                }
            }

            //Free fall
            foreach (alien smallAlien in AlienArmy)
            {
                if (!smallAlien.alienAlive)
                    smallAlien.FreeFall();
            }
        }
        //Alien ship move
        public void AlienShipMove()
        {
            //Move alien ship
            foreach (alien smallAlien in AlienArmy)
            {
                //If ship still alive
                if (smallAlien.alienAlive)
                    smallAlien.Move();
            }
        }
        public void ballMove()
        {
            foreach (ball smallBall in Bullet)
            {
                //If ship still alive
                if (smallBall.ballAlive)
                    smallBall.Move(smallBall.velocity);
            }
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        protected override void Update(GameTime gameTime)
        {
           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //State of the game right here.
            IsMouseVisible = true;
            dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10f;
            keyboardState = Keyboard.GetState();

            //Rotate
            cannon.Rotate();
            //Restrict rotate
            cannon.rotation = MathHelper.Clamp(cannon.rotation, -MathHelper.PiOver2, 0);
            //Cannon fire
            CannonFire();
            //Ball move
            ballMove();
            //ALienShip move
            AlienShipMove();
            //Ball destroy ship
            BallHitShip();

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.    
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //Begin draw here
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.White);
            //Draw bullet
            for(int i = 0; i < maxBullet; i++)
            {
                Bullet[i].Draw(spriteBatch);
            }
            //Draw the alien
            for (int i = 0; i < maxAlien; i++)
            {
                AlienArmy[i].Draw(spriteBatch);
            }
            //Draw the cannon.
            spriteBatch.Draw(cannon.sprite,
                cannon.position,
                null,
                Color.White,
                cannon.rotation,
                cannon.center, 1.0f,
                SpriteEffects.None, 0);
            //End draw here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
