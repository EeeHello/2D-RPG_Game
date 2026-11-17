using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();

    public override bool TakeDamage(float dmg, float elementalDMG, ElementType element, Transform dmgDealer)
    {
        // if dd == player
        // enemy.player = dd
        bool wasHit = base.TakeDamage(dmg, elementalDMG, element, dmgDealer);

        Debug.Log($"Enemy received attack from: {dmgDealer.name}");

        if (wasHit == false)
        {
            Debug.Log("Base damage failed.");
            return false;
        }

        Debug.Log($"Damage applied. Current HP: {currentHp}");

        if (dmgDealer.GetComponent<Player>() != null)
        {
            Debug.Log("Player attacked enemy. Entering battle state.");
            enemy.TryEnterBattleState(dmgDealer);
        }

        return true;
    }
}
