using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image currentHealth;

    private void Start()
    {
        Player.Instance.OnDamageReceive += Player_OnDamageReceive;
    }

    private void Player_OnDamageReceive(object sender, System.EventArgs e)
    {
        float playerHealthMax;
        float playerHealthCur = Player.Instance.GetCurrentHealthOutMax(out playerHealthMax);
        currentHealth.fillAmount = playerHealthCur / playerHealthMax;
    }
}
