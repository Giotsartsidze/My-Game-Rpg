using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class Clone_Skill_Controller : MonoBehaviour
{   
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosinSpeed;
    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closestEnemy;
    private int facingDir = 1;


    private bool canDuplicateClone;
    private float chanceToDuplicate;
    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update() {
        cloneTimer -= Time.deltaTime;
        if(cloneTimer < 0){
            sr.color = new Color(1,1,1,  sr.color.a - (Time.deltaTime * colorLoosinSpeed));
            if(sr.color.a <= 0){
                Destroy(gameObject);
            }
        }
    }
    public void SetUpClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _chanceToDuplicate) { 
        if(_canAttack){
            anim.SetInteger("AttackNumber", Random.Range(1,3));
            Debug.Log(Random.Range(1,4));
        }
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;

        FaceClosestTarget();
    }

     private void AnimationTrigger(){
       cloneTimer = -.1f;
    }

    private void AttackTrigger(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach(var hit in colliders){
            if(hit.GetComponent<Enemy>() != null){
                hit.GetComponent<Enemy>().DamageEffect();

                if (canDuplicateClone)
                {
                    if(Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget(){
        if(closestEnemy != null){
            if(transform.position.x > closestEnemy.position.x){
                facingDir = -1;
                transform.Rotate(0,180,0);
            }
        }
    }
}