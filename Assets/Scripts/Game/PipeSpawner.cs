using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{

    [SerializeField] private GameObject m_Ground = null;
    [SerializeField] private GameObject m_PipeBlueprint = null;


    //Pipe spawn in seconds
    private const float PIPE_SPAWN_TIMER = 3f;
    private float m_PipeSpawnTimer = PIPE_SPAWN_TIMER;

    private const float PIPE_MIN_OFFSET_Y = -1f;
    private const float PIPE_MAX_OFFSET_Y = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy the blueprint and instantiate new ones randomly based on a spawn timer
        //m_PipeBlueprint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Spawn new pipes every X amount of seconds
        if (!GlobalVariables.IsGameOver())
        {
            if (m_PipeSpawnTimer > 0)
            {
                m_PipeSpawnTimer -= Time.deltaTime;
            }
            else
            {
                SpawnPipe();
                m_PipeSpawnTimer = PIPE_SPAWN_TIMER;
            }
        }
    }

    private void SpawnPipe()
    {
        GameObject pipe = Instantiate(m_PipeBlueprint);
        pipe.GetComponent<PipeController>().SetGround(m_Ground);
        pipe.transform.position = new Vector2(2, Random.Range(PIPE_MIN_OFFSET_Y, PIPE_MAX_OFFSET_Y));
    }
}
