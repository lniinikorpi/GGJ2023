using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : Singleton<MainMenuManager>
{
    public List<Button> levelButtons = new List<Button>();
    public GameObject mainPanel;
    public Button playButton;

    public GameObject mapPanel;
    public GameObject congratsPanel;

    public GameObject settingsPanel;

    public Button muteButton;
    public Sprite muteSprite;
    public Sprite unMuteSprite;

    public Slider volumeSlider;

    public TMP_Dropdown resolutionDropDown;

    public GameObject howToPlayPanel;
    public GameObject slide1;
    private void Start() {
        congratsPanel.gameObject.SetActive(false);
        howToPlayPanel.SetActive(false);
        switch (StateManager.state) {
            case MenuState.None:
                mainPanel.SetActive(true);
                mapPanel.SetActive(false);
                break;
            case MenuState.Map:
                mainPanel.SetActive(false);
                mapPanel.SetActive(true);
                break;
        }
        StateManager.state = MenuState.Map;
        settingsPanel.SetActive(false);

        playButton.onClick.AddListener(() => {
            if(StateManager.showTutorial) {
                StateManager.showTutorial = false;
                howToPlayPanel.SetActive(true);
                slide1.SetActive(true);
            }
            else {
                mapPanel.SetActive(true);
            }
        });

        for (int i = 0; i < levelButtons.Count; i++) {
            levelButtons[i].interactable = false;
        }
        for (int i = 0; i <= StateManager.completedLevels; i++) {
            if(i >= levelButtons.Count) {
                if (!StateManager.isCongrated) {
                    StateManager.isCongrated = true;
                    congratsPanel.SetActive(true);
                }
                //Open endless if time left
                break;
            }
            int index = i + 1;
            levelButtons[i].interactable = true;
            levelButtons[i].onClick.AddListener(() => {
                AudioManager.Instance.FadeAudioOut();
                SceneLoader.Instance.LoadScene(index);
            });
        }

        if(StateManager.isMuted) {
            AudioListener.volume = 0;
            muteButton.GetComponent<Image>().sprite = muteSprite;
        }
        else {
            muteButton.GetComponent<Image>().sprite = unMuteSprite;
            volumeSlider.value = StateManager.volume;
            AudioListener.volume = volumeSlider.value;
        }

        muteButton.onClick.AddListener(() => {
            StateManager.isMuted = !StateManager.isMuted;
            if (StateManager.isMuted) {
                AudioListener.volume = 0;
                muteButton.GetComponent<Image>().sprite = muteSprite;
            }
            else {
                muteButton.GetComponent<Image>().sprite = unMuteSprite;
                volumeSlider.value = StateManager.volume;
                AudioListener.volume = volumeSlider.value;
            }
        });

        volumeSlider.onValueChanged.AddListener((value) => {
            StateManager.volume = value;
            if (!StateManager.isMuted) {
                AudioListener.volume = value;
            }
        });

        Resolution[] resolutions = Screen.resolutions;
        List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
        foreach (Resolution resolution in resolutions) {
            float divider = (float)resolution.width / (float)resolution.height;
            if (divider >= 1.77f && divider < 1.78f) {
                optionDatas.Add(new TMP_Dropdown.OptionData() {
                    text = resolution.width + "x" + resolution.height
                });
            }
        }
        optionDatas = optionDatas.DistinctBy(x => x.text).ToList();
        if(optionDatas.Count == 0) {
            resolutionDropDown.gameObject.SetActive(false);
        }
        else {
            resolutionDropDown.AddOptions(optionDatas);
            resolutionDropDown.onValueChanged.AddListener((value) => {
                string widthString = "";
                int width = 0;
                string heightString = "";
                int height = 0;
                bool widthFound = false;
                for (int i = 0; i < resolutionDropDown.options[value].text.Length; i++) {
                    if (!widthFound) {
                        if (resolutionDropDown.options[value].text[i] != 'x') {
                            widthString += resolutionDropDown.options[value].text[i];
                        }
                        else {
                            widthFound = true;
                        }
                    }
                    else {
                        heightString += resolutionDropDown.options[value].text[i];
                    }
                }
                width = int.Parse(widthString);
                height = int.Parse(heightString);
                Screen.SetResolution(width, height, true);
            });
            resolutionDropDown.value = resolutionDropDown.options.Count - 1; 
        }
    }

    public void QuitGame() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
