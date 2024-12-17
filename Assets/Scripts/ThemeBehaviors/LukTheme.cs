using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LukTheme : ThemeSpecificContent
{
    [SerializeField] GameObject lukJoke;
    [SerializeField] GameObject lukGameOverMenu;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void UpdateThings()
    {
        base.UpdateThings();
        if(!lukJoke.activeInHierarchy && zachGameLogic.dropdown.options[zachGameLogic.dropdown.value].text == "Luk")
        {
            lukJoke.SetActive(true);
        }
        else if(zachGameLogic.dropdown.options[zachGameLogic.dropdown.value].text != "Luk" && lukJoke.activeInHierarchy)
        {
            lukJoke.SetActive(false);
        }
    }
    public override void Restart()
    {
        base.Restart();
        lukGameOverMenu.SetActive(false);
    }

    public override void QuitToMain()
    {
        base.QuitToMain();
        lukGameOverMenu.SetActive(false);
    }

    IEnumerator MissionFailedGameOver()
    {
        lukGameOverMenu.SetActive(true);
        List<GameObject> menuItems = new List<GameObject>();
        foreach (Transform child in lukGameOverMenu.gameObject.transform)
        {
            menuItems.Add(child.gameObject);
            if(child.gameObject != menuItems[0]) child.gameObject.SetActive(false);
        }
        float progress = 0f;
        float timeToFade = 5f;
        float alpha = 0;
        RawImage background = menuItems[0].GetComponent<RawImage>();
        while(progress < timeToFade)
        {
            alpha = Mathf.Lerp(0f, 1f, progress / timeToFade);
            if(progress >= 2.2f && !menuItems[1].activeInHierarchy)
            {
                menuItems[1].SetActive(true);
            }
            background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
            progress += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        menuItems[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        menuItems[3].SetActive(true);
    }

    public override void OnGameOver()
    {
        StartCoroutine(MissionFailedGameOver());
        zachGameLogic.typingTargeter.typingOn = false;
        zachGameLogic.woodSpawner.spawningEnabled = false;
        zachGameLogic.gameOverAudioSource.Play();
        zachGameLogic.gameAudioSource.Pause();
        zachGameLogic.gameRunning = false;
    }
    public override void OnThemeExit()
    {
        base.OnThemeExit();
        lukJoke.SetActive(false);
    }
    public override void OnThemeEnter()
    {
        base.OnThemeEnter();
    }
}
