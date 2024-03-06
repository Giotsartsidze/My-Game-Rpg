using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
   protected EnemyStateMachine stateMachine;
   protected Enemy enemyBase;
   protected Rigidbody2D rb;

   private string animBoolname;
   protected float stateTimer;
   protected bool triggerCalled;

   public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName){
    this.enemyBase = _enemyBase;
    this.stateMachine = _stateMachine;
    this.animBoolname = _animBoolName;
   }

   public virtual void Enter(){
    triggerCalled = false;
    rb = enemyBase.rb;  
    enemyBase.anim.SetBool(animBoolname, true);
   }

   public virtual void Update(){
    stateTimer -= Time.deltaTime;
   }

   public virtual void Exit(){
    enemyBase.anim.SetBool(animBoolname, false);
        enemyBase.AssignLastAnimName(animBoolname);
   }

   public virtual void AnimationFinishTrigger() {
      triggerCalled = true;
   }
}
