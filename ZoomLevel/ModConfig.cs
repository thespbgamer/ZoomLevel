using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomLevel
{
    internal class ModConfig
    {
        public SButton IncreaseZoomKey { get; set; } = SButton.OemPeriod;
        public SButton DecreaseZoomKey { get; set; } = SButton.OemComma;
        public SButton IncreaseZoomButton { get; set; } = SButton.RightStick;
        public SButton DecreaseZoomButton { get; set; } = SButton.LeftStick;
        public bool SuppressControllerButton { get; set; } = true;

        public float ZoomLevelIncreaseValue { get; set; } = 0.05f;
        public float ZoomLevelDecreaseValue { get; set; } = -0.05f;

        public float MaxZoomOutLevelValue { get; set; } = 0.35f;

        public float MaxZoomInLevelValue { get; set; } = 2.00f;

        public bool DisableControllerButton { get; set; } = false;
    }
}