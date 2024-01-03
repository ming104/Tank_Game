using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    [Header("해상도 관련")]
    FullScreenMode screenMode;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;
    public GameObject Settingpanel;
    public Button SettingEndButton;

    void Start()
    {
        InitUI();
        SoundSetting();
    }

    #region 해상도


    void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value != 30)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + Math.Round(item.refreshRateRatio.value) + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();

        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    #endregion 해상도
    #region 사운드설정
    [Header("사운드 관련")]
    [SerializeField] private Slider SoundEffect_Slider;
    void SoundSetting()
    {
        if (!PlayerPrefs.HasKey("SoundEffect"))
        {
            PlayerPrefs.SetFloat("SoundEffect", 1f);
        }
        else
        {
            SoundEffect_Slider.value = PlayerPrefs.GetFloat("SoundEffect");
        }
    }

    #endregion 사운드설정
    #region 버튼 설정
    public void GameStart()
    {
        SceneManager.LoadScene("PlayGame");
    }
    public void GameSetting()
    {
        Settingpanel.SetActive(true);
    }
    public void GameExit()
    {
        Application.Quit(0);
    }
    #endregion 버튼 설정
    public void okBtnClick() // 저장
    {
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height,
            screenMode);
        PlayerPrefs.SetFloat("SoundEffect", SoundEffect_Slider.value);
        AudioListener.volume = PlayerPrefs.GetFloat("SoundEffect");
        Settingpanel.SetActive(false);
    }
}
