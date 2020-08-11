using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class GameController : MonoBehaviour
{

    [SerializeField] private GameObject m_Bird = null;
    [SerializeField] private GameObject m_Background = null;
    [SerializeField] private GameObject m_Ground = null;
    [SerializeField] private GameObject m_PipeManager = null;

    #region Bird Controller
    private BirdController m_BirdController = null;

    private BirdController BirdController
    {
        get { return m_BirdController = m_BirdController ?? m_Bird.GetComponent<BirdController>(); }
    }

    #endregion

    #region Background Scroller

    private TextureScroller m_BackgroundScroller = null;

    private TextureScroller BackgroundScroller
    {
       get { return m_BackgroundScroller = m_BackgroundScroller ?? m_Background.GetComponent<TextureScroller>();  }
    }

    #endregion

    #region Ground Scroller

    private TextureScroller m_GroundScroller = null;

    private TextureScroller GroundScroller
    {
        get { return m_GroundScroller = m_GroundScroller ?? m_Ground.GetComponent<TextureScroller>();  }
    }
    #endregion

    #region Pipe Controller

    private PipeController m_PipeController = null;

    private PipeController PipeController
    {
        get { return m_PipeController = m_PipeController ?? m_PipeManager.GetComponent<PipeController>();  }
    }

    #endregion

    #region Game States & Configurations

    private bool m_IsPaused = false;
    private bool m_IsGameOver = false;

    //The number of pipes the player has successfully passed through
    private int m_PipesCleared = 0;

    //Game scroll speed, modifies the overall speed of the game
    [SerializeField] private float m_ScrollSpeed = 0f;

    //Mulitplier for ground scrolling speed to match the pipe up with the grounds scrolling
    //Allows for you to only have to modify the speed of the ground scrolling to also modify the pipe scrolling as well
    [SerializeField] private float m_PipeSpeedMultiplier = 0f;

    #endregion

    #region Game State Callbacks

    public event Action<int> OnPipeCleared = null;
    public event Action OnGameOver = null;

    #endregion

    private void OnEnable()
    {
        //Hook up action callbacks
        this.BirdController.OnCollision += BirdCollisionHandler;
        this.BirdController.OnClearPipe += BirdClearPipeHandler;
    }

    private void OnDisable()
    {
        this.BirdController.OnCollision -= BirdCollisionHandler;
        this.BirdController.OnClearPipe -= BirdClearPipeHandler;
    }

    void Update()
    {
        this.BirdController.ManualUpdate();

        if (!m_IsGameOver)
        {
            this.BackgroundScroller.ManualUpdate(m_ScrollSpeed);
            this.GroundScroller.ManualUpdate(m_ScrollSpeed);
            this.PipeController.ManualUpdate(m_ScrollSpeed, m_PipeSpeedMultiplier);
        }
    }

    private void BirdCollisionHandler(Collision2D collision)
    {
        m_IsGameOver = true;
        OnGameOver.Invoke();
    }

    private void BirdClearPipeHandler(Collider2D collider)
    {
        m_PipesCleared++;
        OnPipeCleared.Invoke(m_PipesCleared);
    }

}