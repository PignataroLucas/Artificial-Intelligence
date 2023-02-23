public class DeadState <T> : States<T>
{
    private AI _ai;

    public DeadState(AI ai){ _ai = ai; }

    public override void OnEnter()
    {
        _ai.animator.SetBool("deadth",true);
        _ai.animator.SetBool("canAttack",false);
        _ai.animator.SetBool("ToIdleAttack",false);
    }
}
