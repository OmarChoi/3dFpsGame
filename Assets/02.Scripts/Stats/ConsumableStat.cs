using UnityEngine;

[System.Serializable]
public class ConsumableStat
{
    [SerializeField] private float _value;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _regenValue;

    public float Value => _value;
    public float MaxValue => _maxValue;
    
    public void Initialize()
    {
        SetValue(_maxValue);
    }

    public bool TryConsume(float amount)
    {
        if (_value < amount) return false;
        Consume(amount);
        return true;
    }

    private void Consume(float amount)
    {
        SetValue(_value - amount);
    }
    
    public void Regenerate(float time)
    {
        SetValue(_value + _regenValue * time);
    }
    
    public void Increase(float amount)
    {
        SetValue(_value + amount);
    }
    
    public void IncreaseMax(float amount)
    {
        _maxValue += amount;
    }

    public void Decrease(float amount)
    {
        SetValue(_value - amount);
    }
    
    public void DecreaseMax(float amount)
    {
        _maxValue -= amount;
    }

    public void SetValue(float value)
    {
        _value = Mathf.Clamp(value, 0, _maxValue);
    }

    public void SetMaxValue(float maxValue)
    {
        _maxValue = maxValue;
    }
}
