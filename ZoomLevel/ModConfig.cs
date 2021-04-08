using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomLevel
{
    internal class ModConfig
    {
        public KeybindList increaseZoomOrUI { get; set; } = KeybindList.Parse("OemPeriod, RightStick");
        public KeybindList decreaseZoomOrUI { get; set; } = KeybindList.Parse("OemComma, LeftStick");
        public KeybindList holdToChangeUIKeys { get; set; } = KeybindList.Parse("LeftShift, RightShift, LeftTrigger + RightTrigger");

        public bool SuppressControllerButton { get; set; } = true;

        public float ZoomLevelIncreaseValue { get; set; } = 0.05f;
        public float ZoomLevelDecreaseValue { get; set; } = -0.05f;

        public float MaxZoomOutLevelValue { get; set; } = 0.35f;

        public float MaxZoomInLevelValue { get; set; } = 2.00f;
    }
}