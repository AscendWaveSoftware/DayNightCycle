public interface IMoveInputSource
{
    /// <summary>
    /// Rückgabe im Bereich [-1,1] pro Achse.
    /// </summary>
    UnityEngine.Vector2 ReadMove();
}
