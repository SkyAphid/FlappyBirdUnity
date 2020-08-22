﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/**
 * This class handles the "Death Flash," its an animation that plays on collision with an object (death).
 * It will fade the screen from white and also shake the camera to add to the force of the impact
 */
public class ScreenFlash : MonoBehaviour
{

    [SerializeField] private Camera m_Camera = null;

    #region Flash Image 

    private Image m_Flash = null;

    private Image Flash
    {
        get { return m_Flash = m_Flash ?? GetComponent<Image>(); }
    }

    #endregion

    [SerializeField] private float m_FadeSpeed = 0f;

    private bool m_CallbacksEnabled = false;
    private bool m_ShakeScreen = false;

    public event Action OnFlashEnd = null;

    /// <summary>
    /// A configurable screen flash that can be used in multiple situations.
    /// Callbacks enabled mode will avoid calling any callbacks and simply flash the screen for aesthetic uses.
    /// Shake screen determines whether or not to shake the camera (simulating force)
    /// 
    /// </summary>
    /// <param name="callbacksEnabled"></param>
    /// <param name="shakeScreen"></param>
    public void ShowFlash(bool callbacksEnabled, bool shakeScreen)
    {
        m_CallbacksEnabled = callbacksEnabled;
        m_ShakeScreen = shakeScreen;

        this.gameObject.SetActive(true);
        StartCoroutine(Fader());
    }

    //Coroutine for fading the screen from white and shaking the camera
    IEnumerator Fader()
    {

        for (float alpha = 1f; alpha > 0f; alpha -= m_FadeSpeed * Time.deltaTime)
        {

            //Adjust opacity of flash
            Color c = Flash.color;
            c.a = alpha;
            Flash.color = c;

            //Screen shake
            if (m_ShakeScreen)
            {
                float ShakeIntensity = 0.005f;
                float ShakeX = Random.value > 0.5 ? -ShakeIntensity : ShakeIntensity;
                float ShakeY = Random.value > 0.5 ? -ShakeIntensity : ShakeIntensity;

                Vector3 shake = new Vector3(ShakeX, ShakeY, 0);
                m_Camera.transform.position += shake;
            }

            yield return null;
        }

        if (m_CallbacksEnabled)
        {
            this.OnFlashEnd.Invoke();
        }

        this.gameObject.SetActive(false);
    }
}