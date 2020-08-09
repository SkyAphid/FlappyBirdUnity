using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{

    #region Singleton
    public static GlobalVariables INSTANCE;

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Debug.LogError("Multiple GlobalVariables instances found.");
        }

        INSTANCE = this;
    }

    #endregion

    private bool m_Paused = false;
    private bool m_GameOver = false;

    //The number of pipes the player has successfully passed through
    private int m_PipesCleared = 0;

    public static void SetPaused(bool Paused)
    {
        INSTANCE.m_Paused = Paused;
    }

    public static bool IsPaused()
    {
        return INSTANCE.m_Paused;
    }

    public static void SetGameOver(bool GameOver)
    {
        INSTANCE.m_GameOver = GameOver;
    }

    public static bool IsGameOver()
    {
        return INSTANCE.m_GameOver;
    }

    public static void AddClearedPipe()
    {
        INSTANCE.m_PipesCleared++;
    }

    public static int GetClearedPipes()
    {
        return INSTANCE.m_PipesCleared;
    }
}
