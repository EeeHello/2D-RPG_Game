using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private Player_Combat combat;
    private bool hasPerformedCounter;

    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = combat.GetCounterRecoveryDuration();
        hasPerformedCounter = combat.CounterAttackPerformed();

        anim.SetBool("counterAttackPerformed", hasPerformedCounter);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, rigidbody.linearVelocity.y);

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (stateTimer < 0 && hasPerformedCounter == false)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }


}
