using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Slider healthBar;
    private Entity_VFX entityVFX;
    private Entity entity;
    private Entity_Stats stats;

    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockBackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockBackPower = new Vector2(7f, 7f);
    [SerializeField] private float knockBackDuration = 0.2f;
    [SerializeField] private float heavyKnockBackDuration = 0.5f;
    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDmgThreshold = 0.3f; // percentage of HP you should lose to consider dmg as heavy

    protected virtual void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        stats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();

        currentHp = stats.GetMaxHealth();
        UpdateHPBar();
    }

    public virtual bool TakeDamage(float dmg, Transform dmgDealer)
    {
        if (isDead)
        {
            return false;
        }

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack!");
            return false;
        }

        Vector2 knockback = CalculateKnockback(dmg, dmgDealer);
        float duration = calculateDuration(dmg);

        entity?.ReceiveKnockBack(knockback, duration);
        entityVFX?.PlayOnDamageVfx(); 
        ReduceHp(dmg);

        return true;
    }

    private bool AttackEvaded() => Random.Range(0, 100) < stats.GetEvasion();

    protected void ReduceHp(float dmg)
    {
        currentHp -= dmg;
        UpdateHPBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();

    }

    //private void UpdateHPBar() => healthBar.value = currentHp / maxHp; 

    private void UpdateHPBar()
    {
        if (healthBar  == null)
        {
            return;
        }

        healthBar.value = currentHp / stats.GetMaxHealth(); 
    }


    private Vector2 CalculateKnockback(float dmg, Transform dmgDealer)
    {
        int direction = transform.position.x > dmgDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(dmg) ? heavyKnockBackPower : knockBackPower;

        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float calculateDuration(float dmg) => IsHeavyDamage(dmg) ? heavyKnockBackDuration : knockBackDuration;
    private bool IsHeavyDamage(float dmg) => dmg / stats.GetMaxHealth() > heavyDmgThreshold;
}
