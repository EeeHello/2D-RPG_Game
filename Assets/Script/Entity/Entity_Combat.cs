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
            bool targetGotHit = damageable.TakeDamage(damage, transform);

            if (targetGotHit)
            {
                vfx.CreateOnHItVFX(target.transform, isCrit);
            }
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
        bool targetGotHit = damageable.TakeDamage(damage, transform);

        if (targetGotHit)
        {
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
