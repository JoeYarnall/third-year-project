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
    class HPBar
    {
        GraphicsDevice device;
        Rectangle hpBarLocation;
        SpriteFont font;
        Texture2D[] hpBar;
        int hpBarDraw;
        bool active;
        string hpBarText;
        Vector2 hpBarTextLocation;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public HPBar(ContentManager content, GraphicsDevice device, SpriteFont font)
        {
            this.device = device;
            this.font = font;
            hpBarLocation = new Rectangle(150, 450, 250, 75);
            hpBar = new Texture2D[2];
            hpBar[0] = content.Load<Texture2D>("UI/HP");
            hpBar[1] = content.Load<Texture2D>("UI/HP2");
            hpBarDraw = 0;
        }

        public void Update(int maxHP, int currentHP)
        {
            if (currentHP / maxHP == 1)
            {
                hpBarDraw = 0;
            }
            else
            {
                hpBarDraw = 1;
            }

            hpBarText = "Health Points: " + currentHP + " / " + maxHP;
            hpBarTextLocation = new Vector2(275 - font.MeasureString(hpBarText).X / 2, 488 - font.MeasureString(hpBarText).Y / 2);
            hpBarLocation = new Rectangle(150, 450, (250 * currentHP / maxHP), 75);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(hpBar[hpBarDraw], hpBarLocation, Color.White);
                spriteBatch.DrawString(font, hpBarText, hpBarTextLocation, Color.White);
            }
        }
    }
}
