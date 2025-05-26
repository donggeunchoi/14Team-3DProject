using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider bgmSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        // //저장된 볼륨값을 슬라이더에 불러와 저장합니다.
        // bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume",1f);
        // sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume",1f);
        
        //사용자가 조절할 때마다 실시간으로 사운드 볼륨을 바꾸게 해주는 로직.
        // bgmSlider.onValueChanged.AddListener((v)=> AudioManager.Instance.SetBGMVolume(v));
        // sfxSlider.onValueChanged.AddListener((v)=> AudioManager.Instance.SetSFXVolume(v));
    }
    
}
