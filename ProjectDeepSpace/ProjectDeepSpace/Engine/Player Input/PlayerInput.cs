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
    public class PlayerInput
    {
        #region Variables, Getters and Setters

        private GlobalVariables globalVariables;
        private LogicEngine logicEngine;

        private KeyboardState currentKeyboardState;
        private KeyboardState oldKeyboardState;

        private MouseState currentMouseState;
        private MouseState oldMouseState;
        
        private Keys move = Keys.M;
        private Keys attack = Keys.N;
        private Keys fire = Keys.Space;
        private Keys cancel = Keys.Escape;
        private Keys endTurn = Keys.Enter;

        private Keys cameraUp = Keys.W;
        private Keys cameraDown = Keys.S;
        private Keys cameraLeft = Keys.A;
        private Keys cameraRight = Keys.D;

        private Keys help = Keys.F1;
        private Keys next = Keys.Right;
        private Keys previous = Keys.Left;

        #endregion Variables, Getters and Setters

        public PlayerInput(Game game, ContentManager content, ref GlobalVariables globalVariables, ref LogicEngine logicEngine)
        {
            this.globalVariables = globalVariables;
            this.logicEngine = logicEngine;
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
        }

        public void Update(GameTime gameTime)
        {
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            oldMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            if (IsMSWIncreasing())
            {
                logicEngine.OnMSWIncreasing();
            }
            if (IsMSWDecreasing())
            {
                logicEngine.OnMSWDecreasing();
            }
            if (IsLMBClicked())
            {
                logicEngine.OnLMBClicked();
            }
            if (IsRMBClicked())
            {
                logicEngine.OnRMBClicked();
            }
            if (IsAttackClicked())
            {
                logicEngine.OnAttackClicked();
            }   
            if (IsMoveClicked())
            {
                logicEngine.OnMoveClicked();
            }
            if (IsFireClicked())
            {
                logicEngine.OnFireClicked();
            }
            if (IsCancelClicked())
            {
                logicEngine.OnCancelClicked();
            }
            if (IsEndTurnClicked())
            {
                logicEngine.OnEndTurnClicked();
            }
            if (IsCameraUpClicked())
            {
                logicEngine.OnCameraUpClicked();
            }
            if (IsCameraDownClicked())
            {
                logicEngine.OnCameraDownClicked();
            }
            if (IsCameraLeftClicked())
            {
                logicEngine.OnCameraLeftClicked();
            }
            if (IsCameraRightClicked())
            {
                logicEngine.OnCameraRightClicked();
            }
            if (IsHelpClicked())
            {
                logicEngine.OnHelpClicked();
            }
            if (IsNextClicked())
            {
                logicEngine.OnNextClicked();
            }
            if (IsPreviousClicked())
            {
                logicEngine.OnPreviousClicked();
            }


        } //DONE

        private bool IsMSWIncreasing()
        {
            if (currentMouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue)
                return true;
            else
                return false;
        } //DONE

        private bool IsMSWDecreasing()
        {
            if (currentMouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue)
                return true;
            else
                return false;
        } //DONE

        private bool IsLMBClicked()
        {
            if (oldMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        } //DONE

        private bool IsRMBClicked()
        {
            if (oldMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        } //DONE

        private bool IsAttackClicked()
        {
            if (oldKeyboardState.IsKeyUp(attack) && currentKeyboardState.IsKeyDown(attack))
                return true;
            else
                return false;
        } //DONE

        private bool IsFireClicked()
        {
            if (oldKeyboardState.IsKeyUp(fire) && currentKeyboardState.IsKeyDown(fire))
                return true;
            else
                return false;
        } //DONE

        private bool IsMoveClicked()
        {
            if (oldKeyboardState.IsKeyUp(move) && currentKeyboardState.IsKeyDown(move))
                return true;
            else
                return false;
        } //DONE

        private bool IsEndTurnClicked()
        {
            if (oldKeyboardState.IsKeyUp(endTurn) && currentKeyboardState.IsKeyDown(endTurn))
                return true;
            else
                return false;
        } //DONE

        private bool IsCancelClicked()
        {
            if (oldKeyboardState.IsKeyUp(cancel) && currentKeyboardState.IsKeyDown(cancel))
                return true;
            else
                return false;
        } //DONE

        private bool IsCameraUpClicked()
        {
            if (currentKeyboardState.IsKeyDown(cameraUp))
                return true;
            else
                return false;
        } //DONE

        private bool IsCameraDownClicked()
        {
            if (currentKeyboardState.IsKeyDown(cameraDown))
                return true;
            else
                return false;
        } //DONE

        private bool IsCameraLeftClicked()
        {
            if (currentKeyboardState.IsKeyDown(cameraLeft))
                return true;
            else
                return false;
        } //DONE

        private bool IsCameraRightClicked()
        {
            if (currentKeyboardState.IsKeyDown(cameraRight))
                return true;
            else
                return false;
        } //DONE
        
        private bool IsHelpClicked()
        {
            if (oldKeyboardState.IsKeyUp(help) && currentKeyboardState.IsKeyDown(help))
                return true;
            else
                return false;
        } //DONE

        private bool IsNextClicked()
        {
            if (oldKeyboardState.IsKeyUp(next) && currentKeyboardState.IsKeyDown(next))
                return true;
            else
                return false;
        } //DONE

        private bool IsPreviousClicked()
        {
            if (oldKeyboardState.IsKeyUp(previous) && currentKeyboardState.IsKeyDown(previous))
                return true;
            else
                return false;
        } //DONE
    }
}
