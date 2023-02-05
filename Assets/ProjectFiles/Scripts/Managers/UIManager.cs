using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public GameObject turretButtonPrefab;
    public Transform turretButtonsParent;
    public TMP_Text currencyText;

    public GameObject pauseEndPanel;
    public TMP_Text label;
    public Button continueButton;
    public Button quitButton;
    public Button restartButton;
    public Button pauseButton;
    public Slider waveSlider;

    public List<Toggle> toggles { get; private set; } = new List<Toggle>();
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var turret in GameManager.Instance.availableTurrest) {
            Toggle toggle = Instantiate(turretButtonPrefab, turretButtonsParent).GetComponent<Toggle>();
            toggle.group = turretButtonsParent.GetComponent<ToggleGroup>();
            toggle.onValueChanged.AddListener((value) => GridManager.Instance.turretToPlace = !value ? null : turret);
            toggle.isOn = false;
            toggle.GetComponent<TurretButton>().Turret = turret;
            toggles.Add(toggle);
        }
        pauseEndPanel.SetActive(false);
        continueButton.onClick.AddListener(ContinueGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(() => {
            AudioManager.Instance.FadeAudioOut();
            SceneLoader.Instance.LoadScene(0);
        });
        pauseButton.onClick.AddListener(PauseGame);
        label.text = "Pause";
        waveSlider.value = 1;
        anim = GetComponent<Animator>();
    }

    public void UnToggleAll() {
        foreach (var item in toggles) {
            item.isOn = false;
        }
    }

    private void OnGameEnd(EventArgs args) {
        continueButton.onClick.RemoveAllListeners();
        pauseEndPanel.SetActive(true);
        if (args.boolValue) {
            quitButton.gameObject.SetActive(false);
            continueButton.onClick.AddListener(() => {
                AudioManager.Instance.FadeAudioOut();
                SceneLoader.Instance.LoadScene(0);
            });
            label.text = "You won!";
        }
        else {
            continueButton.gameObject.SetActive(false);
            label.text = "You lost!";
        }
    }

    private void PauseGame() {
        GlobalEventSender.SendPause(new EventArgs());
    }

    private void ContinueGame() {
        GlobalEventSender.SendContinue(new EventArgs());
    }

    private void RestartGame() {
        AudioManager.Instance.FadeAudioOut();
        SceneLoader.Instance.ReloadScene();
    }

    private void OnPauseGame(EventArgs args) {
        pauseEndPanel.SetActive(true);
    }

    private void OnContinueGame(EventArgs args) {
        pauseEndPanel.SetActive(false);
    }

    private void OnCurrencyChanged(EventArgs args) {
        int currentAmount = int.Parse(currencyText.text);
        if(args.intValue > currentAmount) {
            anim.Play("addMoney");
        }

        currencyText.text = args.intValue.ToString();
    }

    private void OnEnable() {
        GlobalEventSender.CurrencyChangedEvent += OnCurrencyChanged;
        GlobalEventSender.PauseEvent += OnPauseGame;
        GlobalEventSender.ContinueEvent += OnContinueGame;
        GlobalEventSender.GameEndEvent += OnGameEnd;
    }

    private void OnDisable() {
        GlobalEventSender.CurrencyChangedEvent -= OnCurrencyChanged;
        GlobalEventSender.PauseEvent -= OnPauseGame;
        GlobalEventSender.ContinueEvent -= OnContinueGame;
        GlobalEventSender.GameEndEvent -= OnGameEnd;
    }
}
