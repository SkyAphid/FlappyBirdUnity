using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] private GameObject m_Bird = null;
    [SerializeField] private GameObject m_Background = null;
    [SerializeField] private GameObject m_Ground = null;
    [SerializeField] private GameObject m_PipeManager = null;

    private BirdController m_BirdController = null;
    private TextureScroller m_BackgroundScroller = null;
    private TextureScroller m_GroundScroller = null;
    private PipeController m_PipeController = null;

    private bool m_IsPaused = false;
    private bool m_IsGameOver = false;

    //The number of pipes the player has successfully passed through
    private int m_PipesCleared = 0;

    private void Awake()
    {
        //Cache various components
        m_BirdController = m_Bird.GetComponent<BirdController>();
        m_BackgroundScroller = m_Background.GetComponent<TextureScroller>();
        m_GroundScroller = m_Ground.GetComponent<TextureScroller>();
        m_PipeController = m_PipeManager.GetComponent<PipeController>();
    }

    void Update()
    {
        m_BirdController.ManualUpdate();

        if (!m_IsGameOver)
        {
            m_BackgroundScroller.ManualUpdate();
            m_GroundScroller.ManualUpdate();
            m_PipeController.ManualUpdate();
        }
    }

    private void BirdCollisionHandler(Collision2D collision)
    {
        m_IsGameOver = true;
    }

    private void BirdClearPipeHandler(Collider2D collider)
    {
        m_PipesCleared++;
    }

    private void OnEnable()
    {
        //Hook up action callbacks
        m_BirdController.OnCollision += BirdCollisionHandler;
        m_BirdController.OnClearPipe += BirdClearPipeHandler;
    }

    private void OnDisable()
    {
        m_BirdController.OnCollision -= BirdCollisionHandler;
        m_BirdController.OnClearPipe -= BirdClearPipeHandler;
    }
}