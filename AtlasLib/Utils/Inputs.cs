namespace AtlasLib.Utils;

public static class Inputs
{
    public static bool FirePressed => InputManager.Instance.InputSource.Fire1.WasPerformedThisFrame;
    public static bool FireHeld => InputManager.Instance.InputSource.Fire1.IsPressed;
    public static bool FireReleased => InputManager.Instance.InputSource.Fire1.WasCanceledThisFrame;
        
    public static bool AltFirePressed => InputManager.Instance.InputSource.Fire2.WasPerformedThisFrame;
    public static bool AltFireHeld => InputManager.Instance.InputSource.Fire2.IsPressed;
    public static bool AltFireReleased => InputManager.Instance.InputSource.Fire2.WasCanceledThisFrame;
        
    public static bool PunchPressed => InputManager.Instance.InputSource.Punch.WasPerformedThisFrame;
    public static bool PunchHeld => InputManager.Instance.InputSource.Punch.IsPressed;
    public static bool PunchReleased => InputManager.Instance.InputSource.Punch.WasCanceledThisFrame;
}