using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverController : MonoBehaviour
{

    [SerializeField] private GameObject m_Medal = null;
    [SerializeField] private GameObject m_TextFinalScore = null;
    [SerializeField] private GameObject m_TextBestScore = null;

    public void Activate(int finalScore)
    {

        //Set up the game over screen to show the results of the session
        m_TextFinalScore.GetComponent<TextMeshProUGUI>().SetText(finalScore.ToString());
        m_TextBestScore.GetComponent<TextMeshProUGUI>().SetText(finalScore.ToString());

        //Score the points and give the appropriate medal
        DetermineMedal(finalScore);

        //Activate the game over screen
        this.gameObject.SetActive(true);
    }

    //This function will check the score and assign the correct Medal 
    private void DetermineMedal(int finalScore)
    {

    }
}
