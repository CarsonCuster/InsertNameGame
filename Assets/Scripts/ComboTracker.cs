using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboTracker : MonoBehaviour
{
    public GameObject comboObject;
    public GameObject comboText;
    public TMP_Text comboAmount;
    public Animator animator;
    public int currentCombo;
    private int comboLastFrame;
    private IEnumerator enumerator;
    private Rigidbody rigidbodyAmount;
    private Rigidbody rigidbodyText;
    private bool coroutineIsActive;
    private Vector3 startingPosComboAmount;
    private Vector3 startingPosComboText;
    // Start is called before the first frame update
    void Start()
    {
        enumerator = ComboBreak();
        startingPosComboAmount = comboAmount.gameObject.transform.position;
        startingPosComboText = comboText.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentCombo >= 10 && !comboObject.activeInHierarchy)
        {
            if(coroutineIsActive) StopCoroutine(enumerator);
            comboAmount.gameObject.transform.position = startingPosComboAmount;
            comboText.transform.position = startingPosComboText;
            comboObject.SetActive(true);
        }
        else if(currentCombo <= 0 && comboObject.activeInHierarchy)
        {
            StartCoroutine(enumerator);
        }

        if(comboLastFrame != currentCombo && currentCombo >= 10)
        {
            animator.Play("New State");
            animator.SetTrigger("comboScore");
            comboLastFrame = currentCombo;
            comboAmount.text = $"{currentCombo}";
        }
    }

    IEnumerator ComboBreak()
    {
        coroutineIsActive = true;
        Rigidbody comboAmountRigid = comboAmount.gameObject.GetComponent<Rigidbody>();
        Rigidbody comboTextRigid = comboText.GetComponent<Rigidbody>();
        comboAmountRigid.useGravity = true;
        comboTextRigid.useGravity = true;
        comboAmountRigid.AddForce(new Vector3(-100f, 200f, 0f));
        comboTextRigid.AddForce(new Vector3(100f, 200f, 0f));
        float progress = 0f;
        float rotTime = 3f;
        float rot;
        float ang = 30f;
        while(progress < rotTime)
        {
            rot = Mathf.Lerp(comboAmount.gameObject.transform.eulerAngles.z, ang, progress / rotTime);
            comboAmount.gameObject.transform.eulerAngles += new Vector3(0f, 0f, rot);
            comboText.transform.eulerAngles += new Vector3(0f, 0f, -rot);
            progress += Time.deltaTime;
            yield return null;
        }
        coroutineIsActive = false;
    }
}
