using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;
    public static  CursorManager Instance => _instance;
    
    private bool _isCursorLocked = false;
    public bool IsCursorLocked => _isCursorLocked;

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
        }
    }
    
    private void ToggleCursor()
    {
        if (_isCursorLocked)
        {
            UnlockCursor(); 
        }
        else
        {
            LockCursor();
        }
    }
    
    private void LockCursor()
    {
        _isCursorLocked = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        _isCursorLocked = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}