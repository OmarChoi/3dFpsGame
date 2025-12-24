using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    private EGameState _state = EGameState.Ready;
    public EGameState State => _state;

    [SerializeField] private TextMeshProUGUI _stateTextUI;

    [SerializeField] private OptionPopupUI _optionPopupUI;
    
    private const float  GameReadyDuration = 2f;
    private const float  GameStartDelay    = 0.5f;
    private const string ReadyStateText    = "준비중...";
    private const string StartStateText    = "시작!";
    private const string GameOverStateText = "게임 오버!";
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        LockCursor();
    }

    private void Start()
    {
        _stateTextUI.gameObject.SetActive(true);
        
        _state = EGameState.Ready;
        _stateTextUI.text = ReadyStateText;

        StartCoroutine(StartToPlayCoroutine());
    }

    private IEnumerator StartToPlayCoroutine()
    {
        yield return new WaitForSeconds(GameReadyDuration);
        _stateTextUI.text = StartStateText;
        
        yield return new WaitForSeconds(GameStartDelay);
        _state = EGameState.Playing;
        
        _stateTextUI.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        _state = EGameState.GameOver;
        _stateTextUI.text = GameOverStateText;
        _stateTextUI.gameObject.SetActive(true);
    }

    public bool CanPlay()
    {
        return _state == EGameState.Playing;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            _optionPopupUI.Show();
        }
    }
    
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void Pause()
    {
        Time.timeScale = 0;
        UnlockCursor();
    }

    public void Continue()
    {
        Time.timeScale = 1;
        LockCursor();
    }
    
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LoadingScene");
    }
}
