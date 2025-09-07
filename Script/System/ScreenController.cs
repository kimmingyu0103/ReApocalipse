using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenController : MonoBehaviour
{

    FullScreenMode screenMode;
    public TMP_Dropdown dd;
    public Toggle fs;
    public GameObject settings;
    public GameObject Complete;
    List<Resolution> resolutions = new List<Resolution>();
    public int opNum = 0;

    void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        //resolutions.AddRange(Screen.resolutions);
        for (int i = Screen.resolutions.Length - 1; i >= 0; i--)
        {
            if (Screen.resolutions[i].refreshRate == 60)
                resolutions.Add(Screen.resolutions[i]); // 해상도 리스트 불러오기
        }

        if (resolutions.Count > 0) dd.options.Clear();

        foreach (Resolution item in resolutions)
        { // 리스트 업데이트
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            dd.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                dd.value = opNum; // 현재 값 불러오기
            }
            opNum++;
        }
        dd.RefreshShownValue(); // 토글 체크 리셋
        fs.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? false : true;
    }

    public void optionChange(int idx)
    { // 해상도 인덱스 변경
        opNum = idx;
    }

    public void FullScreenBtn(bool isFull)
    { // 창 모드 체크
        screenMode = isFull ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
    }

    public void OnBtnClick()
    { // 확인 버튼
        if (resolutions.Count > 0)
            Screen.SetResolution(resolutions[opNum].width, resolutions[opNum].height, screenMode);
        Time.timeScale = 1;
        settings.SetActive(false);
    }

}