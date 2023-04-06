using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopLeftUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        Player.Instance.OnScoreChange += Player_OnScoreChange;
        scoreText.text = ("SCORE: " + Player.Instance.GetScore());
    }

    private void Player_OnScoreChange(object sender, System.EventArgs e)
    {
        scoreText.text = ("SCORE: " + Player.Instance.GetScore());
    }
}
