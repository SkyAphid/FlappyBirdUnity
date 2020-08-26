using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;

//This class handles the animation and mechanics of the start up animation when the game starts
public class StartUpAnimator : MonoBehaviour
{

    [SerializeField] private GameObject m_MainMenu = null;

    #region Screen Fader

    [SerializeField] private GameObject m_ScreenFaderObj = null;

    private Fader m_ScreenFader = null;

    private Fader ScreenFader 
    {
        get { return m_ScreenFader = m_ScreenFader ?? m_ScreenFaderObj.GetComponent<Fader>(); }
    }

    #endregion

    #region Nokori Logo

    [SerializeField] private GameObject m_NokoriLogoObj = null;

    private Fader m_NokoriLogoFader = null;

    private Fader NokoriLogoFader
    {
        get { return m_NokoriLogoFader = m_NokoriLogoFader ?? m_NokoriLogoObj.GetComponent<Fader>(); }
    }

    #endregion

    [SerializeField] private float m_LogoDelayTimeInSeconds = 1f;
    private bool m_StartLogoDelayTimer = false;
    private float m_LogoDelayTimer = 0f;

    public void OnEnable()
    {
        //When the fade in completes, wait a moment and then start the fade out
        this.NokoriLogoFader.OnFadeInEnd += StartLogoDelayTimer;
        this.NokoriLogoFader.OnFadeOutEnd += StartFadeIntoMainMenu;

        this.ScreenFader.OnFadeOutEnd += DisableStartUpScreen;
    }

    public void OnDisable()
    {
        this.NokoriLogoFader.OnFadeInEnd -= StartLogoDelayTimer;
        this.NokoriLogoFader.OnFadeOutEnd -= StartFadeIntoMainMenu;

        this.ScreenFader.OnFadeOutEnd -= DisableStartUpScreen;
    }


    private void Start()
    {
        //Start the nokori logo fader when the game starts (fade in slowly)
        this.NokoriLogoFader.StartFadeIn(true);
    }

    private void Update()
    {
        //This timer controls how long the logo stays on the screen before it fades back out
        if (m_StartLogoDelayTimer)
        {
            if (m_LogoDelayTimer < m_LogoDelayTimeInSeconds)
            {
                m_LogoDelayTimer += Time.deltaTime;
            }
            else
            {
                //Timer completed: start fade out
                StartLogoFadeOut();
                m_StartLogoDelayTimer = false;
            }
        }
    }

    //Begins the timer before we fade the logo back out
    private void StartLogoDelayTimer()
    {
        m_StartLogoDelayTimer = true;
        m_LogoDelayTimer = 0f;
    }

    //Begin fading the logo out
    private void StartLogoFadeOut()
    {
        this.NokoriLogoFader.StartFadeOut(true);
    }

    //Fade out the logo display screen and fade into the main menu
    private void StartFadeIntoMainMenu()
    {
        ScreenFader.StartFadeOut(true);
        m_MainMenu.SetActive(true);
    }

    private void DisableStartUpScreen()
    {
        this.gameObject.SetActive(false);
    }
}
