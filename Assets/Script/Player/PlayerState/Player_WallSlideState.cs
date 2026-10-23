using Unity.VisualScripting;
using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        HandleWallSlide();

        if (playerInputSet.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }

        if (player.isWallDetected == false)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (player.isGroundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            
            if (player.facingDir != player.moveInput.x)
            {
                player.Flip();
            }

        }
    }

    private void HandleWallSlide()
    {
        if(player.moveInput.y < 0)
        {
            player.SetVelocity(player.moveInput.x, rigidbody.linearVelocity.y);
        }
        else
        {
            player.SetVelocity(player.moveInput.x, rigidbody.linearVelocity.y * player.wallSlideSlowMultiplier);
        }
    }
}
