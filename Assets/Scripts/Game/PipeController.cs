using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{

    private GameObject m_Ground = null;
    private float m_GroundSpeed = 0f;

    //Mulitplier for ground scrolling speed to match the pipe up with the grounds scrolling
    //Allows for you to only have to modify the speed of the ground scrolling to also modify the pipe scrolling as well
    [SerializeField] private float speedMultiplier = 0f;


    // Start is called before the first frame update
    void Start()
    {
        m_GroundSpeed = m_Ground.GetComponent<Scrolling>().GetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalVariables.IsGameOver())
        {
            //Scroll the pipe across the screen
            Vector3 offset = new Vector3(-(m_GroundSpeed * speedMultiplier) * Time.deltaTime, 0, 0);
            gameObject.transform.position += offset;

            //Destroy the pipe once it's outside of the screen
            if (gameObject.transform.position.x < -2f)
            {
                Destroy(gameObject);
            }
        }
    }

    //Set the ground gameobject and scroll based on its scrolling speed
    public void SetGround(GameObject Ground)
    {
        m_Ground = Ground;
    }
}
