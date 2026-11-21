using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_Stats entityStats;
    private Entity_VFX entityVFX;
    private Entity_Health entityHealth;

    private ElementType currentEffect = ElementType.None;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityHealth = GetComponent<Entity_Health>();
        entityStats = GetComponent<Entity_Stats>();
        entityVFX = GetComponent<Entity_VFX>();
    }

    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        float iceRES = entityStats.GetElementalRES(ElementType.Ice);
        float finalDuration = duration * (1 - iceRES);

        StartCoroutine(ChilledEffectCo(finalDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration, float slowMultiplier)
    {
        entity.SlowEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice; // Apply VFX
        entityVFX.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);

        currentEffect = ElementType.None; // Stop VFX


    }

    public void ApplyBurnedEffect(float duration, float fireDMG)
    {
        float fireRES = entityStats.GetElementalRES(ElementType.Fire);
        float finalBurnDMG = fireDMG * (1 - fireRES);

        StartCoroutine(BurnedEffectCo(duration, finalBurnDMG));
    }


    private IEnumerator BurnedEffectCo(float duration, float totalDMG)
    {

        currentEffect = ElementType.Fire; // Apply VFX
        entityVFX.PlayOnStatusVfx(duration, ElementType.Fire);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDMG / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            // reduce HP
            entityHealth.ReduceHp(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None; // Stop VFX
    }

    public bool CanBeApplied(ElementType element)
    {
        return currentEffect == ElementType.None;
    }
}
