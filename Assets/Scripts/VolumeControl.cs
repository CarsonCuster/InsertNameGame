using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    private ZachGameLogic zachGameLogic;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        zachGameLogic = FindObjectOfType<ZachGameLogic>();
        slider.value = zachGameLogic.gameAudioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value != zachGameLogic.gameAudioSource.volume)
        {
            zachGameLogic.gameAudioSource.volume = slider.value;
            zachGameLogic.gameOverAudioSource.volume = slider.value;
        }
    }
}
