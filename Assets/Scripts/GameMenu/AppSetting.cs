using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class AppSetting : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] Toggle bgmMuteToggle;
    [SerializeField] Toggle sfxMuteToggle;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] TMP_Text bgmVolText;
    [SerializeField] TMP_Text sfxVolText;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle windowedModeToggle;
    [SerializeField] GameObject confirmDialog;

    private int currentResolutionIndex;
    private int previousResolutionIndex;

    private void OnEnable()
    {
        bgmMuteToggle.isOn = audioManager.IsBGMMuted;
        sfxMuteToggle.isOn = audioManager.IsSFXMuted;
        bgmSlider.value = audioManager.BGMVolume;
        sfxSlider.value = audioManager.SFXVolume;
        SetBGMVolText(bgmSlider.value);
        SetSFXVolText(sfxSlider.value);

        ResolutionEntries();
    }

    public void ResolutionEntries()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            string option = Screen.resolutions[i].width + " x " + Screen.resolutions[i].height;
            options.Add(option);
            if (Screen.resolutions[i].width == Screen.currentResolution.width &&
                               Screen.resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        // set the current resolution, if playerprefs not found, set to max value
        currentResolutionIndex = PlayerPrefs.GetInt("Resolution", int.MaxValue);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // store previous resolution
        previousResolutionIndex = currentResolutionIndex;
    }

    // once palyer presses 'Apply Video Settings' button, this function will be called
    public void SetResolution()
    {
        Resolution resolution = Screen.resolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionDropdown.RefreshShownValue();
        CheckResolution();
        confirmDialog.gameObject.SetActive(true);
    }

    // this function will check confirmation from player if the resolution is ok
    // after 10 seconds of no confirmation, it will revert to previous resolution
    // a dialog box will appear to ask for confirmation from successful OnSetResolution.Invoke()
    // if player presses 'Yes', it will save the current resolution to playerprefs
    public void CheckResolution()
    {
        StartCoroutine(ResConfirmation());
    }

    IEnumerator ResConfirmation()
    {
        yield return new WaitForSecondsRealtime(10f);
        if (confirmDialog.gameObject.activeSelf)
        {
            RevertResolution();
        }
    }

    public void SaveResolution()
    {
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.Save();
        // update previous res index
        previousResolutionIndex = resolutionDropdown.value;
        confirmDialog.gameObject.SetActive(false);
    }

    public void RevertResolution()
    {
        resolutionDropdown.value = previousResolutionIndex;
        Resolution resolution = Screen.resolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionDropdown.RefreshShownValue();
        confirmDialog.gameObject.SetActive(false);
    }

    public void SetWindowedMode(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt("WindowedMode", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetBGMVolText(float value)
    {
        bgmVolText.text = Mathf.RoundToInt(value * 100).ToString();
    }

    public void SetSFXVolText(float value)
    {
        sfxVolText.text = Mathf.RoundToInt(value * 100).ToString();
    }
}
