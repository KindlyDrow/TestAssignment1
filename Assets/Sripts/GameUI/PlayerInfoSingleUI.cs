using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameSingleUI;
    [SerializeField] private TextMeshProUGUI playerScoreSingleUI;
    [SerializeField] private Color playerColorSingleUI;

    public void SetPlayerInfo(string playerName, int score, int position)
    {
        if (position == 1) GetComponent<Image>().color = playerColorSingleUI;
        playerNameSingleUI.text = (position + ". " + playerName);
        playerScoreSingleUI.text = score.ToString();
    }
}
