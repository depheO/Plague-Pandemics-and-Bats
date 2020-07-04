﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.CompilerServices;

namespace PlaguePandemicsBats
{
    public class Scene
    {
        #region Private variables
        private const float deg2Reg = (float)Math.PI / 180f;
        private string _sceneName;
        private Game1 _game;
        private SpriteBatch _spriteBatch;        
        private List<Sprite> _sprites;
        #endregion

        #region Constructor
        public Scene(Game1 game, string sceneFile)
        {
            _game = game;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _sprites = new List<Sprite>();

            JObject json = JObject.Parse(File.ReadAllText($"Content/pandemics/scenes/{sceneFile}.dt"));
            //gives us jtoken bc they are different types of data, but i convert it to string
            _sceneName = json ["sceneName"].Value<string>();

            //starts reading on composite
            foreach (JToken image in json ["composite"] ["sImages"])
            {
                string imgName = image ["imageName"].Value<string>();
                //if there is no x then the x is taken as value 0
                float x = image ["x"]?.Value<float>() ?? 0f;
                float y = image ["y"]?.Value<float>() ?? 0f;
                float rotation = deg2Reg * (image ["rotation"]?.Value<float>() ?? 0f);
                float scaleX = image ["scaleX"]?.Value<float>() ?? 1;
                float scaleY = image ["scaleY"]?.Value<float>() ?? 1;

                #region Player
                if (image ["itemIdentifier"]?.Value<string>() == "Player")
                {
                    _game.Player.SetSpawn(new Vector2(x, y));
                }
                #endregion

                #region Pink Zombie
                else if (image ["imageName"]?.Value<string>() == "ZGirlD0")
                {
                    new PinkZombie(_game, new Vector2(x, y));
                }
                #endregion

                #region CheckPoint
                else if (image ["itemIdentifier"]?.Value<string>() == "CheckPoint")
                {
                    Sprite sprite = new Sprite(_game, imgName, scale: new Vector2(scaleX, scaleY), collides: false);
                    sprite.SetPosition(new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2));
                    sprite.SetRotation(rotation);
                    _sprites.Add(sprite);

                    OBBCollider checkpointCollider = new OBBCollider(_game, "CheckPoint", new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2), sprite.size, rotation);
                    _game.CollisionManager.Add(checkpointCollider);
                }
                #endregion

                #region Red Tree
                else if (image["itemIdentifier"]?.Value<string>() == "RedTree")
                {
                    Sprite sprite = new Sprite(_game, imgName, scale: new Vector2(scaleX, scaleY), collides: false);
                    sprite.SetPosition(new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2));
                    sprite.SetRotation(rotation);
                    _sprites.Add(sprite);

                    OBBCollider RedTreeCollider = new OBBCollider(_game, "RedTree", new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2), sprite.size, rotation);
                    _game.CollisionManager.Add(RedTreeCollider);
                }
                #endregion

                #region TP Position
                else if (image["itemIdentifier"]?.Value<string>() == "TPPosition")
                {
                    Sprite sprite = new Sprite(_game, imgName, scale: new Vector2(scaleX, scaleY), collides: false);
                    _game.Player.TPpos = new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2);
                    sprite.SetPosition(new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2));
                    sprite.SetRotation(rotation);
                    _sprites.Add(sprite);
                }
                #endregion

                #region Blue House
                else if (image ["itemIdentifier"]?.Value<string>() == "BlueHouse")
                {
                    Sprite sprite = new Sprite(_game, imgName, scale: new Vector2(scaleX, scaleY), collides: false);
                    sprite.SetPosition(new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2));
                    sprite.SetRotation(rotation);
                    _sprites.Add(sprite);

                    OBBCollider BlueHouseeCollider = new OBBCollider(_game, "BlueHouse", new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2), sprite.size, rotation);
                    _game.CollisionManager.Add(BlueHouseeCollider);
                }
                #endregion

                #region TP 
                else if (image["itemIdentifier"]?.Value<string>() == "TP")
                {
                    Sprite sprite = new Sprite(_game, imgName, scale: new Vector2(scaleX, scaleY), collides: false);
                    sprite.SetPosition(new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2));
                    sprite.SetRotation(rotation);
                    _sprites.Add(sprite);

                    OBBCollider TpCollider = new OBBCollider(_game, "TP", new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2), sprite.size, rotation);
                    _game.CollisionManager.Add(TpCollider);
                }
                #endregion

                #region Shooter Zombie
                else if (image ["imageName"]?.Value<string>() == "ZGuyD0")
                {
                    new ShooterZombie(_game, new Vector2(x, y));
                }
                #endregion

                #region Glass Zombie
                else if (image ["imageName"]?.Value<string>() == "ZGlassBoyD0")
                {
                    new SpawnerZombie(_game, new Vector2(x, y));
                }
                #endregion

                #region Cat
                else if (image["imageName"]?.Value<string>() == "catD0")
                {
                    _game.Cat.SetPosition(new Vector2(x,y));
                }
                #endregion

                #region Ammo
                else if (image ["imageName"]?.Value<string>() == "cure")
                {
                    new Ammo(_game, new Vector2(x,y));
                }
                #endregion

                #region Dragon
                else if (image ["imageName"]?.Value<string>() == "DragonFront")
                {
                    _game.Dragon.SetPosition(new Vector2(x + _game.Dragon.SpriteSize.X / 2, y + _game.Dragon.SpriteSize.Y / 2));
                }
                #endregion

                #region Corona
                else if (image ["itemIdentifier"]?.Value<string>() == "borona")
                {
                    new Corona(_game, new Vector2(x, y));
                }
                #endregion

                #region No Colliders
                else if (image ["itemIdentifier"]?.Value<string>() == "NoCollider")
                {
                    Sprite sprite = new Sprite(_game, imgName, scale: new Vector2(scaleX, scaleY), collides: false);
                    sprite.SetPosition(new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2));
                    sprite.SetRotation(rotation);
                    _sprites.Add(sprite);
                }
                #endregion

                #region Collider
                else if (image ["tags"]?.Value<JArray>().ToString() == "collider")
                {                
                    Sprite sprite = new Sprite(_game, imgName, scale: new Vector2(scaleX, scaleY), collides: true);
                    sprite.SetPosition(new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2));
                    sprite.SetRotation(rotation);
                    _sprites.Add(sprite);
                }
                #endregion

                #region Rest of the scene
                else
                {
                    Sprite sprite = new Sprite(_game, imgName, scale: new Vector2(scaleX, scaleY), collides: true);
                    sprite.SetPosition(new Vector2(x + sprite.size.X / 2, y + sprite.size.Y / 2));
                    sprite.SetRotation(rotation);
                    _sprites.Add(sprite);
                }
                #endregion
            }
        }
        #endregion

        #region Properties
        public List<Sprite> Sprites => _sprites;
        #endregion

        #region Methods
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (Sprite s in Sprites.ToArray())
            {
                if (Vector2.DistanceSquared(s.position,_game.Player.Position) <= 7f * 7f)
                {
                    s.Draw(_spriteBatch);
                }
            }
            _spriteBatch.End();
        }
        #endregion
    }
}