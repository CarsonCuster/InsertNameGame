using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class WoodBehavior : MonoBehaviour
{
    [SerializeField]
    TypingTargeter typingTargeter;
    [SerializeField] ZachGameLogic zachGameLogic;
    public TMP_Text text;
    [Header("Game Settings")]
    public float speed;
    public bool isTargeted;
    public bool isBossTarget;
    public int bossHealth = 2;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Transform targetDestination;
    public float waitBeforeMovement = 0;
    [Tooltip("Used for both game and menu travel time")]
    public float timeToReachPlayer;
    public Animator animator;
    public GameObject parentObject;
    private bool isClose = true;
    public bool isGameWood = true;
    [Header("Menu Settings")]
    public bool isTitleText = false;
    [HideInInspector]
    public float distanceToTarget;
    private IEnumerator coroutine;
    public ParticleSystem particleSystem;

    void Awake()
    {
        gameObject.transform.localScale = new Vector3(0,0,0);
    }
    // Start is called before the first frame update
    void Start()
    {
        typingTargeter = FindObjectOfType<TypingTargeter>();
        zachGameLogic = FindObjectOfType<ZachGameLogic>();
        typingTargeter.activeWoodBlocks.Add(this);
        if(!animator) animator = GetComponent<Animator>();
        //text.text = typesOfWood.ToString();
        if(isGameWood) coroutine = MoveTowardsTarget(typingTargeter.gameObject.transform.position);
        else{
            coroutine = MoveTowardsTarget(targetDestination.position);
        }
        StartCoroutine(coroutine);
        //typingTargeter.CheckList();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameWood) distanceToTarget = Vector3.Distance(typingTargeter.transform.position, transform.position);
        else{
            distanceToTarget = Vector3.Distance(targetDestination.position, transform.position);
        }
        //Debug.Log(Vector3.Distance(typingTargeter.transform.position, transform.position));
        if(isGameWood && distanceToTarget < 1.0f && isClose)
        {
            typingTargeter.activeWoodBlocks.Remove(this);
            Destroy(gameObject.transform.parent.gameObject);
            zachGameLogic.healthActive--;
            //typingTargeter
            isClose = false;
        }
    }

    IEnumerator MoveTowardsTarget(Vector3 pos)
    {
        yield return new WaitForSeconds(waitBeforeMovement);
        float progress = 0;
        while(progress < timeToReachPlayer)
        {
            parentObject.transform.position = Vector3.Lerp(parentObject.transform.position, pos, progress / timeToReachPlayer * zachGameLogic.currentTheme.speedModifier);
            progress += Time.deltaTime;
            yield return null;
        }
    }

    public void MakeTextTransparent()
    {
        text.color = Color.clear;
    }

    public void BossChangeType()
    {
        meshRenderer.material = zachGameLogic.currentTheme.materials[UnityEngine.Random.Range(0, zachGameLogic.currentTheme.materials.Count)];
        text.text = meshRenderer.material.name.Replace("(Instance)", "");
        text.text = text.text.Substring(0, text.text.Length - 1);
        if(zachGameLogic.hardMode) MakeTextTransparent();
    }

    public IEnumerator DestroySelf()
    {
        Destroy(this.meshFilter);
        StopCoroutine(coroutine);
        particleSystem.gameObject.SetActive(true);
        GameObject canvas = text.gameObject.transform.parent.gameObject;
        Vector3 globalPosition = canvas.transform.position;
        canvas.transform.SetParent(null);
        canvas.transform.position = globalPosition;
        Rigidbody textR = canvas.GetComponent<Rigidbody>();
        textR.useGravity = true;
        textR.AddForce(new Vector3(0f, 200f, 0f));
        StartCoroutine(RotateObject(canvas));
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject.transform.parent.gameObject);
        Destroy(canvas);
    }

    IEnumerator RotateObject(GameObject gameObject)
    {
        float progress = 0f;
        float rotTime = 3f;
        float rot;
        float ang = 30f;
        bool rotateLeft = UnityEngine.Random.Range(0, 2) == 0 ? true : false;
        if(rotateLeft) ang = -ang;
        while(progress < rotTime)
        {
            rot = Mathf.Lerp(gameObject.transform.rotation.z, ang, progress / rotTime);
            gameObject.transform.eulerAngles += new Vector3(0f, 0f, rot);
            progress += Time.deltaTime;
            yield return null;
        }
    }
    
}
