using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private TextMeshProUGUI recordScoreText;

    void Start()
    {
        //UpdateRecordScore();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //void UpdateRecordScore()
    //{
    //    int recordScore = PlayerPrefs.GetInt("RecordScore", 0);
    //    recordScoreText.text = "Record Score: " + recordScore;
    //}
}
