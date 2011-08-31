using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Airplane
{
    
    public class Airplane : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;

        int screenWidth, screenHeight;

        //layers

        Dictionary<string, GameLayer> gameLayers = new Dictionary<string, GameLayer>();

        //textures

        Texture2D planeImage;
        Texture2D skyImage;
        Texture2D cityImage;
        Texture2D cloudImage;
        Texture2D houseImage;

        Texture2D dummyTexture;

        //ingame objects

        static int NCLOUDS = 1;
        static int PCLOUDS = 4;
        static int FCLOUDS = 10;

        DenseGameObject plane;
        DenseGameObject ahouse;

        GameObject sky;
        GameObject farcity, farcityTwin;

        GameObject[] clouds_near = new GameObject[NCLOUDS];
        GameObject[] clouds_plane = new GameObject[PCLOUDS];
        GameObject[] clouds_far = new GameObject[FCLOUDS];

        //collider
        Collider collider; 

        Random random = new Random();

        public Airplane()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //graphics init
            this.IsMouseVisible = true;
            device = graphics.GraphicsDevice;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 800;
            Window.Title = "Paper Plane";
            graphics.ApplyChanges();

            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load textures
            planeImage = Content.Load<Texture2D>("images/plane2");
            skyImage = Content.Load<Texture2D>("images/sky");
            cityImage = Content.Load<Texture2D>("images/farcity");
            cloudImage = Content.Load<Texture2D>("images/CLOUD1");
            houseImage = Content.Load<Texture2D>("images/rect3102");

            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            InitializeGameObjects(); //shouldnaa't be here
        }

        protected void InitializeGameObjects()
        {
            collider = new Collider();
            //ingame object init

            plane = new DenseGameObject(new Vector2(100,100), planeImage);
            ahouse = new DenseGameObject(new Vector2(300, 300), houseImage);
            
            plane.CollisionEvent = onPlayerHit;

            plane.Tag = "Player";
            ahouse.Tag = "House";

            //decorations
            sky = new GameObject(new Vector2(0, 0), skyImage);
            farcity = new GameObject(new Vector2(0, 5), cityImage);
            farcityTwin = new GameObject(new Vector2(screenWidth, 5), cityImage);
            
            farcity.Speed = new Vector2(-nspeed(87),0);
            farcityTwin.Speed = new Vector2(-nspeed(87), 0);

            //layers init
            gameLayers.Add("plane", new GameLayer());
            gameLayers.Add("sky", new GameLayer());
            gameLayers.Add("farcity", new GameLayer());

            gameLayers["plane"].Level = 0.8f;
            gameLayers["farcity"].Level = 0.5f;
            gameLayers["sky"].Level = 0.0f;

            gameLayers["plane"].addObject(plane);
            gameLayers["plane"].addObject(ahouse);

            gameLayers["sky"].addObject(sky);
            gameLayers["farcity"].addObject(farcity);
            gameLayers["farcity"].addObject(farcityTwin);
            
            //add dense objects to the collider
            collider.addObject(plane);
            collider.addObject(ahouse);

            plane.Scale = 0.4f;

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected void MoveObjects()
        {
            foreach (KeyValuePair<string, GameLayer> layer in gameLayers)
            {
                foreach (GameObject gameObject in layer.Value)
                {
                    gameObject.Position += gameObject.Speed + layer.Value.Speed;
                }
            }
        }
        protected void CheckCollisions()
        {
            if (farcity.Position.X + screenWidth < farcity.Speed.X)
            {
                farcity.Position = new Vector2(farcity.Position.X + screenWidth, 5);
                farcityTwin.Position = new Vector2(farcityTwin.Position.X + screenWidth, 5);
            }

            collider.CheckCollisions();

        }

        protected override void Update(GameTime gameTime)
        {
            float framerate;
            if (gameTime.ElapsedGameTime.Milliseconds > 0)
                framerate = 1000.0f / gameTime.ElapsedGameTime.Milliseconds;

            KeyboardState kbState = Keyboard.GetState();
            {
                if (kbState.IsKeyDown(Keys.Left))
                {
                    plane.Speed = new Vector2(-nspeed(100), 0);
                }

                if (kbState.IsKeyDown(Keys.Right))
                {
                    plane.Speed = new Vector2(nspeed(100), 0);
                }

                if (kbState.IsKeyDown(Keys.Up))
                {
                    plane.Speed = new Vector2(0, -nspeed(100));
                }

                if (kbState.IsKeyDown(Keys.Down))
                {
                    plane.Speed = new Vector2(0, nspeed(100));
                }

            }

            MoveObjects();
            CheckCollisions();
            base.Update(gameTime);
        }

        protected void DrawLayer(GameLayer layer)
        {
            foreach (GameObject gameObject in layer)
            {
                if (gameObject.IsVisible)
                {
                   /* spriteBatch.Draw(
                        gameObject.Image,
                        gameObject.Position + layer.Position,
                        null,
                        Color.White,
                        gameObject.Rotation + layer.Rotation,
                        Vector2.Zero,
                        gameObject.Scale * layer.Scale,
                        SpriteEffects.None, layer.Level
                        );*/

                    //don't sure if this float to int conversion is right
                    spriteBatch.Draw(
                        gameObject.Image, 
                        new Rectangle(
                            (int)(gameObject.Position.X + layer.Position.X),
                            (int)(gameObject.Position.Y + layer.Position.Y),
                            (int)(gameObject.Size.X*gameObject.Scale*layer.Scale),
                            (int)(gameObject.Size.Y*gameObject.Scale*layer.Scale)),
                         null,
                         Color.White,
                         gameObject.Rotation + layer.Rotation,
                         Vector2.Zero,
                         SpriteEffects.None,
                         layer.Level);
                }
            }
        }

        protected void DrawScene()
        {
            foreach (KeyValuePair<string, GameLayer> layer in gameLayers)
            {
                DrawLayer(layer.Value);
            }
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            DrawScene();
            //spriteBatch.Draw(dummyTexture, new Rectangle(400,400,300,300), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected float nspeed(int speed) { return speed / 60.0f; }

        void onPlayerHit(DenseGameObject obj)
        {
            Console.WriteLine("Player hit.");
        }
    }
}
