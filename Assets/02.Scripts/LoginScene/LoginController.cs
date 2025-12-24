using UnityEngine;

public class LoginController : MonoBehaviour
{
    private static LoginController _instance;
    public static LoginController Instance => _instance;
    
    private const string UserIdPrefix = "User_";
    private const string LastLoginIdKey = "LastLoginId";
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    public string GetLastLoginId()
    {
        return PlayerPrefs.GetString(LastLoginIdKey, "");
    }
    
    public bool Register(string id, string password, out string message)
    {
        string key = UserIdPrefix + id;
        if (PlayerPrefs.HasKey(key))
        {
            message = "이미 존재하는 아이디입니다.";
            return false;
        }
        
        string hashedPassword = PasswordHashService.ConvertPasswordToHash(password);
        
        PlayerPrefs.SetString(key, hashedPassword);
        PlayerPrefs.Save();

        message = "";
        return true;
    }
    
    public bool TryLogin(string id, string password, out string message)
    {
        string key = UserIdPrefix + id;
        if (!PlayerPrefs.HasKey(key))
        {
            message = "존재하지 않는 아이디입니다.";
            return false;
        }
        
        string storedHash = PlayerPrefs.GetString(key);
        bool isValid = PasswordHashService.VerifyPassword(password, storedHash);
        
        if (!isValid)
        {
            message = "비밀번호가 일치하지 않습니다.";
            return false;
        }
        
        PlayerPrefs.SetString(LastLoginIdKey, id);
        PlayerPrefs.Save();
        message = null;
        return true;
    }
}
