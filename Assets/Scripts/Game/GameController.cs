using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    #region Input

    public InputMaster Input
    {
        get { return m_InputMaster = m_InputMaster ?? new InputMaster(); }
    }

    private InputMaster m_InputMaster = null;

    #endregion

    #region UI Controller

    [SerializeField] private UIController m_UIController = null;

    #endregion

    #region Bird Controller

    //Bird object
    [SerializeField] private GameObject m_Bird = null;

    //Bird Controller Script
    private BirdController m_BirdController = null;

    private BirdController BirdController
    {
        get { return m_BirdController = m_BirdController ?? m_Bird.GetComponent<BirdController>(); }
    }


    #endregion

    #region Background

    [SerializeField] private Material m_BackgroundMatDay = null;
    [SerializeField] private Material m_BackgroundMatNight = null;
    [SerializeField] private GameObject m_Background = null;

    private TextureScroller m_BackgroundScroller = null;

    private TextureScroller BackgroundScroller
    {
       get { return m_BackgroundScroller = m_BackgroundScroller ?? m_Background.GetComponent<TextureScroller>();  }
    }

    #endregion

    #region Ground

    [SerializeField] private GameObject m_Ground = null;

    private TextureScroller m_GroundScroller = null;

    private TextureScroller GroundScroller
    {
        get { return m_GroundScroller = m_GroundScroller ?? m_Ground.GetComponent<TextureScroller>();  }
    }
    #endregion

    #region Pipe Controller

    [SerializeField] private GameObject m_PipeManager = null;

    private PipeController m_PipeController = null;

    private PipeController PipeController
    {
        get { return m_PipeController = m_PipeController ?? m_PipeManager.GetComponent<PipeController>();  }
    }

    #endregion

    #region Game Pausing

    private bool m_IsPaused = false;
    private bool IsPaused { 
        get => m_IsPaused;

        set {
            m_IsPaused = value;

            if (OnGamePause != null)
            {
                OnGamePause.Invoke(IsPaused);
            }
        } 
    }

    #endregion

    #region Additional Game States & Configurations

    private bool m_IsGameOver = false;

    //The number of pipes the player has successfully passed through
    private int m_PipesCleared = 0;

    //Game scroll speed, modifies the overall speed of the game
    [SerializeField] private float m_ScrollSpeed = 0f;

    //Mulitplier for ground scrolling speed to match the pipe up with the grounds scrolling
    //Allows for you to only have to modify the speed of the ground scrolling to also modify the pipe scrolling as well
    [SerializeField] private float m_PipeSpeedMultiplier = 0f;


    #endregion

    #region Event Callbacks

    private event Action<int> OnPipeCleared = null;
    private event Action<int> OnGameOver = null;
    private event Action<bool> OnGamePause = null;

    #endregion

    private void OnEnable()
    {
        Input.Enable();

        //Connect GameManager to BirdController so it can listen for the Bird hitting anything
        //The bird is the one that calls the callback since it's the one detecting the collisions (instead of vice versa)
        this.BirdController.OnCollision += BirdCollisionHandler;
        this.BirdController.OnClearPipe += BirdClearPipeHandler;

        //This callback is triggered when you successfully clear a pipe
        this.OnPipeCleared += m_UIController.UpdateScoreTextCallback;

        //UI Callbacks for Game Over
        this.OnGameOver += m_UIController.GameOverCallback;

        //When the game is paused, notify the bird object so that it'll stop moving
        this.OnGamePause += this.BirdController.OnGamePause;

        //Pause the game by pressing the space bar
        this.Input.Player.Pause.performed += PauseButtonKeyCallback;
    }

    private void OnDisable()
    {
        Input.Disable();

        this.BirdController.OnCollision -= BirdCollisionHandler;
        this.BirdController.OnClearPipe -= BirdClearPipeHandler;

        this.OnPipeCleared -= m_UIController.UpdateScoreTextCallback;

        this.OnGameOver -= m_UIController.GameOverCallback;

        this.Input.Player.Pause.performed -= PauseButtonKeyCallback;
        this.OnGamePause -= this.BirdController.OnGamePause;
    }

    private void Start()
    {
        if (Random.value > 0.5f)
        {
            m_Background.GetComponent<MeshRenderer>().material = m_BackgroundMatDay;
        }
        else
        {
            m_Background.GetComponent<MeshRenderer>().material = m_BackgroundMatNight;
        }
    }

    void Update()
    {
        if (!IsPaused)
        {
            this.BirdController.ManualUpdate();

            if (!m_IsGameOver)
            {
                this.BackgroundScroller.ManualUpdate(m_ScrollSpeed);
                this.GroundScroller.ManualUpdate(m_ScrollSpeed);
                this.PipeController.ManualUpdate(m_ScrollSpeed, m_PipeSpeedMultiplier);
            }
        }
    }

    #region Various Callbacks

    //If the bird collides with something, it's game over
    private void BirdCollisionHandler(Collision2D collision)
    {
        m_IsGameOver = true;
        this.OnGameOver.Invoke(m_PipesCleared);
    }

    //Add points when you clear pipes
    private void BirdClearPipeHandler(Collider2D collider)
    {
        m_PipesCleared++;
        this.OnPipeCleared.Invoke(m_PipesCleared);
    }

    //This is called from the Pause GameObject's OnClick() function and the below function
    public void PauseGame()
    {
        this.IsPaused = !this.IsPaused;
        m_UIController.PauseButtonCallback(this.IsPaused);
        AudioController.PlaySFX(AudioController.Sound.Select);
        //Debug.Log("Pause Game");
    }

    //Optional way to pause game with the space/esc bar
    public void PauseButtonKeyCallback(InputAction.CallbackContext context)
    {
        PauseGame();
    }

    #endregion
}