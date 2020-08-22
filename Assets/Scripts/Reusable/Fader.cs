using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    #region Fader Image 

    private Image m_Flash = null;

    private Image Flash
    {
        get { return m_Flash = m_Flash ?? GetComponent<Image>(); }
    }

    #endregion

    [SerializeField] private float m_FadeSpeed = 0f;

    public event Action OnFadeOutEnd = null;
    public event Action OnFadeInEnd = null;

    /// <summary>
    /// This function will cause the parent object to fade its alpha in and call a callback when it's completed.
    /// </summary>
    public void StartFadeIn(bool resetColor)
    {

        if (resetColor)
        {
            Color c = Flash.color;
            c.a = 0f;
            Flash.color = c;
        }

        this.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// This function will cause the parent object to fade its alpha out and call a callback when it's completed.
    /// </summary>
    public void StartFadeOut(bool resetColor)
    {
        if (resetColor)
        {
            Color c = Flash.color;
            c.a = 1f;
            Flash.color = c;
        }

        this.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {

        for (float alpha = 0f; alpha < 1f; alpha += m_FadeSpeed * Time.deltaTime)
        {

            //Adjust opacity of flash
            Color c = this.Flash.color;
            c.a = alpha;
            this.Flash.color = c;

            yield return null;
        }

        if (this.OnFadeInEnd != null)
        {
            this.OnFadeInEnd.Invoke();
        }
    }

    IEnumerator FadeOut()
    {

        for (float alpha = 1f; alpha > 0f; alpha -= m_FadeSpeed * Time.deltaTime)
        {

            //Adjust opacity of flash
            Color c = this.Flash.color;
            c.a = alpha;
            this.Flash.color = c;

            yield return null;
        }

        if (this.OnFadeOutEnd != null)
        {
            this.OnFadeOutEnd.Invoke();
        }
    }
}

