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
    class TargetAPBar
    {
        GraphicsDevice device;
        Rectangle apBarLocation;
        SpriteFont font;
        Texture2D[] apBar;
        int apBarDraw;
        bool active;
        string apBarText;
        Vector2 apBarTextLocation;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public TargetAPBar(ContentManager content, GraphicsDevice device, SpriteFont font)
        {
            this.device = device;
            this.font = font;
            apBarLocation = new Rectangle(275, 400, 250, 50);
            apBar = new Texture2D[2];
            apBar[0] = content.Load<Texture2D>("UI/TargetAP");
            apBar[1] = content.Load<Texture2D>("UI/TargetAP2");
            apBarDraw = 0;
        }

        public void Update(int maxAP, int currentAP)
        {
            if (currentAP / maxAP == 1)
            {
                apBarDraw = 0;
            }
            else
            {
                apBarDraw = 1;
            }

            apBarText = "Action Points: " + currentAP + " / " + maxAP;
            apBarTextLocation = new Vector2(400 - font.MeasureString(apBarText).X / 2, 425 - font.MeasureString(apBarText).Y / 2);
            apBarLocation = new Rectangle(275, 400, (250 * currentAP / maxAP), 50);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(apBar[apBarDraw], apBarLocation, Color.White);
                spriteBatch.DrawString(font, apBarText, apBarTextLocation, Color.White);
            }
        }
    }
}
