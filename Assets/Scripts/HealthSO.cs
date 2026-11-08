using UnityEngine;

[CreateAssetMenu(fileName = "HealthSO", menuName = "Scriptable Objects/HealthSO")]
public class HealthSO : ScriptableObject
{
    [SerializeField][Range(10, 100)] private int maxHp;
    [SerializeField][Range(10, 100)] private int maxHunger;
    [SerializeField][Min(1)] private int defaultHungerTickRate; //в секундах
    [SerializeField][Range(0.01f, 1f)] private float defaultStarvationDamagePercent;
    [SerializeField][Min(0.1f)] private float defaultHpMult;
    [SerializeField][Min(0.1f)] private float defaultDamageMult;
    [SerializeField][Min(0.1f)] private float defaultHealMult;
    [SerializeField][Min(0.1f)] private float defaultHungerMult;

    public int MaxHp => maxHp;
    public int MaxHunger => maxHunger;
    public int DefaultHungerTickRate => defaultHungerTickRate;
    public float DefaultStarvationDamagePercent => defaultStarvationDamagePercent;
    public float DefaultHpMult => defaultHpMult;
    public float DefaultDamageMult => defaultDamageMult;
    public float DefaultHealMult => defaultHealMult;
    public float DefaultHungerMult => defaultHungerMult;
}
