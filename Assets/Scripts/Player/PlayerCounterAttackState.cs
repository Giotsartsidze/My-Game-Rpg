using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStatemachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }
    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10; //any value bigger than 1
                    player.anim.SetBool("SuccessfulCounterAttack", true);
                   // if (canCreateClone)
                   // {
                    //    canCreateClone = fal
                        player.skill.clone.CreateCloneOnCounterAttack(hit.transform);
                    //}
                }
            }
        }
        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
