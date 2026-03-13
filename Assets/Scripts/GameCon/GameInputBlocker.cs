using UnityEngine;

public static class GameInputBlocker
{
    public static bool IsInputBlocked()
    {
        return PauseMenuManager.IsPaused;
    }
}