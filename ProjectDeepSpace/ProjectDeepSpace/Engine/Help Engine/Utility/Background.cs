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
    class Background
    {
        Texture2D background;
        Rectangle screenRectangle;
        bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Background(GraphicsDevice device, ContentManager content)
        {
            background = content.Load<Texture2D>("Help/Background");
            screenRectangle = new Rectangle(0, 0, device.Viewport.Width, device.Viewport.Height);
            active = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(background, screenRectangle, Color.White);
            }
        }
    }
}
