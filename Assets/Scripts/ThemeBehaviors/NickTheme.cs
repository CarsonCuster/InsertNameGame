using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NickTheme : ThemeSpecificContent
{
    public GameObject nickJoke;
    public Material skybox;
    private Material oldSkybox;
    public TMP_Text reasonForDeathText;
    public GameObject cam;
    public float camStartingZRot;
    public GameObject gameOverMenu;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        camStartingZRot = cam.transform.rotation.z;
    }

    // Update is called once per frame
    public override void UpdateThings()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.4f);
    }

    public override void OnDamageTake()
    {
        base.OnDamageTake();
    }
    public override void StartGame(){}

    public override void OnGameOver()
    {
        gameOverMenu.SetActive(true);
        zachGameLogic.typingTargeter.typingOn = false;
        zachGameLogic.woodSpawner.spawningEnabled = false;
        zachGameLogic.gameRunning = false;
        //StartCoroutine(cameraRoll());
    }
    public override void Restart()
    {
        gameOverMenu.SetActive(false);
        //cam.transform.eulerAngles = new Vector3(0f,0f, camStartingZRot);
    }
    public override void OnThemeEnter()
    {
        nickJoke.SetActive(true);
        oldSkybox = RenderSettings.skybox;
        RenderSettings.skybox = skybox;
        base.OnThemeEnter();
    }
    public override void OnThemeExit()
    {
        RenderSettings.skybox = oldSkybox;
        base.OnThemeExit();
        nickJoke.SetActive(false);
    }

    public override void BackOutOfSettingsMenu()
    {
        
    }
    public override void OnSettingsOpen()
    {

    }

    IEnumerator cameraRoll()
    {
        float progress = 0f;
        float timeToRoll = 2f;
        float zRot = 0f;
        while(progress < timeToRoll)
        {
            zRot = Mathf.Lerp(zRot, 10f, progress / timeToRoll);
            cam.transform.eulerAngles = new Vector3(0, 0, zRot);
            progress += Time.deltaTime;
            yield return null;
        }
    }
    public override void QuitToMain()
    {
        base.QuitToMain();
        gameOverMenu.SetActive(false);
        //cam.transform.eulerAngles = new Vector3(0f,0f, camStartingZRot);
    }
}
