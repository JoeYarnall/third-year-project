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
    public class KeyboardManager : Microsoft.Xna.Framework.GameComponent, IKeyboardManagerService
    {
        #region Variables, Getters and Setters

        private KeyboardState keyboardState;
        private KeyboardState oldKeyboardState;

        private Keys move = Keys.M;
        private Keys attack = Keys.N;
        private Keys build = Keys.B;
        private Keys cancel = Keys.Escape;
        private Keys endTurn = Keys.Enter;

        private Keys cameraUp = Keys.W;
        private Keys cameraDown = Keys.S;
        private Keys cameraLeft = Keys.A;
        private Keys cameraRight = Keys.D;

        private Keys altCameraUp = Keys.Up;
        private Keys altCameraDown = Keys.Down;
        private Keys altCameraLeft = Keys.Left;
        private Keys altCameraRight = Keys.Right;

        #endregion Variables, Getters and Setters

        public KeyboardManager(Game game)
            : base(game)
        {
            keyboardState = Keyboard.GetState();
        }
   
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            oldKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public Boolean IsAttackClicked()
        {
            if (keyboardState.IsKeyDown(attack))
                return true;
            else
                return false;
        } //DONE

        public Boolean IsMoveClicked()
        {
            if (keyboardState.IsKeyDown(move))
                return true;
            else
                return false;
        } //DONE

        public Boolean IsBuildClicked()
        {
            if (keyboardState.IsKeyDown(build))
                return true;
            else
                return false;
        } //DONE
        
        public Boolean IsEndTurnClicked()
        {
            if (keyboardState.IsKeyDown(endTurn))
                return true;
            else
                return false;
        } //DONE

        public Boolean IsCancelClicked()
        {
            if (keyboardState.IsKeyDown(cancel))
                return true;
            else
                return false;
        } //DONE

        public Boolean IsCameraUpClicked()
        {
            if (keyboardState.IsKeyDown(cameraUp) || keyboardState.IsKeyDown(altCameraUp))
                return true;
            else
                return false;
        } //DONE

        public Boolean IsCameraDownClicked()
        {
            if (keyboardState.IsKeyDown(cameraDown) || keyboardState.IsKeyDown(altCameraDown))
                return true;
            else
                return false;
        } //DONE

        public Boolean IsCameraLeftClicked()
        {
            if (keyboardState.IsKeyDown(cameraLeft) || keyboardState.IsKeyDown(altCameraLeft))
                return true;
            else
                return false;
        } //DONE

        public Boolean IsCameraRightClicked()
        {
            if (keyboardState.IsKeyDown(cameraRight) || keyboardState.IsKeyDown(altCameraRight))
                return true;
            else
                return false;
        } //DONE
    }
}
