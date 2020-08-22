using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Medal", menuName = "New Medal", order = 1)]
public class Medal : ScriptableObject
{
    public int m_MinScoreRequirement = 0;
    public int m_MaxScoreRequirement = 10;
    public Sprite m_MedalSprite = null;

    public bool MatchesMedal(int finalScore)
    {
        if (finalScore >= m_MinScoreRequirement && (finalScore < m_MaxScoreRequirement || finalScore == -1))
        {
            return true;
        }

        return false;
    }
}
