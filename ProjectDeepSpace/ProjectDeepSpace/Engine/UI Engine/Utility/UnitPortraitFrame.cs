using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjectDeepSpace
{
    class UnitPortraitFrame
    {
        GraphicsDevice device;
        Rectangle portraitLocation;
        SpriteFont font;
        Texture2D[] portrait;
        int portraitIndex;
        bool active;
        string unitName;
        Vector2 unitNameTextLocation;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public UnitPortraitFrame(ContentManager content, GraphicsDevice device, SpriteFont font)
        {
            this.font = font;
            this.device = device;

            portraitLocation = new Rectangle(0, 450, 150, 150);
            
            portrait = new Texture2D[11];
            portraitIndex = 0;
            
            portrait[0] = content.Load<Texture2D>("UI/HumanBasePortrait");
            portrait[1] = content.Load<Texture2D>("UI/SmallHumanShipPortrait");
            portrait[2] = content.Load<Texture2D>("UI/MediumHumanShipPortrait");
            portrait[3] = content.Load<Texture2D>("UI/LargeHumanShipPortrait");
            portrait[4] = content.Load<Texture2D>("UI/AIBasePortrait");
            portrait[5] = content.Load<Texture2D>("UI/SmallAIShipPortrait");
            portrait[6] = content.Load<Texture2D>("UI/MediumAIShipPortrait");
            portrait[7] = content.Load<Texture2D>("UI/LargeAIShipPortrait");
            portrait[8] = content.Load<Texture2D>("UI/PlanetPortrait");
            portrait[9] = content.Load<Texture2D>("UI/StarPortrait");
            portrait[10] = content.Load<Texture2D>("UI/AsteroidPortrait");
        }

        public void Update(GameObjectType type)
        {
            if (type == GameObjectType.Human_Base)
            {
                portraitIndex = 0;
                unitName = "Human Base";
            }
            else if (type == GameObjectType.Small_Human_Ship)
            {
                portraitIndex = 1;
                unitName = "Small Human Ship";
            }
            else if (type == GameObjectType.Medium_Human_Ship)
            {
                portraitIndex = 2;
                unitName = "Medium Human Ship";
            }
            else if (type == GameObjectType.Large_Human_Ship)
            {
                portraitIndex = 3;
                unitName = "Large Human Ship";
            }
            else if (type == GameObjectType.AI_Base)
            {
                portraitIndex = 4;
                unitName = "Alien Base";
            }
            else if (type == GameObjectType.Small_AI_Ship)
            {
                portraitIndex = 5;
                unitName = "Small Alien Ship";
            }
            else if (type == GameObjectType.Medium_AI_Ship)
            {
                portraitIndex = 6;
                unitName = "Medium Alien Ship";
            }
            else if (type == GameObjectType.Large_AI_Ship)
            {
                portraitIndex = 7;
                unitName = "Large Alien Ship";
            }
            else if (type == GameObjectType.Planet)
            {
                portraitIndex = 8;
                unitName = "Planet";
            }
            else if (type == GameObjectType.Star)
            {
                portraitIndex = 9;
                unitName = "Star";
            }
            else if (type == GameObjectType.Asteroid)
            {
                portraitIndex = 10;
                unitName = "Asteroid";
            }

            unitNameTextLocation = new Vector2(75 - font.MeasureString(unitName).X / 2, 590 - font.MeasureString(unitName).Y / 2);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(portrait[portraitIndex], portraitLocation, Color.White);
                spriteBatch.DrawString(font, unitName, unitNameTextLocation, Color.White);
            }
        }
    }
}
