using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();

    public override void TakeDamage(float dmg, Transform dmgDealer)
    {
        // if dd == player
        // enemy.player = dd
        base.TakeDamage(dmg, dmgDealer);

        if (isDead)
        {
            return;
        }

        if (dmgDealer.GetComponent<Player>() != null)
        {
            enemy.TryEnterBattleState(dmgDealer);
        }
    }
}
