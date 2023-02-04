using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    public CanvasGroup fader;
    private int sceneIndex;
    private int direction = 1;
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void LoadScene(int index) {
        sceneIndex = index;
        StartCoroutine(Fade());
    }

    /*public void LoadScene(string sceneName) {
        //sceneToLoad = SceneManager.Get(sceneName);
        StartCoroutine(Fade());
    }*/

    public void ReloadScene() {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Fade());
    }

    public int GetSceneIndex() {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private IEnumerator Fade() {
        fader.blocksRaycasts = true;
        direction = 1;
        fader.alpha = 0;
        while(fader.alpha < 1) {
            fader.gameObject.SetActive(true);
            fader.alpha += direction * Time.deltaTime * 3;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        SceneManager.LoadScene(sceneIndex);

        direction = -1;
        fader.alpha = 1;
        while (fader.alpha > 0) {
            fader.gameObject.SetActive(true);
            fader.alpha += direction * Time.deltaTime * 3;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        fader.blocksRaycasts = false;
    }
}
