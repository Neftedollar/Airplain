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

        const float houserate = 0.005f;
        //layers
        //List that will store game object layers. Each layer has a string name
        Dictionary<string, GameLayer> gameLayers = new Dictionary<string, GameLayer>();

        //object textures

        Texture2D planeImage;
        Texture2D skyImage;
        Texture2D cityImage;
        Texture2D cloudImage;
        Texture2D houseImage;
        Texture2D starsImage;

        //ingame objects
        DenseGameObject plane;
        DenseGameObject []houses;

        DenseGameObject cloud_temp;
        Rectangle cloudsArea;

        //decorative objects
        GameObject sky, stars;
        GameObject farcity, farcityTwin;

        //class that will check collisions between DenseGameObjects that added.
        Collider collider;
        Collider leftScreenCollider;
        DenseGameObject leftScreenTrigger;

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
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 400;
            Window.Title = "Paper Plane";
            graphics.ApplyChanges();

            graphics.PreferMultiSampling = true;

            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;

            //
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
            starsImage = Content.Load<Texture2D>("images/stars");

            InitializeGameObjects(); //shouldnaa't be here
        }

        protected void InitializeGameObjects()
        {
            //ingame object init

            plane = new DenseGameObject(new Vector2(100,100), planeImage);
            leftScreenTrigger = new DenseGameObject(new Rectangle(-500,0,500,screenHeight), houseImage);

            cloud_temp = new DenseGameObject(new Vector2(150,100), cloudImage);
            cloudsArea = new Rectangle(screenWidth, 0, 200, screenHeight / 3);
            objectSpawnRandom(cloud_temp, cloudsArea);

            plane.Tag = "Player";
 
            cloud_temp.Tag = "Cloud";

            //decorations
            sky = new GameObject(new Rectangle(0, 0, screenWidth, screenHeight), skyImage);

            //The "infinite" background city is made of two images that run looped each after other
            farcity = new GameObject(new Rectangle(0, 0, screenWidth, screenHeight + 5), cityImage);
            farcityTwin = new GameObject(new Rectangle(screenWidth, 5, screenWidth, screenHeight + 5), cityImage);

            stars = new GameObject(new Rectangle(0, 5, screenWidth, screenHeight + 5), starsImage);

            //set the speed of the background city
            farcity.Speed = new Vector2(-nspeed(30),0);
            farcityTwin.Speed = new Vector2(-nspeed(30), 0);

            //tune the plane
            plane.Scale = 0.3f;

            InitializeLayers();
            InitializeColliders();
           
        }

        protected void InitializeLayers()
        {
             //layers init
            gameLayers.Add("plane", new GameLayer());
            gameLayers.Add("sky", new GameLayer());
            gameLayers.Add("farcity", new GameLayer());
            gameLayers.Add("stars", new GameLayer());

            //set the draw depth for the game layers
            gameLayers["plane"].Depth = 0.8f;
            gameLayers["farcity"].Depth = 0.5f;
            gameLayers["sky"].Depth = 0.0f;
            gameLayers["stars"].Depth = 0.1f;

            //add objects to the layers
            gameLayers["plane"].addObject(plane);
            gameLayers["plane"].addObject(cloud_temp);
            gameLayers["plane"].addObject(leftScreenTrigger);

            gameLayers["sky"].addObject(sky);
            gameLayers["stars"].addObject(stars);
            gameLayers["farcity"].addObject(farcity);
            gameLayers["farcity"].addObject(farcityTwin);

        }

        protected void InitializeColliders()
        {
            collider = new Collider();

            //add dense objects to the collider
            collider.addObject(plane);
            plane.CollisionEvent = onPlayerHit;

            leftScreenCollider = new Collider();
            leftScreenCollider.addObject(leftScreenTrigger);
            leftScreenCollider.addObject(plane);
            leftScreenTrigger.CollisionEvent = onPlayerInArea;
        }

        protected void onPlayerInArea(GameObject self, GameObject other)
        {
            if (self.Position.X + self.SizeScaled.X > other.Position.X + other.SizeScaled.X && other.Tag == "Player")
            {

                other.Position = new Vector2(200, 350);
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected void MoveObjects()
        {
            //to move objects in the world coordinates add summ of its speed and layer's speed to the object's position
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
            //loop the background city
            if (farcity.Position.X + screenWidth < farcity.Speed.X)
            {
                farcity.Position = new Vector2(farcity.Position.X + screenWidth, 5);
                farcityTwin.Position = new Vector2(farcityTwin.Position.X + screenWidth, 5);
            }

            //check it check
            collider.CheckCollisions();
            leftScreenCollider.CheckCollisions();
        }

        protected override void Update(GameTime gameTime)
        {
            float fps;
            if (gameTime.ElapsedGameTime.Milliseconds > 0)
                fps = 1000.0f / gameTime.ElapsedGameTime.Milliseconds;

            KeyboardState kbState = Keyboard.GetState();
            {
                if (kbState.IsKeyDown(Keys.Left))
                {
                    plane.Speed = new Vector2(-nspeed(150), 0);
                }

                if (kbState.IsKeyDown(Keys.Right))
                {
                    plane.Speed = new Vector2(nspeed(150), 0);
                }

                if (kbState.IsKeyDown(Keys.Up))
                {
                    plane.Speed = new Vector2(0, -nspeed(150));
                }

                if (kbState.IsKeyDown(Keys.Down))
                {
                    plane.Speed = new Vector2(0, nspeed(150));
                }

            }

            MoveObjects();
            CheckCollisions();

            float dice = (float)random.Next(1000)/1000;
            if ((dice <= houserate))
            {
                objectSpawnHouse();
            }

            base.Update(gameTime);
        }

        protected void DrawLayer(GameLayer layer)
        {
            foreach (GameObject gameObject in layer)
            {
                if (gameObject.IsVisible)
                {

                    //don't sure if this float to int conversion is right
                    spriteBatch.Draw(
                        gameObject.Image,       //an object texture
                        new Rectangle(          //an object size and position
                            (int)(gameObject.Position.X + layer.Position.X), 
                            (int)(gameObject.Position.Y + layer.Position.Y),
                            (int)(gameObject.Size.X*gameObject.Scale*layer.Scale), //dont forget about the scale
                            (int)(gameObject.Size.Y*gameObject.Scale*layer.Scale)),
                         null,  //?
                         Color.White,   
                         gameObject.Rotation + layer.Rotation,
                         Vector2.Zero,  //?
                         SpriteEffects.None,
                         layer.Depth);  //draw depth
                }
            }
        }

        /// <summary>
        /// goes through layers list and calls DrawLayer for each one
        /// </summary>
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
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected float nspeed(int speed) { return speed / 60.0f; }

        /// <summary>
        /// Moves an object withing the specified rectangle.
        /// </summary>
        /// <param name="obj">An object to spawn</param>
        /// <param name="spawnRect">A region to spawn in.</param>
        protected void objectSpawnRandom(GameObject obj, Rectangle spawnRect)
        {
            obj.Position = objectSpawnRandomCoords(spawnRect);
        }

        /// <summary>
        /// Returns a 2D vector with random X,Y coordinates withing specified rectangle
        /// </summary>
        /// <param name="spawnRect">Rectangle</param>
        /// <returns></returns>
        protected Vector2 objectSpawnRandomCoords(Rectangle spawnRect)
        {
            int x = spawnRect.X + random.Next(spawnRect.Width);
            int y = spawnRect.Y + random.Next(spawnRect.Height);
            return new Vector2(x,y);
        }

        protected void objectSpawnHouse()
        {
            int houseHeight = 100+random.Next(150);
            int houseWidth = 100+random.Next(80);
            DenseGameObject house = new DenseGameObject(new Rectangle(screenWidth,screenHeight-houseHeight,houseWidth, houseHeight),houseImage);
            house.Tag = "House";
            house.Speed = new Vector2(-nspeed(80), 0);
            collider.addObject(house);
            gameLayers["plane"].addObject(house);
        }

        void onPlayerHit(DenseGameObject self,DenseGameObject other)
        {
            if (other.Tag == "House")
            {
                self.Position = new Vector2(self.Position.X - 150, self.Position.Y);
            }
        }
    }
}
