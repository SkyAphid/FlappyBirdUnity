using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{

    private Vector2 offset;
    public float speed = 0.5f;

    // Update is called once per frame
    void Update()
    {
        offset.x += speed * Time.deltaTime;
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = offset;
        Debug.Log(gameObject.GetComponent<Renderer>().material.mainTextureOffset);
    }
}
