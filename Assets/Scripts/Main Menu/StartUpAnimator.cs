using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;

//This class handles the animation and mechanics of the start up animation when the game starts
public class StartUpAnimator : MonoBehaviour
{

    #region Screen Fader

    [SerializeField] private GameObject m_ScreenFaderObj = null;

    private Image m_ScreenFader = null;

    private Image ScreenFader 
    {
        get { return m_ScreenFader = m_ScreenFader ?? m_ScreenFaderObj.GetComponent<Image>(); }
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
        this.NokoriLogoFader.OnFadeInEnd += StartLogoDelayTimer;
    }

    public void OnDisable()
    {
        this.NokoriLogoFader.OnFadeInEnd -= StartLogoDelayTimer;
    }


    // Start is called before the first frame update
    private void Start()
    {
        this.NokoriLogoFader.StartFadeIn(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_StartLogoDelayTimer)
        {
            if (m_LogoDelayTimer < m_LogoDelayTimeInSeconds)
            {
                m_LogoDelayTimer += Time.deltaTime;
            }
            else
            {
                StartFadeOut();
                m_StartLogoDelayTimer = false;
            }
        }
    }

    private void StartLogoDelayTimer()
    {
        m_StartLogoDelayTimer = true;
        m_LogoDelayTimer = 0f;
    }

    private void StartFadeOut()
    {
        this.NokoriLogoFader.StartFadeOut(true);
    }
}
