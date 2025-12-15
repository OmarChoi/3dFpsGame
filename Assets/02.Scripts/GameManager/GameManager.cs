using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    private EGameState _state = EGameState.Ready;
    public EGameState State => _state;

    [SerializeField] private TextMeshProUGUI _stateTextUI;

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
}
