using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;

    //private Entity_SFX sfx;
    private Entity_VFX vfx;
    //private Entity_Stats stats;
    //

    //public DamageScaleData basicAttackScale;

    public float dmg = 10f;

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetcheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    private void Awake()
    {
        // Get components of sfx, vfx and stats later
        vfx = GetComponent<Entity_VFX>();
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

            damageable.TakeDamage(dmg, transform);
            vfx.CreateOnHItVFX(target.transform);
        }
    }

    public void PerformAttackOnTarget(Transform target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable == null)
        {
            return;
        }

        damageable.TakeDamage(dmg, transform);
        vfx.CreateOnHItVFX(target.transform);
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
