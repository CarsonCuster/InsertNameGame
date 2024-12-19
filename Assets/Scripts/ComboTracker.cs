using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboTracker : MonoBehaviour
{
    public GameObject comboObject;
    public GameObject comboText;
    public TMP_Text comboAmount;
    public Animator comboAmountanimator;
    public Animator comboTextAnimator;
    private Rigidbody comboAmountRigidbody;
    private Rigidbody comboTextRigidbody;
    public int currentCombo;
    private int comboLastFrame;
    private bool coroutineIsActive;
    private IEnumerator coroutine;
    private Vector3 startingPosComboAmount;
    private Vector3 startingPosComboText;
    // Start is called before the first frame update
    void Start()
    {
        startingPosComboAmount = comboAmount.gameObject.transform.position;
        startingPosComboText = comboText.transform.position;
        comboAmountRigidbody = comboAmount.gameObject.GetComponent<Rigidbody>();
        comboTextRigidbody = comboText.GetComponent<Rigidbody>();
        coroutine = ComboBreak();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentCombo >= 10 && !comboObject.activeInHierarchy)
        {
            if(coroutineIsActive)
            {
                StopCoroutine(coroutine);
                coroutineIsActive = false;
                coroutine = ComboBreak();
            }
            comboAmountRigidbody.useGravity = false;
            comboAmountRigidbody.velocity = Vector3.zero;
            comboTextRigidbody.velocity = Vector3.zero;
            comboTextRigidbody.useGravity = false;
            // comboAmountRigidbody.isKinematic = true;
            // comboTextRigidbody.isKinematic = true;
            comboAmount.gameObject.transform.eulerAngles = new Vector3(0,0,0);
            comboText.transform.eulerAngles = new Vector3(0,0,0);
            comboAmount.gameObject.transform.position = startingPosComboAmount;
            comboText.transform.position = startingPosComboText;
            coroutine = ComboBreak();
            comboObject.SetActive(true);
        }
        else if(currentCombo <= 0 && comboObject.activeInHierarchy && !coroutineIsActive)
        {
            comboAmountRigidbody.useGravity = true;
            comboTextRigidbody.useGravity = true;
            // comboAmountRigidbody.isKinematic = false;
            // comboTextRigidbody.isKinematic = false;
            comboAmountRigidbody.velocity = Vector3.zero;
            comboTextRigidbody.velocity = Vector3.zero;
            comboAmountRigidbody.AddForce(new Vector3(-2f, 20f, 0f));
            comboTextRigidbody.AddForce(new Vector3(2f, 20f, 0f));
            StartCoroutine(coroutine);
        }

        if(comboLastFrame != currentCombo)
        {  
            if(10 <= currentCombo && currentCombo < 50)
            {
                comboAmountanimator.Play("New State");
                comboAmountanimator.SetTrigger("comboScoreSmall");
            }
            else if(50 <= currentCombo && currentCombo < 100)
            {
                comboAmountanimator.Play("New State");
                comboAmountanimator.SetTrigger("comboScoreMedium");
            }
            else if(100 <= currentCombo && currentCombo < 200)
            {
                comboAmountanimator.Play("New State");
                comboAmountanimator.SetTrigger("comboScoreLarge");
            }
            else if(currentCombo >= 200)
            {
                comboAmountanimator.Play("New State");
                comboAmountanimator.SetTrigger("comboScoreMassive");
            }
            comboLastFrame = currentCombo;
            comboAmount.text = $"{currentCombo}";
        }
    }


    public void DisableComboObject()
    {
        comboObject.SetActive(false);
        comboTextAnimator.SetBool("transition", false);
    }

    IEnumerator ComboBreak()
    {
        Debug.Log("starting");
        coroutineIsActive = true;
        float progress = 0f;
        float rotTime = 3f;
        float rot;
        float ang = 20f;
        while(progress < rotTime)
        {
            rot = Mathf.Lerp(comboText.transform.rotation.z, ang, progress / rotTime);
            comboText.transform.eulerAngles += new Vector3(0f, 0f, -rot);
            comboAmount.transform.eulerAngles += new Vector3(0f, 0f, rot);
            progress += Time.deltaTime;
            yield return null;
        }
        comboAmountRigidbody.useGravity = false;
        comboTextRigidbody.useGravity = false;
        comboAmountRigidbody.velocity = Vector3.zero;
        comboTextRigidbody.velocity = Vector3.zero;
        // comboAmountRigidbody.isKinematic = true;
        // comboTextRigidbody.isKinematic = true;
        comboAmount.gameObject.transform.position = startingPosComboAmount;
        comboText.transform.position = startingPosComboText;
        comboAmount.gameObject.transform.eulerAngles = new Vector3(0,0,0);
        comboText.transform.eulerAngles = new Vector3(0,0,0);
        comboAmount.gameObject.transform.localScale = Vector3.one;
        comboObject.SetActive(false);
        coroutineIsActive = false;
    }
}
