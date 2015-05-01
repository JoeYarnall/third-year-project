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
    class CombatLog
    {
        SpriteFont font;
        GraphicsDevice device;

        Texture2D combatLogFrame;
        Rectangle screenRectangle;

        String[] combatLog;
        Vector2[] textStart;

        public CombatLog(GraphicsDevice device, ContentManager content, SpriteFont font)
        {
            this.font = font;
            this.device = device;

            combatLogFrame = content.Load<Texture2D>("UI/CombatLogFrame");
            screenRectangle = new Rectangle(0, 0, device.Viewport.Width, device.Viewport.Height);

            combatLog = new String[10];

            combatLog[9] = "";
            combatLog[8] = "";
            combatLog[7] = "";
            combatLog[6] = "";
            combatLog[5] = "";
            combatLog[4] = "";
            combatLog[3] = "";
            combatLog[2] = "";
            combatLog[1] = "";
            combatLog[0] = "";

            textStart = new Vector2[10];

            textStart[9] = new Vector2(405, 450);
            textStart[8] = new Vector2(405, 465);
            textStart[7] = new Vector2(405, 480);
            textStart[6] = new Vector2(405, 495);
            textStart[5] = new Vector2(405, 510);
            textStart[4] = new Vector2(405, 525);
            textStart[3] = new Vector2(405, 540);
            textStart[2] = new Vector2(405, 555);
            textStart[1] = new Vector2(405, 570);
            textStart[0] = new Vector2(405, 585);
        }

        public void NewMessage(String msg)
        {
            combatLog[9] = combatLog[8];
            combatLog[8] = combatLog[7];
            combatLog[7] = combatLog[6];
            combatLog[6] = combatLog[5];
            combatLog[5] = combatLog[4];
            combatLog[4] = combatLog[3];
            combatLog[3] = combatLog[2];
            combatLog[2] = combatLog[1];
            combatLog[1] = combatLog[0];
            combatLog[0] = msg;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(combatLogFrame, screenRectangle, Color.White);

            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(font, combatLog[i], textStart[i], Color.White);
            }
        }
    }
}
