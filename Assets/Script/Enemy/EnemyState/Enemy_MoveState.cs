using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        if (enemy.isGroundDetected == false || enemy.isWallDetected)
        {
            enemy.Flip();
        }
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rigidbody.linearVelocity.y);

        if (enemy.isGroundDetected == false || enemy.isWallDetected)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
