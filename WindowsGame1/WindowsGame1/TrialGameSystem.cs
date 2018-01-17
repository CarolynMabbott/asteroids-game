using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;


namespace WindowsGame1
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TrialGameSystem : Microsoft.Xna.Framework.Game
    {
        const double SCREEN_SCALE = 2.0;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D m_RocketTexture;
        Texture2D m_rockTexture;

        const int requiredMoveTimeStepMS = 40;

        TrialGame theGame;

        public TrialGameSystem()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            theGame = new TrialGame();
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
            IsFixedTimeStep = true;
            // update 25 times sec (every 40 ms)
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, requiredMoveTimeStepMS);
            TargetElapsedTime = ts;
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
            m_RocketTexture = Content.Load<Texture2D>("Rocket2");
            m_RocketTexture.Tag = "Rocket";

            m_rockTexture = Content.Load<Texture2D>("rock");
            m_rockTexture.Tag = "rock";
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
         //   if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
         //       this.Exit();

            // TODO: Add your update logic here
            double elapsed = gameTime.ElapsedGameTime.Milliseconds;
            HandleKeyboardInput();
            theGame.MakeOneMove(elapsed);

            base.Update(gameTime);
        }


        /// <summary>
        /// Handle the input from the device                  
        /// At the moment this just exits the game if the 'Q' button is pressed
        /// </summary>
        /// <returns> bool true if the keyboard state is read else false</returns>
        private bool HandleKeyboardInput()
        {

#if WINDOWS
            try
            {
                KeyboardState ks = Keyboard.GetState();

                if (ks.IsKeyDown(Keys.Up))
                {
                    theGame.GetRocket().fireBothThrusters();
                    // Allows the Windows game to exit when Q is pressed
                    //QuitGame();
                }
                if (ks.IsKeyDown(Keys.Left))
                {
                    theGame.GetRocket().firePortThruster();
                    // Allows the Windows game to exit when Q is pressed
                    //QuitGame();
                }
                if (ks.IsKeyDown(Keys.Right))
                {
                    theGame.GetRocket().fireStarboardThruster();
                    // Allows the Windows game to exit when Q is pressed
                    //QuitGame();
                }
                if (ks.IsKeyDown(Keys.Q))
                {
                    // Allows the Windows game to exit when Q is pressed
                    //QuitGame();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
#else
            return false;
#endif

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            // TODO: Add your drawing code here

            Rocket rkt = theGame.GetRocket();
            Point2D lctn = rkt.GetRocketPosition();
            double headingRads = rkt.HeadingRads;

            DrawRocket((Int32)lctn.X, (Int32)lctn.Y, headingRads);



            rock rck = theGame.getRock();
            lctn = rck.GetRocketPosition();
            headingRads = rck.HeadingRads;

            DrawRock((Int32)lctn.X, (Int32)lctn.Y, headingRads);

            spriteBatch.End();
            base.Draw(gameTime);
        }


        /// <summary>
        /// draw the rocket texture on the screen centred at an x, y location
        /// </summary>
        /// <param name="x">the screen X location</param>
        /// <param name="cols">the screen X location</param>
        /// <returns>true if the cursor was drawn</returns>
        private bool DrawRocket(Int32 x, Int32 y, double hdgRads)
        {
            Rectangle sourceRect;
            Rectangle destRect;

            x = (int)(x * SCREEN_SCALE);
            y = (int)(y * SCREEN_SCALE);


            if (RestrictToScreen(m_RocketTexture, ref x, ref y) == false)
                return false;

            double hdgDegs = (hdgRads * 360.0)/GeneralMaths.TWO_PI;
            Debug.WriteLine("hdgDegs = " + hdgDegs.ToString());
            Debug.WriteLine("x = " + x.ToString());
            Debug.WriteLine("y = " + y.ToString());
            sourceRect = new Rectangle(0, 0, m_RocketTexture.Width, m_RocketTexture.Height);

            Point2D zeroPt;

            // REM the XNA texture rotates about the top left corner, so need to move the point to draw the texture so it rotates about the centre
            RotateTextureZeroPtAboutCentre(m_RocketTexture, hdgRads, out zeroPt);

            destRect = new Rectangle(x + (Int32)zeroPt.X, y + (Int32)zeroPt.Y , m_RocketTexture.Width, m_RocketTexture.Height);


            spriteBatch.Draw(m_RocketTexture, destRect, sourceRect, Color.White, (float)(hdgRads), Vector2.One, SpriteEffects.None, 0.0f);

            return true;
        }
        private bool DrawRock(Int32 x, Int32 y, double hdgRads)
        {
            Rectangle sourceRect;
            Rectangle destRect;

            x = (int)(x * SCREEN_SCALE);
            y = (int)(y * SCREEN_SCALE);


            if (RestrictToScreen(m_rockTexture, ref x, ref y) == false)
                return false;

            double hdgDegs = (hdgRads * 360.0) / GeneralMaths.TWO_PI;
            Debug.WriteLine("hdgDegs = " + hdgDegs.ToString());
            Debug.WriteLine("x = " + x.ToString());
            Debug.WriteLine("y = " + y.ToString());
            sourceRect = new Rectangle(0, 0, m_rockTexture.Width, m_rockTexture.Height);

            Point2D zeroPt;

            // REM the XNA texture rotates about the top left corner, so need to move the point to draw the texture so it rotates about the centre
            RotateTextureZeroPtAboutCentre(m_rockTexture, hdgRads, out zeroPt);

            destRect = new Rectangle(x + (Int32)zeroPt.X, y + (Int32)zeroPt.Y, m_rockTexture.Width, m_rockTexture.Height);


            spriteBatch.Draw(m_rockTexture, destRect, sourceRect, Color.White, (float)(hdgRads), Vector2.One, SpriteEffects.None, 0.0f);

            return true;
        }

        bool RotateTextureZeroPtAboutCentre(Texture2D tex, double hdgRads, out Point2D newZeroPoint)
        {
            Point2D textTopPt = new Point2D();
            textTopPt.X = -tex.Width / 2;
            textTopPt.Y = -tex.Height / 2;
            Point2D rotPt = new Point2D();
            rotPt.X = 0.0;
            rotPt.Y = 0.0;
            MovementAssist.RotatePoint(rotPt, textTopPt, -hdgRads, out newZeroPoint);
            Debug.WriteLine("resPt.X = " + newZeroPoint.X.ToString());
            Debug.WriteLine("resPt.Y = " + newZeroPoint.Y.ToString());
            return true;
        }


        private bool RestrictToScreen(Texture2D texture, ref Int32 x, ref Int32 y)
        {
            Int32 maxX = GraphicsDevice.Viewport.Width;
            Int32 maxY = GraphicsDevice.Viewport.Height;

            Int32 scX = x % maxX;
            Int32 scY = y % maxY;
            if (scX < 0)
                scX += maxX;
            if (scY < 0)
                scY += maxY;
            x = scX;
            y = maxY - scY;
            return true;
        }


    }
}
