using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace NetworkGame.Engine
{
    public class InputWorker
    {
        HashSet<Keys> m_prevKeysSet;
        KeyboardHandlerParams m_keyboardParams;

        MouseHandlerParams m_mouseParams;

        GamePadHandlerParams m_gamePadParams;

        public InputWorker()
        {
            TouchPanel.EnabledGestures = GestureType.Tap
                | GestureType.FreeDrag | GestureType.DragComplete
                | GestureType.Pinch | GestureType.PinchComplete;

            m_prevKeysSet = new HashSet<Keys>();
            m_keyboardParams = new KeyboardHandlerParams();
            m_mouseParams = new MouseHandlerParams();
            m_gamePadParams = new GamePadHandlerParams();
        }

        public KeyboardHandlerParams GetKeyboardInput()
        {
            var keyboardState = Keyboard.GetState();
            var keys = keyboardState.GetPressedKeys();
            m_keyboardParams.DownKeysSet.Clear();
            m_keyboardParams.DownKeysSet.UnionWith(keys);
            m_keyboardParams.PressedKeysSet.Clear();
            m_keyboardParams.PressedKeysSet.UnionWith(keys);
            m_keyboardParams.PressedKeysSet.ExceptWith(m_prevKeysSet);

            m_prevKeysSet.Clear();
            m_prevKeysSet.UnionWith(keys);

            m_keyboardParams.Handled = false;

            return m_keyboardParams;
        }

        public MouseHandlerParams GetMouseInput()
        {
            const int preDragLength = 5;

            var mouseState = Mouse.GetState();

            m_mouseParams.Handled = false;

            m_mouseParams.IsPostDrag = false;

            bool isPredDrag = (m_mouseParams.IsLeftButtonDown && (mouseState.LeftButton == ButtonState.Pressed)
                || m_mouseParams.IsRightButtonPressed && (mouseState.RightButton == ButtonState.Pressed));

            if (m_mouseParams.IsDrag)
            {
                m_mouseParams.IsDrag = isPredDrag;
                if(!m_mouseParams.IsDrag)
                {
                    m_mouseParams.IsPostDrag = true;
                    m_mouseParams.DragStartPositionX = 0;
                    m_mouseParams.DragStartPositionY = 0;
                }
            }
            else
            {
                if (isPredDrag)
                {
                    if (!m_mouseParams.IsPreDrag)
                    {
                        m_mouseParams.IsPreDrag = true;

                        m_mouseParams.PreDragPositionX = mouseState.X;
                        m_mouseParams.PreDragPositionY = mouseState.Y;
                    }

                    var delta = new Vector2(mouseState.X, mouseState.Y) - m_mouseParams.PreDragPosition;
                    if (delta.Length() > preDragLength)
                    {
                        m_mouseParams.IsPreDrag = false;
                        m_mouseParams.IsDrag = true;

                        m_mouseParams.DragStartPositionX = mouseState.X;
                        m_mouseParams.DragStartPositionY = mouseState.Y;
                    }
                }
                else
                    m_mouseParams.IsPreDrag = false;

                m_mouseParams.IsLeftButtonPressed = (m_mouseParams.IsLeftButtonDown && (mouseState.LeftButton == ButtonState.Released));
                m_mouseParams.IsRightButtonPressed = (m_mouseParams.IsRightButtonDown && (mouseState.RightButton == ButtonState.Released));
            }

            if(!m_mouseParams.IsPreDrag)
            {
                m_mouseParams.PreDragPositionX = 0;
                m_mouseParams.PreDragPositionY = 0;
            }

            m_mouseParams.DeltaPositionX = mouseState.X - m_mouseParams.PositionX;
            m_mouseParams.DeltaPositionY = mouseState.Y - m_mouseParams.PositionY;
            m_mouseParams.PositionX = mouseState.X;
            m_mouseParams.PositionY = mouseState.Y;

            m_mouseParams.IsLeftButtonDown = (mouseState.LeftButton == ButtonState.Pressed);
            m_mouseParams.IsRightButtonDown = (mouseState.RightButton == ButtonState.Pressed);

            m_mouseParams.DeltaWheelValue = mouseState.ScrollWheelValue - m_mouseParams.WheelValue;
            m_mouseParams.WheelValue = mouseState.ScrollWheelValue;

            return m_mouseParams;
        }

        public TouchHandlerParams GetTouchInput()
        {
            var touchParams = new TouchHandlerParams();
            touchParams.IsGestureAvailable = TouchPanel.IsGestureAvailable;

            if (touchParams.IsGestureAvailable)
                touchParams.Gesture = TouchPanel.ReadGesture();

            return touchParams;
        }

        public GamePadHandlerParams GetGamePadInput()
        {
            var state = GamePad.GetState(PlayerIndex.One);
            m_gamePadParams.BackButtonPressed = (m_gamePadParams.BackButtonDown && state.Buttons.Back == ButtonState.Released);
            m_gamePadParams.BackButtonDown = (state.Buttons.Back == ButtonState.Pressed);

            return m_gamePadParams;
        }
    }

    public class GamePadHandlerParams
    {
        public bool BackButtonDown;
        public bool BackButtonPressed;
    }

    public class KeyboardHandlerParams
    {
        public bool Handled;
        public HashSet<Keys> DownKeysSet;
        public HashSet<Keys> PressedKeysSet;

        public KeyboardHandlerParams()
        {
            DownKeysSet = new HashSet<Keys>();
            PressedKeysSet = new HashSet<Keys>();
        }
    }

    public class MouseHandlerParams
    {
        public bool Handled;
        public bool IsLeftButtonDown;
        public bool IsRightButtonDown;
        public bool IsLeftButtonPressed;
        public bool IsRightButtonPressed;
        public bool IsPreDrag;
        public bool IsDrag;
        public bool IsPostDrag;
        public int PositionX;
        public int PositionY;
        public int PreDragPositionX;
        public int PreDragPositionY;
        public int DragStartPositionX;
        public int DragStartPositionY;
        public int DeltaPositionX;
        public int DeltaPositionY;
        public int WheelValue;
        public int DeltaWheelValue;

        public Vector2 Position { get { return new Vector2(PositionX, PositionY); } }
        public Vector2 PreDragPosition { get { return new Vector2(PreDragPositionX, PreDragPositionY); } }
        public Vector2 DragStartPosition { get { return new Vector2(DragStartPositionX, DragStartPositionY); } }
        public Vector2 DeltaPosition { get { return new Vector2(DeltaPositionX, DeltaPositionY); } }

        public bool IsInput { get { return IsLeftButtonPressed || IsDrag || IsPostDrag || DeltaWheelValue != 0; } }

        public void CopyFrom(MouseHandlerParams mouseParams, Vector2 offset)
        {
            Handled = mouseParams.Handled;
            IsLeftButtonDown = mouseParams.IsLeftButtonDown;
            IsRightButtonDown = mouseParams.IsRightButtonDown;
            IsLeftButtonPressed = mouseParams.IsLeftButtonPressed;
            IsRightButtonPressed = mouseParams.IsRightButtonPressed;
            IsDrag = mouseParams.IsDrag;
            PositionX = mouseParams.PositionX + (int)offset.X;
            PositionY = mouseParams.PositionY + (int)offset.Y;
            DeltaPositionX = mouseParams.DeltaPositionX;
            DeltaPositionY = mouseParams.DeltaPositionY;
            WheelValue = mouseParams.WheelValue;
            DeltaWheelValue = mouseParams.DeltaWheelValue;
        }

    }

    public struct TouchHandlerParams
    {
        public bool Handled;
        public bool IsGestureAvailable;
        public GestureSample Gesture;

        public TouchHandlerParams Copy(Vector2 offset)
        {
            var touchParams = new TouchHandlerParams();

            touchParams.Handled = Handled;
            touchParams.IsGestureAvailable = IsGestureAvailable;
            touchParams.Gesture = new GestureSample(Gesture.GestureType, Gesture.Timestamp,
                new Vector2(Gesture.Position.X + offset.X, Gesture.Position.Y + offset.Y),
                new Vector2(Gesture.Position2.X + offset.X, Gesture.Position2.Y + offset.Y),
                Gesture.Delta, Gesture.Delta2);

            return touchParams;
        }
    }

    public interface IKeyboardHandler
    {
        void Keyboard(KeyboardHandlerParams _params);
    }

    public interface IMouseHandler
    {
        void Mouse(MouseHandlerParams _params);
    }

    public interface ITouchHandler
    {
        void Touch(ref TouchHandlerParams _params);
    }
}
