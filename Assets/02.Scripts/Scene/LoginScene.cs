using System.Text.RegularExpressions;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    // 로그인씬 (로그인/회원가입) -> 게임씬

    private enum ESceneMode
    {
        Login,
        Register
    }
    
    private ESceneMode _mode = ESceneMode.Login;
    
    [Header("Button")]
    [SerializeField] private GameObject _passwordConfirmObject;
    [SerializeField] private Button _gotoRegisterButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _gotoLoginButton;
    [SerializeField] private Button _registerButton;

    [Header("Text")]
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;
    [SerializeField] private TextMeshProUGUI _messageTextUI;
    
    // Error Message
    private const string LoginErrorMessage = "잘못된 아이디 또는 비밀번호 입니다.";
    private string _errorMessage;

    // 마지막 로그인 아이디 정보
    private const string LastLoginID = "LastLoginID";
    private string _lastLoginID;
    
    // 정규 표현식
    private const string EmailPattern =
        @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
    private const string PasswordPattern =
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])[A-Za-z\d\W_]{7,20}$";
    
    private void Start()
    {
        AddButtonEvents();
        Refresh();
        if (PlayerPrefs.HasKey(LastLoginID))
        {
            _idInputField.text = PlayerPrefs.GetString(LastLoginID);
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString(LastLoginID, _lastLoginID);
    }

    private void AddButtonEvents()
    {
        _gotoRegisterButton.onClick.AddListener(GotoRegister);
        _loginButton.onClick.AddListener(Login);
        _gotoLoginButton.onClick.AddListener(GotoLogin);
        _registerButton.onClick.AddListener(Register);
    }

    private void Refresh()
    {
        _passwordConfirmObject.SetActive(_mode == ESceneMode.Register);
        _gotoRegisterButton.gameObject.SetActive(_mode == ESceneMode.Login);
        _loginButton.gameObject.SetActive(_mode == ESceneMode.Login);
        _gotoLoginButton.gameObject.SetActive(_mode == ESceneMode.Register);
        _registerButton.gameObject.SetActive(_mode == ESceneMode.Register);
    }

    private void Login()
    {
        string id = _idInputField.text;
        if (!IsValidID(id))
        {
            _messageTextUI.text = LoginErrorMessage;
            return;
        }
        
        string password = _passwordInputField.text;
        if (!IsValidPassword(password))
        {
            _messageTextUI.text = LoginErrorMessage;
            return;
        }
        
        if (!PlayerPrefs.HasKey(id))
        {
            _messageTextUI.text = LoginErrorMessage;
            return;
        }
        
        string myPassword = PlayerPrefs.GetString(id);
        if (myPassword != password)
        {
            _messageTextUI.text = LoginErrorMessage;
            return;
        }
        _lastLoginID = id;
        SceneManager.LoadSceneAsync("LoadingScene");
    }

    private void Register()
    {
        string id = _idInputField.text;

        if (!IsValidID(id))
        {
            _messageTextUI.text = _errorMessage;
            return;
        }

        string password = _passwordInputField.text;
        if (!IsValidPassword(password))
        {
            _messageTextUI.text = _errorMessage;
            return;
        }

        string confirmPassword = _passwordInputField.text;
        if (string.IsNullOrEmpty(confirmPassword) || password != confirmPassword)
        {
            _messageTextUI.text = "패스워드를 확인해주세요.";
            return;
        }
        
        if (PlayerPrefs.HasKey(id))
        {
            _messageTextUI.text = "중복된 아이디입니다.";
            return;
        }

        PlayerPrefs.SetString(id, password);

        GotoLogin();
    }
    
    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            _errorMessage = "패스워드를 입력해주세요.";
            return false;
        }
        if (!Regex.IsMatch(password, PasswordPattern))
        {
            _errorMessage = "잘못된 비밀번호 형식입니다.";
            return false;
        }
        return true;
    }


    private bool IsValidID(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            _errorMessage = "아이디를 확인해주세요.";
            return false;
        }
        if (!Regex.IsMatch(id, EmailPattern))
        {
            _errorMessage = "잘못된 아이디 형식입니다.";
            return false;
        }
        return true;
    }
    
    private void GotoLogin()
    {
        _mode = ESceneMode.Login;
        Refresh();
    }

    private void GotoRegister()
    {
        _mode = ESceneMode.Register;
        Refresh();
    }
}