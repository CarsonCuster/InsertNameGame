using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class ZachGameLogic : MonoBehaviour
{
    [Header("Game Settings")]
    public List<GameObject> healthObjs;
    public int healthSet;
    [HideInInspector] public int healthActive;
    [HideInInspector] public int healthBeforeChange;
    [SerializeField] Transform healthStartingTransform;
    [SerializeField] GameObject healthObj;
    public bool caseSensitive = false;
    public bool hardMode = false;
    public bool gameRunning = false;
    public TMP_Dropdown dropdown;
    [Header("Audio")]
    public AudioSource gameAudioSource;
    public AudioSource gameOverAudioSource;
    public AudioSource damageSFX;
    public AudioSource targetDestroyedSFX;
    public AudioClip genericDamage;
    public AudioClip genericTargetDestroyed;
    [Header("Menus")]
    [SerializeField] Canvas canvas;
    public GameObject gameOverMenu;
    public GameObject mainMenu;
    [SerializeField] Transform nameTransform;
    [SerializeField] GameObject namePrefab;
    public GameObject settingsMenu;
    public TMP_Text nameText;
    public TMP_Text scoreText;
    public Color skyboxColor1;
    public Color skyboxColor2;
    [SerializeField] Transform titleParent;
    private List<Toggle> toggles;
    [Header("Key Components")]
    public WoodSpawner woodSpawner;
    public TypingTargeter typingTargeter;
    public ThemeContainer currentTheme;
    public GameObject themeBehaviorHolder;
    public ThemeSpecificContent themeBehavior;
    [HideInInspector]
    public int playerScore;
    private IEnumerator currentSkyboxLerp;
    public GameObject background;
    [HideInInspector]
    public MeshRenderer backgroundRenderer;

    
    // Start is called before the first frame update
    void Start()
    {
        gameOverMenu.SetActive(false);
        mainMenu.SetActive(true);
        gameAudioSource.clip = currentTheme.gameMusic;
        gameOverAudioSource.clip = currentTheme.gameOverMusic;
        if(currentTheme.loopGameOverMusic) gameOverAudioSource.loop = true;
        else{
            gameOverAudioSource.loop = false;
        }
        gameAudioSource.Play();
        if(currentTheme.damageTaken != null)
        {
            damageSFX.clip = currentTheme.damageTaken;
        }
        else{
            damageSFX.clip = genericDamage;
        }
        if(currentTheme.destroySFX != null)
        {
            targetDestroyedSFX.clip = currentTheme.destroySFX;
        }
        else{
            targetDestroyedSFX.clip = genericTargetDestroyed;
        }
        backgroundRenderer = background.GetComponent<MeshRenderer>();
        //themeBehavior.OnThemeEnter();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBeforeChange != healthActive && healthActive >= 0)
        {
            healthBeforeChange = healthActive;
            GameObject woodToDelete = healthObjs[healthObjs.Count - 1];
            healthObjs.Remove(woodToDelete);
            Destroy(woodToDelete);
            themeBehavior.OnDamageTake();
        }
        if(healthActive == 0 && !gameOverMenu.activeInHierarchy && !mainMenu.activeInHierarchy && gameRunning)
        {
            themeBehavior.OnGameOver();
        }
        //silly haha time
        themeBehavior.UpdateThings();
    }

    public void RestartGame()
    {
        if(typingTargeter.activeWoodBlocks.Count >= 1)
        {
            foreach(WoodBehavior woodBlock in typingTargeter.activeWoodBlocks)
            {
                Destroy(woodBlock.gameObject.transform.parent.gameObject);
            }
            typingTargeter.activeWoodBlocks.Clear();
        }
        themeBehavior.Restart();
        gameRunning = true;
        woodSpawner.spawningEnabled = true;
        typingTargeter.typingOn = true;
        SetHealth(healthSet);
        UpdateScore(false);
    }

    public void StartGame()
    {
        SetHealth(healthSet);
        themeBehavior.StartGame();
        // audioSource.clip = currentTheme.gameMusic;
        // audioSource.Play();
        gameRunning = true;
        woodSpawner.spawningEnabled = true;
        typingTargeter.typingOn = true;
        mainMenu.SetActive(false);
        scoreText.gameObject.SetActive(true);
        UpdateScore(false);
    }

    public void SetHealth(int h)
    {
        float nextXPos = new float();
        healthBeforeChange = h;
        healthActive = h;
        for(int i = new int(); i < h; i++)
        {
            GameObject obj = Instantiate(healthObj, canvas.transform);
            obj.transform.position = healthStartingTransform.position + new Vector3(nextXPos, 0f,0f);
            nextXPos += healthStartingTransform.position.x + 15f;
            healthObjs.Add(obj);
        }
    }
    public void AddHealth(int h)
    {
        healthBeforeChange += h;
        healthActive += h;
        for(int i = new int(); i < h; i++)
        {
            GameObject obj = Instantiate(healthObj);
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            obj.transform.position = healthObjs[healthObjs.Count - 1].transform.position + new Vector3(57.30606f, 0f,0f);
            obj.transform.SetParent(canvas.transform);
            healthObjs.Add(obj);
        }
    }

    public void QuitGame()
    {
        
    }
    public void QuitToMainMenu()
    {
        if(typingTargeter.activeWoodBlocks.Count >= 1)
        {
            foreach(WoodBehavior woodBlock in typingTargeter.activeWoodBlocks)
            {
                Destroy(woodBlock.gameObject.transform.parent.gameObject);
            }
            typingTargeter.activeWoodBlocks.Clear();
        }
        themeBehavior.QuitToMain();
        gameRunning = false;
        woodSpawner.spawningEnabled = false;
        typingTargeter.typingOn = false;
        scoreText.gameObject.SetActive(false);
        UpdateScore(false);
    }
    public void SettingsMenu()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        settingsMenu.SetActive(true);
        themeBehavior.OnSettingsOpen();
    }

    public void UpdateScore(bool scored)
    {
        playerScore++;
        if(scored) 
        {   
            scoreText.text = $"{playerScore}";
            themeBehavior.OnScoreIncrease();
        }
        else{
            scoreText.text = $"{playerScore = 0}";
        }
    }

    public void UpdateTheme()
    {
        themeBehavior.OnThemeExit();
        string name;
        TMP_Text tMP_Text = null;
        name = dropdown.options[dropdown.value].text;
        ThemeContainer obj = Resources.Load<ThemeContainer>($"{name}/{name}ThemeContainer");;
        currentTheme = obj;
        themeBehavior = themeBehaviorHolder.GetComponent($"{name}Theme") as ThemeSpecificContent;
        GameObject gameObject = Instantiate(namePrefab, nameTransform.position, Quaternion.identity, titleParent);
        gameObject.transform.localRotation = Quaternion.Euler(0f,0f,Random.Range(-20f, 20f));
        tMP_Text = gameObject.GetComponent<TMP_Text>();
        tMP_Text.text = $"{name.ToUpper()}'S";
        tMP_Text.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
        themeBehavior.OnThemeEnter();
    }
    public void ToggleCaseSensitivity()
    {
        if(caseSensitive) caseSensitive = false;
        else{
            caseSensitive = true;
        }
    }

    public void ToggleHardMode()
    {
        if(hardMode) hardMode = false;
        else{
            hardMode = true;
        }
        // if(!caseSensitive) caseSensitive = true;
    }
    public void BackOutOfMenu()
    {
        if(settingsMenu.activeInHierarchy)
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
            themeBehavior.BackOutOfSettingsMenu();
        }
    }
    public void ResetBackground()
    {
        backgroundRenderer.material.SetFloat("_Blend1", 0f);
        backgroundRenderer.material.SetFloat("_Blend2", 0f);
        backgroundRenderer.material.SetFloat("_Blend3", 0f);
    }

    //Lerps skybox from current color to target color. Will interupt active coroutine for smoother transition
    public void BeginSkyboxLerp(Color end)
    {
        if(currentSkyboxLerp != null) StopCoroutine(currentSkyboxLerp);
        currentSkyboxLerp = LerpSkyBoxColor(RenderSettings.skybox.GetColor("_Tint"), end);
        StartCoroutine(currentSkyboxLerp);
    }

    IEnumerator LerpSkyBoxColor(Color startColor, Color endColor)
    {
        float progress = 0;
        float t = 1f;
        while(progress < t)
        {
            RenderSettings.skybox.SetColor("_Tint" , Color.Lerp(startColor, endColor, progress / t));
            progress += Time.deltaTime;
            yield return null;
        }
        RenderSettings.skybox.SetColor("_Tint", endColor);
    }
    public IEnumerator FadeBackground(string blendNum)
    {
        Material mat = background.GetComponent<MeshRenderer>().material;
        float progress = 0;
        float fadeTime = 3f;
        while(progress < fadeTime)
        {
            float fade = Mathf.Lerp(0, 1, progress / fadeTime);
            background.GetComponent<MeshRenderer>().material.SetFloat(blendNum, fade);
            progress += Time.deltaTime;
            yield return null;
        }
        background.GetComponent<MeshRenderer>().material.SetFloat(blendNum, 1);
    }
}
