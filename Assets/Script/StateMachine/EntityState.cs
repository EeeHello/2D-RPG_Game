using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rigidbody;

    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        // Evertime stage will be changed
        anim.SetBool(animBoolName, true);
        Debug.Log("I enter " + animBoolName);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        UpdateAnimationParameters();
        // Run logic of the state

    }

    public virtual void Exit()
    {
        // Everytime exit state and change to next state
        //Debug.Log("I exit " + animBoolName);
        anim.SetBool(animBoolName, false);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }

    public virtual void UpdateAnimationParameters()
    {

    }
}
