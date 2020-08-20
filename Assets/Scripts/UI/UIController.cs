using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

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

    #region Death Flash

    [SerializeField] private GameObject m_DeathFlashPanel = null;

    private DeathFlash m_DeathFlash = null;

    private DeathFlash DeathFlash
    {
        get { return m_DeathFlash = m_DeathFlash ?? m_DeathFlashPanel.GetComponent<DeathFlash>(); }
    }

    //Only flash it once
    private bool m_HasFlashed = false;

    #endregion

    #region Pause Button

    [SerializeField] private Sprite m_SpritePause = null;
    [SerializeField] private Sprite m_SpritePlay = null;

    [SerializeField] private GameObject m_PauseButtonParent = null;

    private Button m_PauseButton = null;

    private Button PauseButton
    {
        get { return m_PauseButton = m_PauseButton ?? m_PauseButtonParent.GetComponent<Button>(); }
    }

    private Image m_PauseImage = null;

    private Image PauseImage
    {
        get { return m_PauseImage = m_PauseImage ?? m_PauseButtonParent.GetComponent<Image>(); }
    }

    #endregion

    #region Game Over Screen

    [SerializeField] private GameObject m_GameOverScreen = null;

    private GameOverController m_GameOverController = null;

    private GameOverController GameOverController
    {
        get { return m_GameOverController = m_GameOverController ?? m_GameOverScreen.GetComponent<GameOverController>(); }
    }

    #endregion

    private int m_FinalScore = 0;

    public void OnEnable()
    {
        DeathFlash.OnFlashEnd += ShowGameOverScreen;  
    }

    public void OnDisable()
    {
        DeathFlash.OnFlashEnd -= ShowGameOverScreen;
    }

    public void Start()
    {
        //Activate the splash screen on game start
        m_SplashScreen.SetActive(true);
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
            this.DeathFlash.ShowDeathFlash();

            m_SplashScreen.SetActive(false);
            m_ScoreText.SetActive(false);
            m_PauseButtonParent.SetActive(false);

            m_HasFlashed = true;
            m_FinalScore = finalScore;
        }
    }

    private void ShowGameOverScreen()
    {
        this.GameOverController.Activate(m_FinalScore);
    }

}
