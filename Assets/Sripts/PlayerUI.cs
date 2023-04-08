using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image currentHealth;
    [SerializeField] private Player _player;

    private void Start()
    {
        _player.OnDamageReceive += Player_OnDamageReceive;
    }

    private void Player_OnDamageReceive(object sender, System.EventArgs e)
    {
        float playerHealthMax;
        float playerHealthCur = _player.GetCurrentHealthOutMax(out playerHealthMax);
        currentHealth.fillAmount = playerHealthCur / playerHealthMax;
    }
}
