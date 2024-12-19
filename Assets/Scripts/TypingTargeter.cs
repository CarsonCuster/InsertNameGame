using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using UnityEngine;

public class TypingTargeter : MonoBehaviour
{
    public List<WoodBehavior> activeWoodBlocks;
    public ComboTracker comboTracker;
    private ZachGameLogic zachGameLogic;
    public GameObject targetIndicator;
    private WoodBehavior currentTarget;
    public char[] woodLetters;
    private int currentLetterPos = 0;
    private string compareString;
    public bool typingOn = true;
    public float timeToReachTarget;
    private bool followTarget = false;
    private bool isRunning = false;
    private IEnumerator currentCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        targetIndicator.SetActive(false);
        zachGameLogic = FindAnyObjectByType<ZachGameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) && currentTarget != null && activeWoodBlocks.Count > 1)
        {
            float smallestFloat = new float();
            GameObject closestObj = currentTarget.gameObject;
            foreach(WoodBehavior wood in activeWoodBlocks)
            {
                float x = wood.transform.position.x;
                if(wood.transform.position.x < currentTarget.transform.position.x)
                {
                    if(x > smallestFloat || smallestFloat == 0f)
                    {
                        smallestFloat = x;
                        closestObj = wood.gameObject;
                    }
                }
            }
            currentTarget.isTargeted = false;
            followTarget = false;
            SetTarget(closestObj.GetComponent<WoodBehavior>());
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) && currentTarget != null && activeWoodBlocks.Count > 1)
        {
            float smallestFloat = new float();
            GameObject closestObj = currentTarget.gameObject;
            foreach(WoodBehavior wood in activeWoodBlocks)
            {
                float x = wood.transform.position.x;
                if(x > currentTarget.transform.position.x)
                {
                    if(x < smallestFloat || smallestFloat == 0f)
                    {
                        smallestFloat = x;
                        closestObj = wood.gameObject;
                    }
                }
            }
            currentTarget.isTargeted = false;
            followTarget = false;
            SetTarget(closestObj.GetComponent<WoodBehavior>());
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && currentTarget != null && activeWoodBlocks.Count > 1)
        {
            StartCoroutine(FindClosestTarget());
        }
        if(currentTarget != null && typingOn)
        {
            if(currentLetterPos == woodLetters.Length)
            {
                CheckPlayerAccuracy();
            }
            foreach(char a in zachGameLogic.caseSensitive ? Input.inputString : Input.inputString.ToLower())
            {
                if(a == '\b')
                {
                    if(currentLetterPos != 0)
                    {
                        compareString = compareString.Substring(0, compareString.Length - 1);
                        currentLetterPos--;
                        ChangeCharColor(Color.white);
                    }
                }
                else if(a == '\r')
                {
                    compareString = "";
                    currentLetterPos = 0;
                    string refresh = currentTarget.text.text;
                    currentTarget.text.text = "Hi once again friends.";
                    currentTarget.text.text = refresh;
                }
                else if(a != woodLetters[currentLetterPos] && a != '\b' && currentLetterPos < woodLetters.Length)
                {
                    if(zachGameLogic.hardMode) return;
                    compareString += a;
                    ChangeCharColor(Color.red);
                    currentLetterPos++;
                    comboTracker.currentCombo = 0;
                    // Attempted to change to char at the position the player messed up at. Turns out being much more difficult than I thought it would be. Cutting for now

                    // char[] chars = currentTarget.text.text.ToCharArray();
                    // chars[currentLetterPos] = a;
                    // string updatedString = "";
                    // foreach(char b in chars)
                    // {
                    //     updatedString += b;
                    // }
                    //currentTarget.text.text = updatedString;
                }
                else if(a == woodLetters[currentLetterPos] && currentLetterPos < woodLetters.Length)
                {
                    compareString += a;
                    ChangeCharColor(Color.green);
                    comboTracker.currentCombo++;
                    currentLetterPos++;
                }
            }
        }
        if(followTarget && currentTarget != null)
        {
            targetIndicator.transform.position = currentTarget.transform.position;
            if(targetIndicator.transform.localScale != currentTarget.transform.parent.gameObject.transform.localScale)
            {
                targetIndicator.transform.localScale = currentTarget.transform.parent.gameObject.transform.localScale;
            }
        }
        if(currentTarget == null && targetIndicator.activeInHierarchy)
        {
            targetIndicator.SetActive(false);
        }

        if(currentTarget == null && typingOn && activeWoodBlocks.Count >= 1)
        {
            StartCoroutine(FindClosestTarget());
            //SetTarget(activeWoodBlocks[0]);
        }
        
        //Water Gun
    }
    IEnumerator FindClosestTarget()
    {
        float shortestDistance = 1000f;
        WoodBehavior closestObj = null;
        foreach(WoodBehavior wood in activeWoodBlocks)
        {
            if(closestObj == null || shortestDistance > wood.distanceToTarget)
            {
                closestObj = wood;
                shortestDistance = closestObj.distanceToTarget;
            }
        }
        SetTarget(closestObj);
        yield return null;
    }
    public void SetTarget(WoodBehavior wood)
    {
        if(isRunning && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            isRunning = false;
        }
        wood.isTargeted = true;
        if(currentTarget != null)
        {
            string refresh = currentTarget.text.text;
            currentTarget.text.text = "Hi friends, you will not see this string but sometimes nice things don't have to be seen. I love you all";
            currentTarget.text.text = refresh;
        }
        currentTarget = wood;
        woodLetters = zachGameLogic.caseSensitive ? currentTarget.text.text.ToCharArray() : currentTarget.text.text.ToLower().ToCharArray();
        if(activeWoodBlocks.Count > 1)
        {
            int index = activeWoodBlocks.IndexOf(currentTarget);
            activeWoodBlocks.RemoveAt(index);
            activeWoodBlocks.Insert(0, currentTarget);

        }
        currentLetterPos = 0;
        compareString = "";
        if(!targetIndicator.activeInHierarchy)
        {
            targetIndicator.transform.position = currentTarget.transform.position;
            targetIndicator.SetActive(true);
            followTarget = true;
            if(targetIndicator.transform.localScale != currentTarget.transform.parent.gameObject.transform.localScale)
            {
                targetIndicator.transform.localScale = currentTarget.transform.parent.gameObject.transform.localScale;
            }
        }
        else{
            targetIndicator.SetActive(true);
            currentCoroutine = MoveTargetIndicator(wood);
            StartCoroutine(currentCoroutine);
        }

    }

    void ChangeCharColor(Color color)
    {
        if(!currentTarget.text.textInfo.characterInfo[currentLetterPos].isVisible) return;
        int meshIndex = currentTarget.text.textInfo.characterInfo[currentLetterPos].materialReferenceIndex;
        int vertexIndex = currentTarget.text.textInfo.characterInfo[currentLetterPos].vertexIndex;
        Color32[] vertexColors = currentTarget.text.textInfo.meshInfo[meshIndex].colors32;
        vertexColors[vertexIndex + 0] = color;
        vertexColors[vertexIndex + 1] = color;
        vertexColors[vertexIndex + 2] = color;
        vertexColors[vertexIndex + 3] = color;
        currentTarget.text.UpdateVertexData(TMPro.TMP_VertexDataUpdateFlags.All);
    }

    void CheckPlayerAccuracy()
    {
        int letters = new int();
        char[] playerChars = compareString.ToCharArray();
        foreach(char c in woodLetters)
        {
            if(c == playerChars[letters])
            {
                // Continue looping if the letter matches
                letters++;
                continue;
            }
            else{
                // Break the loop and do not execute the rest of the function
                letters = 0;
                return;
            }
        }
        if(currentTarget.isBossTarget && currentTarget.bossHealth >= 1)
        {
            currentTarget.bossHealth--;
            Array.Clear(woodLetters, 0 ,woodLetters.Length);
            currentTarget.BossChangeType();
            compareString = "";
            currentLetterPos = 0;
            woodLetters = zachGameLogic.caseSensitive ? currentTarget.text.text.ToCharArray() : currentTarget.text.text.ToLower().ToCharArray();
            return;
        }
        // Player 100% accurate.
        //Destroy(currentTarget.transform.parent.gameObject);
        zachGameLogic.themeBehavior.OnTargetDestroy();
        StartCoroutine(currentTarget.DestroySelf());
        currentTarget = null;
        Array.Clear(woodLetters, 0 ,woodLetters.Length);
        compareString = "";
        currentLetterPos = 0;
        activeWoodBlocks.RemoveAt(0);
        followTarget = false;
        if(activeWoodBlocks.Count > 0) StartCoroutine(FindClosestTarget());
        zachGameLogic.UpdateScore(true);
    }

    // Controls the Target Indicator and moves it to the currently targeted object.
    IEnumerator MoveTargetIndicator(WoodBehavior wood)
    {
        isRunning = true;
        float progress = 0;
        while(progress < 0.5f)
        {
            targetIndicator.transform.position = Vector3.Lerp(targetIndicator.transform.position, wood.gameObject.transform.position, progress / 0.5f);
            targetIndicator.transform.localScale = Vector3.Lerp(targetIndicator.transform.localScale, wood.transform.parent.gameObject.transform.localScale, progress / 0.5f);
            progress += Time.deltaTime;
            yield return null;
        }
        targetIndicator.transform.position = wood.transform.position;
        followTarget = true;
        isRunning = false;
    }
}
