using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiddleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image reloadImage;
    private Player _player;

    private void Start()
    {
        StartCoroutine(WaitForPlayerInstance());
        reloadImage.fillAmount = 0f;
    }

    private IEnumerator WaitForPlayerInstance()
    {
        while (Player.LocalInstance == null)
        {
            yield return null;
        }

        Player.LocalInstance.OnBarrelCurrentChange += BulletFactory_OnShot;
        Player.LocalInstance.OnReload += BulletFactory_OnReload;
    }

    private void BulletFactory_OnReload(object sender, System.EventArgs e)
    {
        float reloadTime = Player.LocalInstance.GetReloadTime();
        
        StartCoroutine(StartReloadVisual(reloadTime));
    }

    private void BulletFactory_OnShot(object sender, System.EventArgs e)
    {
        ResetAmmoText();
    }

    private IEnumerator StartReloadVisual(float reloadTime)
    {
        reloadTime += .1f;
        float reloadTimeMax = reloadTime;
        reloadImage.fillAmount = 1f;
        
        while (reloadTime > 0)
        {
            reloadImage.fillAmount = reloadTime / reloadTimeMax;
            reloadTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        reloadImage.fillAmount = 0f;
        ResetAmmoText();
        yield return null;
    }

    private void ResetAmmoText()
    {
        ammoText.text = (Player.LocalInstance.GetBarrelCurrent() + "/" + Player.LocalInstance.GetBarrelMax());
    }
}
