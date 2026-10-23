using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        anim.enabled = false;


        rigidbody.gravityScale = 12f;
        rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 15f);

        enemy.GetComponent<Collider2D>().enabled = false;

        stateMachine.SwitchOffStateMachine();
    }
}
