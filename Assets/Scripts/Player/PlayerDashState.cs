using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStatemachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.clone.CreateClone(player.transform, Vector2.zero);
        stateTimer = player.dashDuration;
    }
    public override void Update()
    {
        base.Update();
        if(!player.IsGroundDetected() && player.IsWallDetected()){
            stateMachine.ChangeState(player.wallSlide);
        }
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        if(stateTimer < 0){
            stateMachine.ChangeState(player.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.velocity.y);
    }
}
