using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenuController : MonoBehaviour
{

    //Constant for the name of the game scene; at the top for easy editing if needed
    private const string GAME_SCENE = "GameScene";

    #region Input

    public InputMaster Input
    {
        get { return m_InputMaster = m_InputMaster ?? new InputMaster(); }
    }

    private InputMaster m_InputMaster = null;

    #endregion

    [Header("Background")]
    #region Background

    [SerializeField] private Material m_BackgroundMatDay = null;
    [SerializeField] private Material m_BackgroundMatNight = null;
    [SerializeField] private GameObject m_Background = null;

    private TextureScroller m_BackgroundScroller = null;

    private TextureScroller BackgroundScroller
    {
        get { return m_BackgroundScroller = m_BackgroundScroller ?? m_Background.GetComponent<TextureScroller>(); }
    }

    #endregion

    #region Ground

    [SerializeField] private GameObject m_Ground = null;

    private TextureScroller m_GroundScroller = null;

    private TextureScroller GroundScroller
    {
        get { return m_GroundScroller = m_GroundScroller ?? m_Ground.GetComponent<TextureScroller>(); }
    }
    #endregion

    //Game scroll speed, modifies the overall speed of the background
    [SerializeField] private float m_ScrollSpeed = 0f;

    [Header("UI")]

    #region Screen Fader

    [SerializeField] private GameObject m_ScreenFaderPanel = null;

    private Fader m_ScreenFader = null;

    public Fader ScreenFader
    {
        get { return m_ScreenFader = m_ScreenFader ?? m_ScreenFaderPanel.GetComponent<Fader>(); }
    }

    #endregion

    #region Play Button

    [SerializeField] private GameObject m_PlayButtonObj = null;

    private Button m_PlayButton = null;

    public Button PlayButton
    {
        get { return m_PlayButton = m_PlayButton ?? m_PlayButtonObj.GetComponent<Button>(); }
    }

    #endregion

    private void OnEnable()
    {
        this.Input.Enable();

        //If the play button is clicked, start the game
        this.PlayButton.onClick.AddListener(() => StartGame());

        //Add callback to screen fader to change the scene when the screen is white
        this.ScreenFader.OnFadeInEnd += StartGameFaderCallback;
    }

    private void OnDisable()
    {
        this.PlayButton.onClick.RemoveAllListeners();

        this.ScreenFader.OnFadeInEnd -= StartGameFaderCallback;

        this.Input.Disable();
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
        this.BackgroundScroller.ManualUpdate(m_ScrollSpeed);
        this.GroundScroller.ManualUpdate(m_ScrollSpeed);
    }

    #region Callbacks

    //Start the game by transitioning to the game state
    private void StartGame()
    {
        m_PlayButtonObj.SetActive(false);
        this.ScreenFader.StartFadeIn(true);
    }
    private void StartGameFaderCallback()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(GAME_SCENE);
    }

    #endregion
}
