using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    //public List<GameObject> woodObjects;
    public GameObject targetPrefab;
    public ZachGameLogic zachGameLogic;
    public Camera playerCamera;
    public float timeBetweenSpawns;
    public bool isAbleToSpawnBosses = false;
    public float chanceToSpawnBoss;
    public float chanceToSpawn2;
    public float chanceToSpawn3;
    private float time;
    private float totalTime;
    public bool spawningEnabled = true;
    public bool isGameSpawner = true;
    public float xSpawnRange;
    public float ySpawnRange;
    private bool lastSpawn3 = false;
    private bool lastSpawn2 = false;
    private bool lastSpawnBoss = false;
    private float changeTimeBetweenSpawns;
    // Start is called before the first frame update
    void Start()
    {
        zachGameLogic = FindObjectOfType<ZachGameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        totalTime += Time.deltaTime;
        if(isGameSpawner)
        {
            if(time > timeBetweenSpawns + zachGameLogic.currentTheme.secondsBetweenSpawnsModifier && spawningEnabled)
            {
                if(isAbleToSpawnBosses && Random.Range(0f, 1f) <= chanceToSpawnBoss && !lastSpawnBoss)
                {
                    SpawnObjs(1, 30000f, true);
                    lastSpawn3 = false;
                    lastSpawn2 = false;
                    lastSpawnBoss = true;
                    timeBetweenSpawns = 6f - changeTimeBetweenSpawns;
                    time = 0;
                }
                else if(Random.Range(0f,1f) <= chanceToSpawn3 && !lastSpawn3)
                {
                    SpawnObjs(3, 24000f);
                    lastSpawn3 = true;
                    lastSpawn2 = false;
                    lastSpawnBoss = false;
                    timeBetweenSpawns = 4f - changeTimeBetweenSpawns;
                    time = 0;
                }
                else if(Random.Range(0f,1f) <= chanceToSpawn2 && !lastSpawn2)
                {
                    SpawnObjs(2, 18000f);
                    lastSpawn2 = true;
                    lastSpawn3 = false;
                    lastSpawnBoss = false;
                    timeBetweenSpawns = 2f - changeTimeBetweenSpawns;
                    time = 0;                                                                                                                                                                                                                                                                                                                                                                                          
                }
                else{
                    SpawnObjs(1, 12000f);
                    lastSpawn2 = false;
                    lastSpawn3 = false;
                    lastSpawnBoss = false;
                    timeBetweenSpawns = 2f - changeTimeBetweenSpawns;
                    time = 0;
                }
            }

            if(zachGameLogic.playerScore == 20 && changeTimeBetweenSpawns == 0f)
            {
                changeTimeBetweenSpawns += 0.3f;
            }
            else if(zachGameLogic.playerScore == 50 && changeTimeBetweenSpawns == 0.3f)
            {
                changeTimeBetweenSpawns += 0.3f;
                isAbleToSpawnBosses = true;
            }
            else if(zachGameLogic.playerScore == 100 && changeTimeBetweenSpawns == 0.6f)
            {
                changeTimeBetweenSpawns += 0.4f;
            }
        }
        // else if(zachGameLogic.settingsMenu.activeInHierarchy){
        //     SpawnObjs(1, 1f);
        // }
    }


    Vector3 RandomPos()
    {
        Vector3 newPos = new Vector3(Random.Range(-xSpawnRange, xSpawnRange), Random.Range(0f, ySpawnRange), 0f);
        return newPos;
    }

    void SpawnObjs(int numToSpawn, float reachPlayerTime, bool isBoss = false)
    {
        float speedDamp = numToSpawn;
        for(int i = new int(); i < numToSpawn; i++)
        {
            //GameObject obj = Instantiate(woodObjects[Random.Range(0, woodObjects.Count)], transform.position + RandomPos(), Quaternion.identity);
            GameObject obj = Instantiate(zachGameLogic.currentTheme.prefab, isGameSpawner? transform.position + RandomPos() : transform.position, Quaternion.identity);
            if(isBoss) obj.transform.localScale = new Vector3(2f,2f,2f);
            WoodBehavior woodBehavior = obj.GetComponentInChildren<WoodBehavior>();
            if(isBoss) woodBehavior.isBossTarget = true;
            if(!isGameSpawner) woodBehavior.isGameWood = false;
            SetObjType(woodBehavior, isBoss);
            woodBehavior.waitBeforeMovement = i - 1f;
            int stringLength =  woodBehavior.text.text.Count();
            float stringLengthEffector = 1f;
            if(stringLength > 8) stringLengthEffector = 2f;
            woodBehavior.timeToReachPlayer = reachPlayerTime / speedDamp * stringLengthEffector * zachGameLogic.currentTheme.speedModifier;
            speedDamp--;
        }
    }

    void SetObjType(WoodBehavior behavior, bool isBoss = false)
    {
        Material mat = null;
        int type = Random.Range(0, zachGameLogic.currentTheme.materials.Count);
        if(zachGameLogic.currentTheme.meshHasSpecificMaterial)
        {
            mat = zachGameLogic.currentTheme.materials[type];
            behavior.meshFilter.mesh = zachGameLogic.currentTheme.meshes[type];
        }
        else if(zachGameLogic.currentTheme.meshHasMultipleMaterials)
        {
            int selectedMesh = Random.Range(0, zachGameLogic.currentTheme.meshes.Count);
            if(selectedMesh == 1)
            {
                mat = zachGameLogic.currentTheme.materials[Random.Range(0, zachGameLogic.currentTheme.mesh1Materials)];
                behavior.meshFilter.mesh = zachGameLogic.currentTheme.meshes[selectedMesh];
            }
            else{
                mat = zachGameLogic.currentTheme.materials[Random.Range(zachGameLogic.currentTheme.mesh2Materials, zachGameLogic.currentTheme.materials.Count)];
                behavior.meshFilter.mesh = zachGameLogic.currentTheme.meshes[selectedMesh];
            }
        }
        else{
            mat = zachGameLogic.currentTheme.materials[type];
        }
        behavior.meshRenderer.material = mat;
        behavior.text.text = mat.name;
        if(zachGameLogic.hardMode) behavior.MakeTextTransparent();
    }
}
