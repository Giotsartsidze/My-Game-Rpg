using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PlayerPrimaryAttack(Player _player, PlayerStatemachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0; //we need this for bugfix off atack direction
        if(comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow){
            comboCounter = 0;
        }
        player.anim.SetInteger("ComboCounter", comboCounter);
        // player.anim.speed = 1.2f;
        float attackDir = player.facingDir;
        if(xInput != 0){
            attackDir = xInput;
        }
        player.SetVelocity(player.attackMovemnt[comboCounter].x * attackDir, player.attackMovemnt[comboCounter].y);
        stateTimer = 0.1f;
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer < 0){
            player.ZeroVelocity();
        }
        if(triggerCalled){
            stateMachine.ChangeState(player.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .15f);
        comboCounter++;
        lastTimeAttacked = Time.time;
    }
}
