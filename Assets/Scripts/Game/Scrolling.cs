using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{

    private Vector2 m_Offset = new Vector2();
    [SerializeField] private float m_Speed = 0f;

    // Update is called once per frame
    void Update()
    {
        if (!GlobalVariables.IsGameOver())
        {
            float bOffsetX = m_Offset.x;
            float nOffsetX = m_Offset.x + m_Speed;
            m_Offset.x = Mathf.Lerp(bOffsetX, nOffsetX, Time.deltaTime);

            gameObject.GetComponent<Renderer>().material.mainTextureOffset = m_Offset;
        }
    }

    public float GetSpeed()
    {
        return m_Speed;
    }
}
