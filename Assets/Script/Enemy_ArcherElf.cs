using UnityEngine;

public class Enemy_ArcherElf : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_ArcherElfBattleState elfBattleState;

    [Header("Archer Elf Specifics")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowStartPoint;
    [SerializeField] private float arrowSpeed = 8f;

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        elfBattleState = new Enemy_ArcherElfBattleState(this, stateMachine, "battle");
        battleState = elfBattleState;
    }

    public override void SpecialAttack()
    {
        base.SpecialAttack();
        GameObject newArrow = Instantiate(arrowPrefab, arrowStartPoint.position, Quaternion.identity);
        newArrow.GetComponent<Enemy_ArcherElfArrow>().SetUpArrow(arrowSpeed * facingDir, combat);


    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public void HandleCounter()
    {
        if (CanBeCountered == false)
        {
            return;
        }
        stateMachine.ChangeState(stunnedState);
    }

}
