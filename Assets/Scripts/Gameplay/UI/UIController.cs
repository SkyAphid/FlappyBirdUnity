using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{


    [Header("UI State Elements")]

    #region Splash Screen

    [SerializeField] private GameObject m_SplashScreen = null;

    #endregion

    #region Score Text

    //Store the Points Text Object
    [SerializeField] private GameObject m_ScoreText = null;

    //Then fetch the Text Mesh from the Text Object
    private TextMeshProUGUI m_ScoreTextMesh = null;
    public TextMeshProUGUI ScoreText
    {
        get { return m_ScoreTextMesh = m_ScoreTextMesh ?? m_ScoreText.GetComponent<TextMeshProUGUI>(); }
    }

    #endregion

    #region Screen Flash

    [SerializeField] private GameObject m_ScreenFlashPanel = null;

    private ScreenFlash m_ScreenFlash = null;

    public ScreenFlash ScreenFlash
    {
        get { return m_ScreenFlash = m_ScreenFlash ?? m_ScreenFlashPanel.GetComponent<ScreenFlash>(); }
    }

    //Only flash it once
    private bool m_HasFlashed = false;

    #endregion

    #region Game Over Screen

    [SerializeField] private GameObject m_GameOverScreen = null;

    private GameOverController m_GameOverController = null;

    private GameOverController GameOverController
    {
        get { return m_GameOverController = m_GameOverController ?? m_GameOverScreen.GetComponent<GameOverController>(); }
    }

    #endregion

    [Header("UI Buttons")]

    #region Pause Button

    [SerializeField] private Sprite m_SpritePause = null;
    [SerializeField] private Sprite m_SpritePlay = null;

    [SerializeField] private GameObject m_PauseButtonObj = null;

    private Button m_PauseButton = null;

    public Button PauseButton
    {
        get { return m_PauseButton = m_PauseButton ?? m_PauseButtonObj.GetComponent<Button>(); }
    }

    private Image m_PauseImage = null;

    private Image PauseImage
    {
        get { return m_PauseImage = m_PauseImage ?? m_PauseButtonObj.GetComponent<Image>(); }
    }

    #endregion

    #region Play Again Button

    [SerializeField] private GameObject m_PlayAgainObj = null;

    private Button m_PlayAgainButton = null;

    public Button PlayAgainButton
    {
        get { return m_PlayAgainButton = m_PlayAgainButton ?? m_PlayAgainObj.GetComponent<Button>(); }
    }

    #endregion

    #region Main Menu Button

    [SerializeField] private GameObject m_MainMenuButtonObj = null;

    private Button m_MainMenuButton = null;

    public Button MainMenuButton
    {
        get { return m_MainMenuButton = m_MainMenuButton ?? m_MainMenuButtonObj.GetComponent<Button>(); }
    }

    #endregion

    private int m_FinalScore = 0;

    public void OnEnable()
    {
        ScreenFlash.OnFlashEnd += ShowGameOverScreen;  
    }

    public void OnDisable()
    {
        ScreenFlash.OnFlashEnd -= ShowGameOverScreen;
    }

    public void Start()
    {
        //Activate the splash screen on game start
        m_SplashScreen.SetActive(true);
    }
    
    //Notify the splash screen that it can fade out once the player begins playing. Only called once per round.
    public void OnGameStartCallback ()
    {
        m_SplashScreen.GetComponent<SplashScreen>().StartFaderTimer();
    }

    //Updates the point text at the top of the screen with the current score
    public void UpdateScoreTextCallback(int score)
    {
        this.ScoreText.text = score.ToString();
    }

    public void PauseButtonCallback(bool isPaused)
    {
        if (isPaused)
        {
            this.PauseImage.sprite = m_SpritePlay;
        } else
        {
            this.PauseImage.sprite = m_SpritePause;
        }
    }

    //Calls various UI functions for when the game is over
    public void GameOverCallback(int finalScore)
    {

        if (!m_HasFlashed)
        {
            this.ScreenFlash.ShowFlash(true, true);

            m_SplashScreen.SetActive(false);
            m_ScoreText.SetActive(false);
            m_PauseButtonObj.SetActive(false);

            m_HasFlashed = true;
            m_FinalScore = finalScore;
        }
    }

    private void ShowGameOverScreen()
    {
        this.GameOverController.Activate(m_FinalScore);
    }

}
