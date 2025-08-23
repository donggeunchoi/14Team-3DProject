using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    public Scrollbar sfxScrollbar;
    public Scrollbar bgmScrollbar;
    
    // Start is called before the first frame update
    void Start()
    {
        var data = SaveManager.Instance.Data;
        bgmScrollbar.value = data.bgmVolume;
        sfxScrollbar.value = data.sfxVolume;

        bgmScrollbar.onValueChanged.AddListener((value) =>
        {
            AudioManager.Instance?.SetBGMVolume(value);
            SaveManager.Instance.Data.bgmVolume = value;
            SaveManager.Instance.SaveNow();
        });

        sfxScrollbar.onValueChanged.AddListener((value) =>
        {
            AudioManager.Instance?.SetSFXVolume(value);
            SaveManager.Instance.Data.sfxVolume = value;
            SaveManager.Instance.SaveNow();
        });
        
        
        //  //저장된 볼륨값을 슬라이더에 불러와 저장합니다.
        //  bgmScrollbar.value = PlayerPrefs.GetFloat("BGMVolume",1f);
        //  sfxScrollbar.value = PlayerPrefs.GetFloat("SFXVolume",1f);
        //
        // // 사용자가 조절할 때마다 실시간으로 사운드 볼륨을 바꾸게 해주는 로직.
        //  bgmScrollbar.onValueChanged.AddListener((v)=> AudioManager.Instance.SetBGMVolume(v));
        //  sfxScrollbar.onValueChanged.AddListener((v)=> AudioManager.Instance.SetSFXVolume(v));
    }
    
}
