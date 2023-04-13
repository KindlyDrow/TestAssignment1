using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownTimer;

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance._state == GameManager.State.GamePreparing)
            {
                Show();
                countDownTimer.text = Mathf.Ceil(GameManager.Instance.timer.Value).ToString() + "!";
            }
            else
            {
                Hide();
            }
        } else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
