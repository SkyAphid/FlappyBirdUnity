﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenuController : MonoBehaviour
{
    #region Input

    public InputMaster Input
    {
        get { return m_InputMaster = m_InputMaster ?? new InputMaster(); }
    }

    private InputMaster m_InputMaster = null;

    #endregion

    #region Background

    [SerializeField] private Material m_BackgroundMatDay = null;
    [SerializeField] private Material m_BackgroundMatNight = null;
    [SerializeField] private GameObject m_Background = null;

    private TextureScroller m_BackgroundScroller = null;

    private TextureScroller BackgroundScroller
    {
        get { return m_BackgroundScroller = m_BackgroundScroller ?? m_Background.GetComponent<TextureScroller>(); }
    }

    #endregion

    #region Ground

    [SerializeField] private GameObject m_Ground = null;

    private TextureScroller m_GroundScroller = null;

    private TextureScroller GroundScroller
    {
        get { return m_GroundScroller = m_GroundScroller ?? m_Ground.GetComponent<TextureScroller>(); }
    }
    #endregion

    //Game scroll speed, modifies the overall speed of the background
    [SerializeField] private float m_ScrollSpeed = 0f;

    private void OnEnable()
    {
        Input.Enable();
    }

    private void OnDisable()
    {
        Input.Disable();
    }

    private void Start()
    {
        if (Random.value > 0.5f)
        {
            m_Background.GetComponent<MeshRenderer>().material = m_BackgroundMatDay;
        }
        else
        {
            m_Background.GetComponent<MeshRenderer>().material = m_BackgroundMatNight;
        }
    }

    void Update()
    {
        this.BackgroundScroller.ManualUpdate(m_ScrollSpeed);
        this.GroundScroller.ManualUpdate(m_ScrollSpeed);
    }
}