using UnityEngine;

public class Player_AirState : PlayerState
{
    public Player_AirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x  != 0)
        {
            player.SetVelocity(player.moveInput.x * (player.moveSpeed * player.inAirMultiplier), rigidbody.linearVelocity.y);
        }

        if (playerInputSet.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpAttackState);    
        }

        // Perrhaps in the future 
        // Allow player to parry/counter in Airstate
        //if (playerInputSet.Player.CounterAttack.WasPressedThisFrame())
        //{
        //    stateMachine.ChangeState(player.counterAttackState);
        //}
    }
}
