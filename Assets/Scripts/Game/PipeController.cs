using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{

    #region Pipe Movement

    [SerializeField] private GameObject m_Ground = null;
    private float m_GroundSpeed = 0f;

    //Mulitplier for ground scrolling speed to match the pipe up with the grounds scrolling
    //Allows for you to only have to modify the speed of the ground scrolling to also modify the pipe scrolling as well
    [SerializeField] private float speedMultiplier = 0f;

    #endregion

    #region Pipe Spawning

    [SerializeField] private GameObject m_PipeBlueprint = null;

    //Pipe spawn in seconds
    private const float PIPE_SPAWN_TIMER = 3f;
    private float m_PipeSpawnTimer = PIPE_SPAWN_TIMER;

    private const float PIPE_MIN_OFFSET_Y = -1f;
    private const float PIPE_MAX_OFFSET_Y = 1f;

    private readonly List<GameObject> m_Pipes = new List<GameObject>();

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        m_GroundSpeed = m_Ground.GetComponent<TextureScroller>().Speed;
    }

    // Manual update function called from GameManager
    public void ManualUpdate()
    {
        //Pipe Spawning
        if (m_PipeSpawnTimer > 0)
        {
            m_PipeSpawnTimer -= Time.deltaTime;
        }
        else
        {
            SpawnPipe();
            m_PipeSpawnTimer = PIPE_SPAWN_TIMER;
        }

        //Pipe movement
        for (int i = 0; i < m_Pipes.Count; i++)
        {
            ScrollPipe(m_Pipes[i]);
        }
    }

    //Set the ground gameobject and scroll based on its scrolling speed
    public void SetGround(GameObject Ground)
    {
        m_Ground = Ground;
    }

    private void SpawnPipe()
    {
        GameObject pipe = Instantiate(m_PipeBlueprint);
        pipe.transform.position = new Vector2(2, Random.Range(PIPE_MIN_OFFSET_Y, PIPE_MAX_OFFSET_Y));
        m_Pipes.Add(pipe);
    }

    private void ScrollPipe(GameObject pipe)
    {
        //Scroll the pipe across the screen
        Vector3 offset = new Vector3(-(m_GroundSpeed * speedMultiplier) * Time.deltaTime, 0, 0);
        pipe.transform.position += offset;

        //Destroy the pipe once it's outside of the screen
        if (pipe.transform.position.x < -2f)
        {
            Destroy(pipe);
            m_Pipes.Remove(pipe);
        }
    }
}
