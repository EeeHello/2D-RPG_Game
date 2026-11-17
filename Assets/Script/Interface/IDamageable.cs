using UnityEngine;

public interface IDamageable
{
    public bool TakeDamage(float damage, float elementalDMG, ElementType element, Transform damageDealer);
}
