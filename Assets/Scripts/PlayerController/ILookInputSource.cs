public interface ILookInputSource
{
    /// <summary>
    /// Maus/Gamepad-Look. X=Yaw (links/rechts), Y=Pitch (hoch/runter).
    /// </summary>
    UnityEngine.Vector2 ReadLook();    
}
