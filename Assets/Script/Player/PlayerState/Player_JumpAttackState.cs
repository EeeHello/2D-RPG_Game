using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    private bool touchGround;

    public Player_JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        touchGround = false;
        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.isGroundDetected && touchGround == false)
        {
            touchGround = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0, rigidbody.linearVelocity.y);
        }

        if (triggerCalled && player.isGroundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
