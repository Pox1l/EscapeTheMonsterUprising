using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public TextMeshProUGUI loadingText;
    public float textUpdateInterval = 0.5f;

    private bool animateText = false;

    public void LoadSceneWithTransition(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex, null));
    }

    public void LoadSceneWithTransition(int sceneIndex, Action onComplete)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex, onComplete));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex, Action onComplete)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        // spustíme animaci teèek
        animateText = true;
        StartCoroutine(AnimateLoadingText());

        yield return new WaitForSeconds(0.5f); // vizuální efekt

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // ukonèíme animaci a pøípadnì skryjeme loading screen
        animateText = false;

        onComplete?.Invoke();
    }

    private IEnumerator AnimateLoadingText()
    {
        int dotCount = 0;
        string baseText = "Loading";

        while (animateText)
        {
            loadingText.text = baseText + new string('.', dotCount);
            dotCount = (dotCount + 1) % 4;
            yield return new WaitForSeconds(textUpdateInterval);
        }
    }
}
