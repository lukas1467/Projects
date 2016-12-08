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

namespace Projekt
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D goalieTexture;
        
        Texture2D background;

        Rectangle goalieRectangle;
        Rectangle gateRectangle;

        Vector2 position;
        List<Shot> shotList;

        Rectangle mouseRectangle;
        Boolean canClick;

        Texture2D ballTexture;

        int scoreGoalie, scoreAttack;
        
        SpriteFont font;       
        SoundEffect goal;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            graphics.PreferredBackBufferWidth = 620; // okreœlamy tutaj szerokoœæ okna
            graphics.PreferredBackBufferHeight = 750; // okreœlamy tutaj wysokoœæ okna
            graphics.IsFullScreen = false; // czy ma byæ tryb pe³noekranowy
            graphics.ApplyChanges(); // Zatwierdzamy zmiany

            scoreGoalie = 0;
            scoreAttack = 0;

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


            //efekty dzwiekowe
            goal = Content.Load<SoundEffect>("sounds/GoalSound");

            goalieRectangle = new Rectangle(240, 660, 100, 85);
            gateRectangle = new Rectangle(0, 740, 620, 10);

            background = Content.Load<Texture2D>("boisko");
            goalieTexture = Content.Load<Texture2D>("goalie");
            

            shotList = new List<Shot>();

            mouseRectangle = new Rectangle(0, 0, 1, 1);
            canClick = true;

            ballTexture = Content.Load<Texture2D>("pika");
            IsMouseVisible = true; //kursor widoczny podczas gry

            font = Content.Load<SpriteFont>("Fonts");

            


            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();     

            if(Keyboard.GetState().IsKeyDown(Keys.Right) && goalieRectangle.X<graphics.GraphicsDevice.Viewport.Width- goalieRectangle.Width)
            {
                goalieRectangle.X += 15;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && goalieRectangle.X > 0)
            {
                goalieRectangle.X -= 15;
            }

            mouseRectangle.X = Mouse.GetState().X;
            mouseRectangle.Y = Mouse.GetState().Y;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && canClick == true)
            {
                Shot shot = new Shot(ballTexture, new Rectangle(mouseRectangle.X, mouseRectangle.Y, 64, 64), new Vector2(mouseRectangle.X, mouseRectangle.Y), 0f);
                shotList.Add(shot);
                canClick = false;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                canClick = true;
            }

            foreach(Shot shot in shotList)
            {
                shot.Position.Y += shot.Ball;
                shot.Rectangle.Y = (int)shot.Position.Y;
                shot.Ball += 0.5f;

            }

            foreach(Shot shot in shotList)
            {
                if(shot.Rectangle.Intersects(goalieRectangle))
                {
                    scoreGoalie += 1;
                }
                else
                {
                    if (shot.Rectangle.Intersects(gateRectangle))
                    {
                        scoreAttack += 1;
                        goal.Play();
                    }
                }
                
            }
            // TODO: Add your update logic here

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
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(goalieTexture, goalieRectangle, Color.White);
            spriteBatch.DrawString(font, "Goalie Score: " + scoreGoalie, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 25, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Black);
            spriteBatch.DrawString(font, "Shooter Score: " + scoreAttack, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 25, GraphicsDevice.Viewport.TitleSafeArea.Y + 40), Color.Black);
            foreach (Shot shot in shotList)
            {
                shot.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
