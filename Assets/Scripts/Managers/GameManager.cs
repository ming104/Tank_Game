using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [Header("Player")]
    private PlayerController playerController;

    [Header("Time")]
    public TextMeshProUGUI Time_Text;
    private float time;

    private int minutes;
    private int seconds;
    [Header("Score")]
    public TextMeshProUGUI Score_Text;
    private int Score;
    public int score
    {
        get { return Score; }
        set { Score = value; }
    }
    [Header("GameOver_Panel")]
    public GameObject GameOverPanel;
    public TextMeshProUGUI End_Time_Text;
    public TextMeshProUGUI End_Score_Text;

    [Header("Resolution")]
    public TMP_Dropdown resolutionDropdown;
    private FullScreenMode screenMode;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;
    public GameObject Settingpanel;
    public Button SettingEndButton;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        var SoundManager = FindObjectsOfType<MainMenu>();
        playerController = GameObject.Find("Tank_Player").GetComponent<PlayerController>();
        InitUI();
        SoundSetting();
        time = 0;
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Score_Text.text = "Score : " + Score.ToString();
        Timer();

        if (playerController.currentPlayerHp < 0) // 체력이 0이하일 때
        {
            GameEnd();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameSetting();
        }
    }
    void Timer()
    {
        time += Time.deltaTime;

        // 경과 시간을 분과 초로 변환
        minutes = (int)(time / 60);
        seconds = (int)(time % 60);

        // 경과 시간을 표시
        Time_Text.text = string.Format("Time : " + "{0:00}:{1:00}", minutes, seconds);
    }

    void GameEnd()
    {
        GameOverPanel.SetActive(true);
        End_Score_Text.text = score.ToString() + "점";
        End_Time_Text.text = string.Format("{0:00}분 : {1:00}초", minutes, seconds); ;
        Time.timeScale = 0;
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
    public void GameSetting()
    {
        Time.timeScale = 0;
        Settingpanel.SetActive(true);
    }
    public void ReGameButton()
    {
        SceneManager.LoadScene("PlayGame");
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion 버튼 설정
    public void okBtnClick() // 저장
    {
        Time.timeScale = 1f;
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height,
            screenMode);
        PlayerPrefs.SetFloat("SoundEffect", SoundEffect_Slider.value);
        AudioListener.volume = PlayerPrefs.GetFloat("SoundEffect");
        Settingpanel.SetActive(false);
    }
}
