using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVFX;

    private ElementType currentEffect = ElementType.None;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
    }

    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        entity.SlowEntity(duration, slowMultiplier);
        StartCoroutine(ChilledEffectCo(duration));
    }

    private IEnumerator ChilledEffectCo(float duration)
    {
        currentEffect = ElementType.Ice; // Apply VFX

        entityVFX.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);

        currentEffect = ElementType.None; // Stop VFX


    }

    public bool CanBeApplied(ElementType element)
    {
        return currentEffect == ElementType.None;
    }
}
