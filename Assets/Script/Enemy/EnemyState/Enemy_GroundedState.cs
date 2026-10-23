using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // if player detected 
        // stateM change to battle state

        if (enemy.PlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
