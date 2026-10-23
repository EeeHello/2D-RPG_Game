using UnityEngine;

public class Player_JumpState : Player_AirState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Make obj go up, increase Y velo
        player.SetVelocity(rigidbody.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        // if Y velo goes down, chr falls -> to fallstate
        if (rigidbody.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState) // Need to make sure we are not in jump attack state when we transfer to fall state
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
