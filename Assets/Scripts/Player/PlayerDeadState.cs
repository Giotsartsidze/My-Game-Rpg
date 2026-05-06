using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        if (RunManager.instance != null)
        {
            RunManager.instance.EndRun();
            GameObject.Find("Canvas").GetComponent<UI>().SwitchOnDeathScreen();
        }
        else
        {
            // Fallback to original end screen if RunManager not present
            GameObject.Find("Canvas").GetComponent<UI>().SwitchOnEndScreen();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();
    }
}
