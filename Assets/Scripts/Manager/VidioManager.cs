using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidioManager : MonoBehaviour
{
    FullScreenMode m_ScreenMode;
    private List<Resolution> m_Resolutions = new List<Resolution>();
    public Dropdown m_ResolutionDropdown;
    public Dropdown m_FpsDropDown;
    public Toggle m_FullScreen;
    public Toggle m_VSync;
    public int m_ResolutionNumX = 0;

    

    private void Start()
    {
        InitUI();
    }

    void InitUI()
    {
       for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value == 60 ||
                Screen.resolutions[i].refreshRateRatio.value == 144)
            {
                m_Resolutions.Add(Screen.resolutions[i]);
            }
        }
        m_ResolutionDropdown.options.Clear();

        int selectNum = 0;

        foreach (Resolution item in m_Resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRateRatio + "hz";
            m_ResolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                m_ResolutionDropdown.value = selectNum;
                selectNum++;
        }

        m_ResolutionDropdown.RefreshShownValue(); // 드롭다운 새로고침 (옵션이 변경됬으니 한번해줌)

        m_FullScreen.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;

        m_VSync.isOn = QualitySettings.vSyncCount > 0;

        InitFPS_DropDown();
    }

    void InitFPS_DropDown()
    {
        List<string> fpsOptions = new List<string> { "30 FPS", "60 FPS", "100 FPS", "140 FPS", "200 FPS" };

        m_FpsDropDown.options.Clear();

        foreach (string fps in fpsOptions)
        {
            m_FpsDropDown.options.Add(new Dropdown.OptionData(fps));
        }

        int defaultIndex = fpsOptions.IndexOf(Application.targetFrameRate.ToString());
        m_FpsDropDown.value = defaultIndex != -1 ? defaultIndex : 1;
        m_FpsDropDown.RefreshShownValue();
    }

    public void DropBoxOptionChange(int _x)
    {
        m_ResolutionNumX = _x;
    }

    public void DropBoxFPSChange(int _index)
    {
        int[] fpsValue = { 30, 60, 100, 140, 200 };
        Application.targetFrameRate = fpsValue[_index];
        Debug.Log($"FPS 제한 설정: {fpsValue[_index]}");
    }

    public void FullScreenButton(bool _isFull)
    {
        m_ScreenMode = _isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void VScyncButton(bool _isOn)
    {
        QualitySettings.vSyncCount = _isOn ? 1 : 0;
    }

    public void OnButtonClick()
    {
        Screen.SetResolution(m_Resolutions[m_ResolutionNumX].width,
            m_Resolutions[m_ResolutionNumX].height,
            m_ScreenMode);

        
    }

   
  
}
