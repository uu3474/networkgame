using System;

namespace NetworkGame
{
    public class GameParams
    {
        public int DPI;
        public string ProfileDir;
        public bool ShowFps;
        public bool UseAdvancedEffects;
        public Action Exit;

        public bool TouchInput;
        public bool GamePadInput;
        public bool MouseInput;
        public bool KeyboardInput;
    }

}
