using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{

    #region Splash Image

    private Image m_SplashImage = null;

    private Image SplashImage
    {
        get { return m_SplashImage = m_SplashImage ?? gameObject.GetComponent<Image>(); }
    }

    #endregion 

    [SerializeField] private float m_FadeDelayTime = 2f;
    [SerializeField] private float m_FadeTime = 1f;

    private bool m_FadeStart = false;

    private void Update()
    {
        m_FadeDelayTime -= Time.deltaTime;

        if (m_FadeDelayTime < 0 && !m_FadeStart)
        {
            StartCoroutine("Fader");
            m_FadeStart = true;
        }
    }

    IEnumerator Fader()
    {
        float progress = 0f;

        do
        {

            float normalizedProgress = (progress / m_FadeTime);
            float alpha = Mathf.Lerp(1, 0, normalizedProgress);

            Color c = SplashImage.color;
            c.a = alpha;
            SplashImage.color = c;

            progress += Time.deltaTime;

            yield return null;

        } while (progress < m_FadeTime);

        gameObject.SetActive(false);
    }
}
