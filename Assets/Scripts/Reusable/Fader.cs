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

    [SerializeField] private float m_FadeTime = 1f;

    public event Action OnFadeOutEnd = null;
    public event Action OnFadeInEnd = null;

    public void SetOpacity(float alpha)
    {
        Color c = Flash.color;
        c.a = alpha;
        Flash.color = c;
    }

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

        float progress = 0f;

        do
        {
            float normalizedProgress = (progress / m_FadeTime);
            float alpha = Mathf.Lerp(0f, 1f, normalizedProgress);

            //Adjust opacity of flash
            Color c = this.Flash.color;
            c.a = alpha;
            this.Flash.color = c;

            progress += Time.deltaTime;

            yield return null;
        } while (progress < m_FadeTime);

        //Snap color on end
        Color c2 = this.Flash.color;
        c2.a = 1f;
        this.Flash.color = c2;

        //Call event handler for fade out end
        if (this.OnFadeInEnd != null)
        {
            this.OnFadeInEnd.Invoke();
        }
    }

    IEnumerator FadeOut()
    {

        float progress = 0f;

        do
        {
            float normalizedProgress = progress / m_FadeTime;
            float alpha = Mathf.Lerp(1f, 0f, normalizedProgress);

            //Adjust opacity of flash
            Color c = this.Flash.color;
            c.a = alpha;
            this.Flash.color = c;

            progress += Time.deltaTime;

            yield return null;
        } while (progress < m_FadeTime);

        //Snap color on end
        Color c2 = this.Flash.color;
        c2.a = 0f;
        this.Flash.color = c2;

        //Call event handler for fade out end
        if (this.OnFadeOutEnd != null)
        {
            this.OnFadeOutEnd.Invoke();
        }
    }
}

