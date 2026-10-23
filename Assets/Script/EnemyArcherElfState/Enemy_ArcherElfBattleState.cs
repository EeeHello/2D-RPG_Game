using UnityEngine;

public class Enemy_ArcherElfBattleState : Enemy_BattleState
{
    private bool canFlip;
    private bool reachDeadEnd;

    public Enemy_ArcherElfBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        reachDeadEnd = false;
    }

    public override void Update()
    {
        stateTimer -= Time.deltaTime;

        UpdateAnimationParameters();

        if (enemy.isGroundDetected == false || enemy.isWallDetected)
        {
            reachDeadEnd = true;
        }

        if (enemy.PlayerDetected() == true)
            UpdateBattleTimer();

        if (BattleTimeIsOver())
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (CanAttack())
        {
            if (enemy.PlayerDetected() == false/* && canFlip*/)
            {
                enemy.HandleFlip(DirectionToPlayer());
                //canFlip = false;
            }

            enemy.SetVelocity(0, rigidbody.linearVelocity.y);

            if (WithinAttackRange() && enemy.PlayerDetected())
            {
                //canFlip = true;
                lastTimeAttacked = Time.time;
                stateMachine.ChangeState(enemy.attackState);
                return;
            }

        }
        else
        {
            bool shouldWalkAway = reachDeadEnd == false && DistanceToPlayer() < (enemy.attackDistance * 0.85f);

            if (shouldWalkAway)
            {
                enemy.SetVelocity((enemy.battleMoveSpeed * -1) * DirectionToPlayer(), rigidbody.linearVelocity.y);
            }
            else
            {
                enemy.SetVelocity(0, rigidbody.linearVelocity.y);

                if (enemy.PlayerDetected() == false)
                {
                    enemy.HandleFlip(DirectionToPlayer());   
                }
            }
        }

    }
}
