using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TextMeshProUGUI _progressText;

    private void Start()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("MainScene");

        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            float progress = ao.progress;
            _progressSlider.value = progress;
            _progressText.text = $"{(int)(progress * 100)}%";

            if (progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
