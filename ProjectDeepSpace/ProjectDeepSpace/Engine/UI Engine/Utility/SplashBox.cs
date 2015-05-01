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

namespace ProjectDeepSpace
{
    class SplashBox
    {
        String[] splashBox;
        TimeSpan timeSinceLast;
        SpriteFont font;
        Vector2[] textStart;
        GraphicsDevice device;
        
        public SplashBox(GraphicsDevice device, SpriteFont font)
        {
            timeSinceLast = new TimeSpan(0, 0, 0);
            this.device = device;
            this.font = font;

            splashBox = new String[5];
            splashBox[0] = "";
            splashBox[1] = "";
            splashBox[2] = "";
            splashBox[3] = "";
            splashBox[4] = "";

            textStart = new Vector2[5];

            textStart[0] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[0]).X / 2, 100);
            textStart[1] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[1]).X / 2, 80);
            textStart[2] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[2]).X / 2, 60);
            textStart[3] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[3]).X / 2, 40);
            textStart[4] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[4]).X / 2, 20);
        }

        public void NewMessage(String msg)
        {
            if (timeSinceLast > new TimeSpan(0, 0, 0, 0, 300))
            {
                splashBox[4] = splashBox[3];
                splashBox[3] = splashBox[2];
                splashBox[2] = splashBox[1];
                splashBox[1] = splashBox[0];
                splashBox[0] = msg;

                textStart[4] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[4]).X / 2, 20);
                textStart[3] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[3]).X / 2, 40);
                textStart[2] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[2]).X / 2, 60);
                textStart[1] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[1]).X / 2, 80);
                textStart[0] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[0]).X / 2, 100);

                timeSinceLast = new TimeSpan(0, 0, 0);
            }
        }

        private void Remove()
        {
            if (!splashBox[4].Equals(""))
            {
                splashBox[4] = "";
                textStart[4] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[4]).X / 2, 20);
                timeSinceLast = new TimeSpan(0, 0, 0);
            }
            else if (!splashBox[3].Equals(""))
            {
                splashBox[3] = "";
                textStart[3] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[3]).X / 2, 40);
                timeSinceLast = new TimeSpan(0, 0, 0);
            }
            else if (!splashBox[2].Equals(""))
            {
                splashBox[2] = "";
                textStart[2] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[2]).X / 2, 60);
                timeSinceLast = new TimeSpan(0, 0, 0);
            }
            else if (!splashBox[1].Equals(""))
            {
                splashBox[1] = "";
                textStart[1] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[1]).X / 2, 80);
                timeSinceLast = new TimeSpan(0, 0, 0);
            }
            else if (!splashBox[0].Equals(""))
            {
                splashBox[0] = "";
                textStart[0] = new Vector2(device.Viewport.Width / 2 - font.MeasureString(splashBox[0]).X / 2, 100);
                timeSinceLast = new TimeSpan(0, 0, 0);
            }
        }

        public void Update(TimeSpan elapsedTime)
        {
            timeSinceLast += elapsedTime;

            if (timeSinceLast > new TimeSpan(0, 0, 2))
            {
                Remove();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 5; i++)
            {
                spriteBatch.DrawString(font, splashBox[i], textStart[i], Color.White);
            }
        }
    }
}
