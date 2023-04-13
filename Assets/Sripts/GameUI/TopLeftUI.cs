using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopLeftUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private bool waitingForPlayer;

    private void Start()
    {
        StartCoroutine(WaitForPlayerInstance());
    }

    private IEnumerator WaitForPlayerInstance()
    {
        while (Player.LocalInstance == null)
        {
            yield return null;
        }
        scoreText.text = ("SCORE: " + Player.LocalInstance.GetScore());
        Player.LocalInstance.OnScoreChange += Player_OnScoreChange;
    }

    private void Player_OnScoreChange(object sender, System.EventArgs e)
    {
        scoreText.text = ("SCORE: " + Player.LocalInstance.GetScore());
    }
}
