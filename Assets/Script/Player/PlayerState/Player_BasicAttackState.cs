using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    // Const variable always start with CAP Letter
    private const int FirstComboIndex = 1; // alway start combo index with number 1, is use in anim parameter
    private int comboIndex = 1;
    private int comboLimit = 3;
    private int attackDir;

    private float attackVelocityTimer;
    private float lastTimeAttacked;

    private bool comboAttackQueued;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (comboLimit != player.attackVelocity.Length)
        {
            Debug.LogWarning("Adjusted combo limit to match attack velocity array!");
            comboLimit = player.attackVelocity.Length;
        }
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;

        ResetComboIndexIfNeeded();

        // attackDir results equal
        //        -> (if) Question         -> (if) YES               -> (if) NO
        attackDir = player.moveInput.x != 0 ? (int)player.moveInput.x : player.facingDir;

        // This IF statement can be rewritten like the line above!
        //if (player.moveInput.x != 0)
        //{
        //    attackDir = ((int)player.moveInput.x);
        //}
        //else
        //{
        //    attackDir = player.facingDir;
        //}
            anim.SetInteger("basicAttackIndex", comboIndex);

        ApplyAttackVelocity();
    }


    public override void Update()
    {
        base.Update();

        HandleAttackVelocity();

        // Detect and damage enemies

        if (playerInputSet.Player.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }

        if(triggerCalled)
        {
            HandleStateExit();
        }
    }

    public override void Exit()
    {
        base.Exit();

        comboIndex++;

        lastTimeAttacked = Time.time;
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateDelay();
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, rigidbody.linearVelocity.y);
        }
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];

        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }

    private void ResetComboIndexIfNeeded()
    {
        // if time we attacked was long ago,
        // reset the combo 

        if (comboIndex > comboLimit || Time.time > lastTimeAttacked + player.comboResetTime)
        {
            comboIndex = FirstComboIndex;
        }
    }

    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
        {
            comboAttackQueued = true;
        }
    }
}
