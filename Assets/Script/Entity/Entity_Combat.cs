using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;

    //private Entity_SFX sfx;
    private Entity_VFX vfx;
    private Entity_Stats stats;
    //

    //public DamageScaleData basicAttackScale;

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetcheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    [Header("Status effect details")]
    [SerializeField] private float defaultDuration = 3f;
    [SerializeField] private float chillSlowMultiplier = 0.2f;
 
    private void Awake()
    {
        // Get components of sfx, vfx and stats later
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders(whatIsTarget))
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
            {
                continue;
            }

            float damage = stats.GetPhysicalDamage(out bool isCrit);
            float eDamage = stats.GetElementalDamage(out ElementType element);

            bool targetGotHit = damageable.TakeDamage(damage, eDamage, element, transform);

            if (element != ElementType.None)
            {
                ApplyStatusEffect(target.transform, element);
            }

            if (targetGotHit)
            {
                vfx.UpdateOnHitColor(element);
                vfx.CreateOnHItVFX(target.transform, isCrit);
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType element, float scaleFactor = 1)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>(); 

        if (statusHandler == null)
        {
            return;
        }

        if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
        {
            statusHandler.ApplyChilledEffect(defaultDuration, chillSlowMultiplier * scaleFactor);
            Debug.Log("Slow is " + chillSlowMultiplier);
        }

        if (element == ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire))
        {
            float fireDMG = stats.offense.fireDMG.GetValue() * scaleFactor;
            statusHandler.ApplyBurnedEffect(defaultDuration, fireDMG);
            Debug.Log("Burn is " + fireDMG);
        }

    }

    public void PerformAttackOnTarget(Transform target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable == null)
        {
            return;
        }

        float damage = stats.GetPhysicalDamage(out bool isCrit);
        float eDamage = stats.GetElementalDamage(out ElementType element);

        bool targetGotHit = damageable.TakeDamage(damage, eDamage, element, transform);

        if (element != ElementType.None)
        {
            ApplyStatusEffect(target.transform, element);
        }

        if (targetGotHit)
        {
            vfx.UpdateOnHitColor(element);
            vfx.CreateOnHItVFX(target.transform, isCrit);
        }
    }

    protected Collider2D[] GetDetectedColliders(LayerMask whatToDetect)
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetcheckRadius, whatToDetect);


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetcheckRadius);
    }


}
