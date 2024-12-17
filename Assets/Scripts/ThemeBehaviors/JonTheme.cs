using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JonTheme : ThemeSpecificContent
{
    public Animator waterGun;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void UpdateThings()
    {
        base.UpdateThings();
        if(!waterGun.gameObject.activeInHierarchy && zachGameLogic.gameRunning )
        {
            waterGun.gameObject.SetActive(true);
        }
        else if(waterGun.gameObject.activeInHierarchy && !zachGameLogic.gameRunning)
        {
            waterGun.gameObject.SetActive(false);
        }
    }

    public override void OnTargetDestroy()
    {
        base.OnTargetDestroy();
        if(waterGun.gameObject.activeInHierarchy)
        {
            waterGun.SetTrigger("destroyedTarget");
        }
    }
    public override void OnGameOver()
    {
        base.OnGameOver();
    }
    public override void Restart()
    {
        base.Restart();
    }
    public override void OnThemeEnter()
    {
        base.OnThemeEnter();
    }
}
