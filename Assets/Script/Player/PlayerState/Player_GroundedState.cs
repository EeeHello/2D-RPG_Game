using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (rigidbody.linearVelocity.y < 0 && player.isGroundDetected == false)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (playerInputSet.Player.Jump.WasPressedThisFrame() && player.isGroundDetected)
        {
            Debug.Log("Jump!");
            stateMachine.ChangeState(player.jumpState);
        }

        if (playerInputSet.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.basicAttackState);
        }

        if (playerInputSet.Player.CounterAttack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.counterAttackState);
        }
    }
}
