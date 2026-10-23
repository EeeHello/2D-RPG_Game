using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entityCombat;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<Entity_Combat>();
    }

    private void CurrentStateTrigger()
    {
        // get access to player and let currnet player's state we want to exit state
        entity.CurrentStateAnimationTrigger();
    }

    private void AttackTrigger()
    {
        //Debug.Log("I perform Target detection");
        entityCombat.PerformAttack();
    }
}
