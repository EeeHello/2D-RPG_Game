using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet playerInputSet;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.animator;
        rigidbody = player.rb;
        playerInputSet = player.input;
    }

    public override void Update()
    {
        base.Update();

        if (playerInputSet.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        anim.SetFloat("yVelocity", rigidbody.linearVelocity.y);
    }

    private bool CanDash()
    {
        if (player.isWallDetected)
        {
            return false;
        }

        if (stateMachine.currentState == player.dashState)
        {
            return false;
        }

        return true;
    }
}
