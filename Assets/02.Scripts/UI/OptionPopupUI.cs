using UnityEngine;
using UnityEngine.UI;

public class OptionPopupUI : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _gameExitButton;

    private void Start()
    {
        _continueButton.onClick.AddListener(GameContinue);
        _restartButton.onClick.AddListener(GameRestart);
        _gameExitButton.onClick.AddListener(GameExit);

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        // todo. 애니메이션 및 이펙트 추가
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        // todo. 애니메이션 및 이펙트 추가
    }

    private void GameContinue()
    {
        GameManager.Instance.Continue();
        Hide();
    }

    private void GameRestart()
    {
        GameManager.Instance.Restart();
        Hide();
    }

    private void GameExit()
    {
        GameManager.Instance.Quit();
    }
}