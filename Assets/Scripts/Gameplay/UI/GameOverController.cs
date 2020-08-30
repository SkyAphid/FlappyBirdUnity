using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{

    #region Serialized Fields

    [Header("General")]
    [SerializeField] private GameObject m_GameOverTitle = null;
    [SerializeField] private GameObject m_ResultWindow = null;
    [SerializeField] private GameObject m_MedalIcon = null;

    [Header("Score Text")]
    [SerializeField] private GameObject m_TextFinalScore = null;
    [SerializeField] private GameObject m_TextBestScore = null;

    [Header("Buttons")]
    [SerializeField] private GameObject m_PlayAgainButton = null;
    [SerializeField] private GameObject m_MainMenuButton = null;

    [Header("Medals")]
    [SerializeField] private Medal[] Medals = null;

    #endregion


    //This function activates the game over menu and is only intended to be called once per session
    public void Activate(int finalScore)
    {
        //Activate the game over screen
        this.gameObject.SetActive(true);

        //Set up the game over screen to show the results of the session
        this.m_TextFinalScore.GetComponent<TextMeshProUGUI>().SetText(finalScore.ToString());
        this.m_TextBestScore.GetComponent<TextMeshProUGUI>().SetText(finalScore.ToString());

        //Score the points and give the appropriate medal
        DetermineMedal(finalScore);

        //Animate the game over screen
        StartAnimation();
    }

    //This function will check the score and assign the correct Medal 
    private void DetermineMedal(int finalScore)
    {

        Image medalImage = m_MedalIcon.GetComponent<Image>();

        for (int i = 0; i < this.Medals.Length; i++)
        {
            if (this.Medals[i].MatchesMedal(finalScore))
            {
                medalImage.sprite = Medals[i].m_MedalSprite;
                return;
            }
        }

        //Set the sprite to null if your score isn't high enough to earn one
        m_MedalIcon.SetActive(false);
    }

    #region Animation


    [Button]
    private void StartAnimation()
    {
        //Disable the buttons until the animation is donee
        m_PlayAgainButton.SetActive(false);
        m_MainMenuButton.SetActive(false);

        //The result window isn't visible until after the game over title
        CanvasGroup resultCanvasGroup = m_ResultWindow.GetComponent<CanvasGroup>();
        resultCanvasGroup.alpha = 0f;

        //Start with the game over title
        m_GameOverTitle.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0f), 0.3f, 6, 1f)
            .OnComplete(() => AnimateResultWindow(resultCanvasGroup));

    }

    //Animates the result menu fade in
    private void AnimateResultWindow(CanvasGroup resultCanvasGroup)
    {

        float animationSpeed = 0.5f;

        Transform transform = m_ResultWindow.transform;

        //Scroll the result menu up
        float originalY = transform.position.y;
        transform.position += new Vector3(0, -10, 0);
        transform.DOMoveY(originalY, animationSpeed);

        AudioController.PlaySFX(AudioController.Sound.Swoosh);

        //Fade it in as it scrolls up for a nice effect
        resultCanvasGroup.DOFade(1f, animationSpeed).OnComplete(() => AnimationButtons());
    }

    //Animates the buttons popping in once the animation is concluded
    private void AnimationButtons()
    {
        m_PlayAgainButton.SetActive(true);

        m_PlayAgainButton.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 0.3f, 6, 1f)
            .OnComplete(() => {

                //Pop in the main menu button after the play again button
                m_MainMenuButton.SetActive(true);
                m_MainMenuButton.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 0.3f, 6, 1f);

            });

    }

    #endregion
}
