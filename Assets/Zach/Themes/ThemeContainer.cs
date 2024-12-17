using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ThemeContainer", order = 1)]
public class ThemeContainer : ScriptableObject
{
    [Header("Prefab")]
    public GameObject prefab;
    [Header("Mesh")]
    public List<Mesh> meshes;
    public int materialSlots;
    public List<Material> materials;
    [Header("Audio")]
    public AudioClip destroySFX;
    public AudioClip gameOverMusic;
    public AudioClip typingSFX;
    public AudioClip gameMusic;
    public AudioClip damageTaken;
    public bool loopGameOverMusic;
    [Header("Backgrounds")]
    public Texture background1;
    public Texture background2;
    public Texture background3;
    public Texture background4;
    [Header("Game Changes")]
    public float speedModifier = 1f;
    public float secondsBetweenSpawnsModifier = 0f;
    public Color menuTextColor;
    public bool meshHasSpecificMaterial = false;
    public bool hasThemeSpecificGameOver = false;
    [Tooltip("If true, adjust below settings")]public bool meshHasMultipleMaterials = false;
    public int mesh1Materials;
    public int mesh2Materials;
}
