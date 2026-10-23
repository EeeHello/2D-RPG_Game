using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    protected Transform player;
    protected float lastTimeWasInBattle;
    protected float lastTimeAttacked;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();

        Debug.Log("I entered battle");

        if (player == null)
        {
            player = enemy.GetPlayerReference();
        }

        if (ShouldRetreat())
        {
            rigidbody.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected() == true)
            UpdateBattleTimer();

        if (BattleTimeIsOver())
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (WithinAttackRange() && enemy.PlayerDetected() && CanAttack())
        {
            stateMachine.ChangeState(enemy.attackState);
            return;
        }

        if (enemy.isWallDetected)
        {
            enemy.SetVelocity(0, rigidbody.linearVelocity.y);
        }
        else
        {
            float xVelocity = enemy.canChasePlayer ? enemy.battleMoveSpeed * DirectionToPlayer() : 0.0001f;
            enemy.SetVelocity(xVelocity, rigidbody.linearVelocity.y);
        }
    }

    protected bool CanAttack() => Time.time > lastTimeAttacked + enemy.attackCD;

    protected void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;

    protected bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;

    protected bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;

    protected bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    protected float DistanceToPlayer()
    {
        if (player == null)
        {
            return float.MaxValue;
        }

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    protected int DirectionToPlayer()
    {
        if (player == null)
        {
            return 0;
        }

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
