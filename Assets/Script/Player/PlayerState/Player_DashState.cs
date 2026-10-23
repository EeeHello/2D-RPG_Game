using UnityEngine;

public class Player_DashState : PlayerState
{
    private float originalGravityScale;
    private int dashDir; // Save value in case of facingDir changes durimg dashState

    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        dashDir = player.moveInput.x != 0 ? (int)player.moveInput.x : player.facingDir; ;
        stateTimer = player.dashDuration;

        originalGravityScale = rigidbody.gravityScale;
        rigidbody.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        CancelDashIfNeeded();

        player.SetVelocity(player.dashSpeed * dashDir, 0); 

        if (stateTimer < 0)
        {
            if (player.isGroundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.fallState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, 0);
        rigidbody.gravityScale = originalGravityScale;
    }

    private void CancelDashIfNeeded()
    {
        if (player.isWallDetected)
        {
            if (player.isGroundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.wallSlideState);
            }
        }
    }
}
