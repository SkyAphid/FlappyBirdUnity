using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{

    #region Pipe Spawning

    [SerializeField] private GameObject m_PipeBlueprint = null;

    //The initial delay before pipes start spawning
    [SerializeField] private float m_PipeSpawnDelayInSeconds = 0f;

    //Pipe spawn rate in seconds (how often a new pipe is spawned)
    [SerializeField] private float m_PipeRespawnRateInSeconds = 0f;

    //Pipe spawn timer
    private float m_PipeSpawnTimer = 0f;

    private const float PIPE_MIN_OFFSET_Y = -1f;
    private const float PIPE_MAX_OFFSET_Y = 1f;

    private readonly List<GameObject> m_Pipes = new List<GameObject>();

    #endregion

    public void Start()
    {
        m_PipeSpawnTimer = m_PipeSpawnDelayInSeconds;
    }

    // Manual update function called from GameManager
    public void ManualUpdate(float scrollSpeed, float pipeSpeedMultiplier)
    {
        //Pipe Spawning
        if (m_PipeSpawnTimer > 0)
        {
            m_PipeSpawnTimer -= Time.deltaTime;
        }
        else
        {
            SpawnPipe();
            m_PipeSpawnTimer = m_PipeRespawnRateInSeconds;
        }

        //Pipe movement
        for (int i = 0; i < m_Pipes.Count; i++)
        {
            ScrollPipe(scrollSpeed, pipeSpeedMultiplier, m_Pipes[i]);
        }
    }

    private void SpawnPipe()
    {
        GameObject pipe = Instantiate(m_PipeBlueprint);
        pipe.transform.position = new Vector2(2, Random.Range(PIPE_MIN_OFFSET_Y, PIPE_MAX_OFFSET_Y));
        m_Pipes.Add(pipe);
    }

    private void ScrollPipe(float scrollSpeed, float pipeSpeedMultiplier, GameObject pipe)
    {
        //Scroll the pipe across the screen
        Vector3 offset = new Vector3(-(scrollSpeed * pipeSpeedMultiplier) * Time.deltaTime, 0, 0);
        pipe.transform.position += offset;

        //Destroy the pipe once it's outside of the screen
        if (pipe.transform.position.x < -2f)
        {
            m_Pipes.Remove(pipe);
            Destroy(pipe);
        }
    }
}
