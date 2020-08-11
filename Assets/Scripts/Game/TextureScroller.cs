using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroller : MonoBehaviour
{

    private Renderer m_Renderer;

    private Vector2 m_Offset = new Vector2();

    private void Start()
    {
        m_Renderer = gameObject.GetComponent<Renderer>();
    }

    // Manual update function called from GameManager
    public void ManualUpdate(float scrollSpeed)
    {
        //Scroll the texture on the material left to simulate horizontal movement
        float bOffsetX = m_Offset.x;
        float nOffsetX = m_Offset.x + scrollSpeed;
        m_Offset.x = Mathf.Lerp(bOffsetX, nOffsetX, Time.deltaTime);

        m_Renderer.material.mainTextureOffset = m_Offset;
    }
}
