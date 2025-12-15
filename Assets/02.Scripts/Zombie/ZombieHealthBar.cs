using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Zombie))]
public class ZombieHealthBar : MonoBehaviour
{
    private Zombie _zombie;
    [SerializeField] private Image _gaugeImage;
    [SerializeField] private Transform _healthBarTransform;
    private Camera _mainCamera;
    private float _lastHealth = -1;
    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        float currentHealth = _zombie.Health.Value / _zombie.Health.MaxValue;
        if (!Mathf.Approximately(_lastHealth, currentHealth))
        {
            _gaugeImage.fillAmount = currentHealth;
            _lastHealth = currentHealth;
        }
        
        
        _healthBarTransform.forward = _mainCamera.transform.forward;
    }
}
