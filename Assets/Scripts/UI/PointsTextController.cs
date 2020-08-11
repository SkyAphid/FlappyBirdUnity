using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointsTextController : MonoBehaviour
{

    private TextMeshProUGUI m_PointsText;
    private TextMeshProUGUI PointsText
    {
        get { return m_PointsText = m_PointsText ?? GetComponent<TextMeshProUGUI>(); }
    }

    public void UpdateText(int points)
    {
        PointsText.text = points.ToString();
    }
}
