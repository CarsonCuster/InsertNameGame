using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSpecificContent : MonoBehaviour
{
    [HideInInspector]
    public ZachGameLogic zachGameLogic;
    [HideInInspector]
    public TypingTargeter typingTargeter;
    [HideInInspector] public bool hasThemeGameOver;
    private AudioClip damageTakenSFX;
    private AudioClip targetShotSFX;
    public ThemeContainer themeContainer;
    // Start is called before the first frame update
    public virtual void Start()
    {
        zachGameLogic = FindObjectOfType<ZachGameLogic>();
        typingTargeter = FindObjectOfType<TypingTargeter>();
        if(themeContainer.damageTaken != null)damageTakenSFX = themeContainer.damageTaken;
        if(themeContainer.destroySFX != null)targetShotSFX = themeContainer.destroySFX;
        hasThemeGameOver = themeContainer.hasThemeSpecificGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void StartGame()
    {
        zachGameLogic.background.SetActive(true);
        //zachGameLogic.ResetBackground();
    }
    public virtual void Restart()
    {  
        zachGameLogic.gameOverMenu.SetActive(false);
        zachGameLogic.gameOverAudioSource.Stop();
        zachGameLogic.gameAudioSource.Play();
        zachGameLogic.ResetBackground();
    }
    public virtual void QuitToMain()
    {
        zachGameLogic.gameOverMenu.SetActive(false);
        zachGameLogic.gameOverAudioSource.Stop();
        zachGameLogic.gameAudioSource.Play();
        zachGameLogic.mainMenu.SetActive(true);
        zachGameLogic.ResetBackground();
        zachGameLogic.background.SetActive(false);
    }
    public virtual void UpdateThings(){}
    public virtual void OnGameOver()
    {
        zachGameLogic.gameOverMenu.SetActive(true);
        zachGameLogic.typingTargeter.typingOn = false;
        zachGameLogic.woodSpawner.spawningEnabled = false;
        zachGameLogic.gameOverAudioSource.Play();
        zachGameLogic.gameAudioSource.Pause();
        zachGameLogic.gameRunning = false;
    }
    public virtual void OnThemeExit(){}
    public virtual void OnThemeEnter()
    {  
        zachGameLogic.gameAudioSource.clip = themeContainer.gameMusic;
        zachGameLogic.gameAudioSource.Play();
        if(themeContainer.loopGameOverMusic) zachGameLogic.gameOverAudioSource.loop = true;
        else{
            zachGameLogic.gameOverAudioSource.loop = false;
        }
        zachGameLogic.gameOverAudioSource.clip = themeContainer.gameOverMusic;
        if(damageTakenSFX != null) zachGameLogic.damageSFX.clip = damageTakenSFX;
        else{
            zachGameLogic.damageSFX.clip = zachGameLogic.genericDamage;
        }
        if(targetShotSFX != null) zachGameLogic.targetDestroyedSFX.clip = targetShotSFX;
        else{
            zachGameLogic.targetDestroyedSFX.clip = zachGameLogic.genericTargetDestroyed;
        }
        if(themeContainer.background1 != null)
        {
            zachGameLogic.backgroundRenderer.material.SetTexture("_MainTex", themeContainer.background1);
            zachGameLogic.backgroundRenderer.material.SetTexture("_Texture2", themeContainer.background2);
            zachGameLogic.backgroundRenderer.material.SetTexture("_Texture3", themeContainer.background3);
            zachGameLogic.backgroundRenderer.material.SetTexture("_Texture4", themeContainer.background4);
        }
    }
    public virtual void OnTargetDestroy()
    {
        zachGameLogic.targetDestroyedSFX.Play();
    }
    public virtual void OnScoreIncrease()
    {
        if(zachGameLogic.playerScore == 20f)
        {
            StartCoroutine(zachGameLogic.FadeBackground("_Blend1"));
        }
        if(zachGameLogic.playerScore == 50f)
        {
            StartCoroutine(zachGameLogic.FadeBackground("_Blend2"));
        }
        if(zachGameLogic.playerScore == 100f)
        {
            StartCoroutine(zachGameLogic.FadeBackground("_Blend3"));
        }
    }
    public virtual void OnDamageTake()
    {
        zachGameLogic.damageSFX.Play();
    }
    public virtual void BackOutOfSettingsMenu()
    {
        zachGameLogic.BeginSkyboxLerp(zachGameLogic.skyboxColor1);
    }
    public virtual void OnSettingsOpen()
    {
        zachGameLogic.BeginSkyboxLerp(zachGameLogic.skyboxColor2);
    }
}
