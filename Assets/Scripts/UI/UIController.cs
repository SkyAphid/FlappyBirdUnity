using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject m_GameManager = null;
    [SerializeField] private GameObject m_PointsText = null;

    #region Game Controller

    private GameController m_GameController = null;
    private GameController GameController
    {
        get { return m_GameController = m_GameController ?? m_GameManager.GetComponent<GameController>(); }
    }

    #endregion

    #region Points Text Controller

    private PointsTextController m_PointsTextController = null;

    private PointsTextController PointsTextController
    {
        get { return m_PointsTextController = m_PointsTextController ?? m_PointsText.GetComponent<PointsTextController>(); }
    }

    #endregion

    public void OnEnable()
    {
        GameController.OnPipeCleared += PointsTextController.UpdateText;
    }

    public void OnDisable()
    {
        GameController.OnPipeCleared -= PointsTextController.UpdateText;
    }
}
