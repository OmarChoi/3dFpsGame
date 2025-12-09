using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStat : MonoBehaviour
{
    [Header("Stamina")]
    [Space]
    [SerializeField] private float  _stamina;
    [SerializeField] private float  _maxStamina;
    [SerializeField] private float  _regenerationRate;
    [SerializeField] private float  _regenerateCooldown;
    private float  _lastUseStaminaTime;
    public static Action OnStaminaChanged;
    private float Stamina
    {
        get => _stamina;
        set
        {
            if (Mathf.Approximately(_stamina, value)) return;
            _stamina = Mathf.Clamp(value, 0, _maxStamina );
            OnStaminaChanged?.Invoke();
        }
    }
    
    private void Update()
    {
        RegenerateStamina();
    }

    private void RegenerateStamina()
    {
        if (Mathf.Approximately(Stamina, _maxStamina)) return;
        float currentTime = Time.time;
        if (currentTime - _lastUseStaminaTime < _regenerateCooldown) return;
        Stamina += _maxStamina * _regenerationRate * Time.deltaTime;
    }

    public bool TryUseStamina(float stamina)
    {
        if (Stamina < stamina) return false;
        Stamina -= stamina;
        _lastUseStaminaTime = Time.time;
        return true;
    }
}
