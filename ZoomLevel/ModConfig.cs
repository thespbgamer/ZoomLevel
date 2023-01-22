using StardewModdingAPI.Utilities;

namespace ZoomLevel
{
    internal class ModConfig
    {
        public KeybindList KeybindListHoldToChangeUI { get; set; } = KeybindList.Parse("LeftShift, RightShift, LeftTrigger + RightTrigger");
        public KeybindList KeybindListIncreaseZoomOrUI { get; set; } = KeybindList.Parse("OemPeriod, RightStick");
        public KeybindList KeybindListDecreaseZoomOrUI { get; set; } = KeybindList.Parse("OemComma, LeftStick");
        public KeybindList KeybindListResetZoomOrUI { get; set; } = KeybindList.Parse("");
        public KeybindList KeybindListMaxZoomOrUI { get; set; } = KeybindList.Parse("");
        public KeybindList KeybindListMinZoomOrUI { get; set; } = KeybindList.Parse("");
        public KeybindList KeybindListToggleUI { get; set; } = KeybindList.Parse("");
        public KeybindList KeybindListToggleHideUIWithCertainZoom { get; set; } = KeybindList.Parse("");
        public KeybindList KeybindListChangeZoomToApproximateCurrentMapSize { get; set; } = KeybindList.Parse("");

        public KeybindList KeybindListMovementCameraLeft { get; set; } = KeybindList.Parse("");
        public KeybindList KeybindListMovementCameraRight { get; set; } = KeybindList.Parse("");
        public KeybindList KeybindListMovementCameraUp { get; set; } = KeybindList.Parse("");
        public KeybindList KeybindListMovementCameraDown { get; set; } = KeybindList.Parse("");
        public KeybindList KeybindListResetCameraMovement { get; set; } = KeybindList.Parse("");

        public float ZoomLevelIncreaseValue { get; set; } = 0.05f;
        public float ZoomLevelDecreaseValue { get; set; } = -0.05f;

        public float MaxZoomOutLevelAndUIValue { get; set; } = 0.35f;

        public float MaxZoomInLevelAndUIValue { get; set; } = 2.00f;

        public float ResetZoomOrUIValue { get; set; } = 1.00f;
        public float ZoomLevelThatHidesUI { get; set; } = 0.35f;
        public int CameraMovementSpeed { get; set; } = 25;

        public bool SuppressControllerButton { get; set; } = true;
        public bool ZoomAndUIControlEverywhere { get; set; } = false;
        public bool IsHideUIWithCertainZoom { get; set; } = false;

        public bool PressAnyButtonToCenterCamera { get; set; } = true;
    }
}