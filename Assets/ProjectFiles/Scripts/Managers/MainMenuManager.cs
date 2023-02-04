using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : Singleton<MainMenuManager>
{
    public List<Button> levelButtons = new List<Button>();
    public GameObject mainPanel;
    public GameObject mapPanel;
    public GameObject congratsPanel;
    private void Start() {
        congratsPanel.gameObject.SetActive(false);
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
            levelButtons[i].onClick.AddListener(() => SceneLoader.Instance.LoadScene(index));
        }
    }
}
