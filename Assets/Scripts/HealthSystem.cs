using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private HealthSO healthSO;

    private int hp;
    private int maxHpWithMult;
    private int maxHungerWithMult;
    private int hunger;
    private int hungerTickRate; //в секундах
    private float starvationDamagePercent;
    private float maxHpMult;
    private float maxHungerMult;
    private float damageMult;
    private float healMult;
    private bool isDead;
    private bool isLoadFromSave;

    public bool IsDead => isDead;
    public int Hp
    {
        get => hp;
        private set
        {
            hp = Mathf.Clamp(value, 0, maxHpWithMult);
            if (hp == 0 && !isDead)
            {
                isDead = true;
                OnZeroHp?.Invoke();
            }
        }
    }
    public int Hunger
    {
        get => hunger;
        private set
        {
            hunger = Mathf.Clamp(value, 0, maxHungerWithMult);
        }
    }
    public int HungerTickRate
    {
        get => hungerTickRate;
        private set
        {
            hungerTickRate = Mathf.Max(value, 1);
        }
    }
    public float StarvationDamagePercent
    {
        get => starvationDamagePercent;
        private set
        {
            starvationDamagePercent = Mathf.Clamp(value, 0.01f, 1);
        }
    }
    public float MaxHpMult
    {
        get => maxHpMult;
        private set
        {
            maxHpMult = Mathf.Max(0.1f, value);
            maxHpWithMult = Mathf.RoundToInt(healthSO.MaxHp * maxHpMult);
            hp = Mathf.Min(hp, maxHpWithMult);
        }
    }
    public float MaxHungerMult
    {
        get => maxHungerMult;
        private set
        {
            maxHungerMult = Mathf.Max(0.1f, value);
            maxHungerWithMult = Mathf.RoundToInt(healthSO.MaxHunger * maxHungerMult);
            hunger = Mathf.Min(hunger, maxHungerWithMult);
        }
    }
    public float DamageMult
    {
        get => damageMult;
        private set
        {
            damageMult = Mathf.Max(0.1f, value);
        }
    }
    public float HealMult
    {
        get => healMult;
        private set
        {
            healMult = Mathf.Max(0.1f, value);
        }
    }

    public UnityEvent OnZeroHp;

    private void Awake()
    {
        MaxHpMult = healthSO.DefaultHpMult;
        DamageMult = healthSO.DefaultDamageMult;
        HealMult = healthSO.DefaultHealMult;
        MaxHungerMult = healthSO.DefaultHungerMult;
        Hp = maxHpWithMult;
        Hunger = maxHungerWithMult;
        hungerTickRate = healthSO.DefaultHungerTickRate;
        StarvationDamagePercent = healthSO.DefaultStarvationDamagePercent;
        if (isLoadFromSave)
        {
            //загрузить из файла сохранения и перезаписать
        }
    }

    public void Heal(int points)
    {
        if (isDead || points <= 0) return;
        Hp += Mathf.RoundToInt(points * healMult);
    }

    public void Damage(int points)
    {
        if (isDead || points <= 0) return;
        Hp -= Mathf.RoundToInt(points * damageMult);
    }

    public void Eat(int points)
    {
        if (isDead || points <= 0) return;
        Hunger += points;
    }

    public void Starve(int points)
    {
        if (isDead || points <= 0) return;
        Hunger -= points;
    }

    public void IncreaseMaxHpMult(float value)
    {
        MaxHpMult += value;
    }

    public void DecreaseMaxHpMult(float value)
    {
        MaxHpMult -= value;
    }

    public void IncreaseMaxHungerMult(float value)
    {
        MaxHungerMult += value;
    }

    public void DecreaseMaxHungerMult(float value)
    {
        MaxHungerMult -= value;
    }

    public void IncreaseDamageMult(float value)
    {
        DamageMult += value;
    }

    public void DecreaseDamageMult(float value)
    {
        DamageMult -= value;
    }

    public void IncreaseHealMult(float value)
    {
        HealMult += value;
    }

    public void DecreaseHealMult(float value)
    {
        HealMult -= value;
    }

    public void IncreaseStarvationDamagePercent(float value)
    {
        StarvationDamagePercent += value;
    }

    public void DecreaseStarvationDamagePercent(float value)
    {
        StarvationDamagePercent -= value;
    }

    public void IncreaseHungerTickRate(int value)
    {
        HungerTickRate += value;
    }

    public void DecreaseHungerTickRate(int value)
    {
        HungerTickRate += value;
    }

    private IEnumerator HungerCoroutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(hungerTickRate);
            if (hunger > 0)
            {
                Starve(1);
                StarvationDamagePercent = healthSO.DefaultStarvationDamagePercent;
            }
            else
            {
                IncreaseStarvationDamagePercent(0.01f);
                Hp -= Mathf.Max(1, Mathf.RoundToInt(maxHpWithMult * starvationDamagePercent));
            }
        }
    }

    private void OnEnable()
    {
        if (!isDead) StartCoroutine(HungerCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}