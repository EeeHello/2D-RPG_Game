using UnityEngine;

public class Player_FallState : Player_AirState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        // if detect ground below, YES -> go to idle state
        if (player.isGroundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.isWallDetected)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
