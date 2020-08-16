using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    #region Points Text

    //Store the Points Text Object
    [SerializeField] private GameObject m_PointsText = null;

    //Then fetch the Text Mesh from the Text Object
    private TextMeshProUGUI m_PointsTextMesh = null;
    public TextMeshProUGUI PointsText
    {
        get { return m_PointsTextMesh = m_PointsTextMesh ?? m_PointsText.GetComponent<TextMeshProUGUI>(); }
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

    [SerializeField] private GameObject m_PauseButtonObject = null;

    private Button m_PauseButton = null;

    private Button PauseButton
    {
        get { return m_PauseButton = m_PauseButton ?? m_PauseButtonObject.GetComponent<Button>(); }
    }

    private Image m_PauseImage = null;

    private Image PauseImage
    {
        get { return m_PauseImage = m_PauseImage ?? m_PauseButtonObject.GetComponent<Image>(); }
    }

    #endregion

    public void OnEnable()
    {
        DeathFlash.OnFlashEnd += ShowGameOverScreen;        
    }

    public void OnDisable()
    {
        DeathFlash.OnFlashEnd -= ShowGameOverScreen;
    }

    //Updates the point text at the top of the screen with the current score
    public void UpdateTextCallback(int points)
    {
        PointsText.text = points.ToString();
    }

    public void PauseButtonCallback(bool isPaused)
    {
        if (isPaused)
        {
            PauseImage.sprite = m_SpritePlay;
        } else
        {
            PauseImage.sprite = m_SpritePause;
        }
    }

    //Calls various UI functions for when the game is over
    public void GameOverCallback()
    {
        if (!m_HasFlashed)
        {
            DeathFlash.ShowDeathFlash();
            PauseButton.interactable = false;
            m_HasFlashed = true;
        }
    }

    private void ShowGameOverScreen()
    {

    }

}
