using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

     [Header("Collision Info")]
     public Transform attackCheck;
    public float attackCheckRadius; 
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;


    public int facingDir {get; private set;} = 1;
    protected bool facingRight = true;


    #region Components
    public Animator anim {get; private set;}
    public Rigidbody2D rb{get; private set;}
    public EntityFX fx {get; private set;}
    public SpriteRenderer sr { get; private set; }

    public CharacterStats stats { get; private set; }
    #endregion
    [Header("Knockback Info")]  
    [SerializeField] protected Vector2 knockBackDirection;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnocked;

    protected virtual void Awake() {
        
    }

    protected virtual void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        stats = GetComponent<CharacterStats>();
    }

    protected virtual void Update(){
        
    }

    public virtual void DamageEffect(){
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack");
      //Debug.Log(gameObject.name + "Was damaged");
    }

    protected virtual IEnumerator HitKnockBack(){
        isKnocked = true;
        rb.velocity = new Vector2(knockBackDirection.x * -facingDir,knockBackDirection.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
    }

    #region Collision
   public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
   public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

   protected virtual void OnDrawGizmos() {
    Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance ));
    Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y ));
    Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
   }
    #endregion
    #region Flip
    public void Flip(){
    facingDir = facingDir * -1;
    facingRight = !facingRight;
    transform.Rotate(0, 180, 0);
   }
   public void FlipController( float _x){
    if(_x > 0 && !facingRight){
        Flip();
    }else if(_x < 0 && facingRight){
        Flip();
    }
   }
   #endregion
    #region Velocity
      public virtual void ZeroVelocity(){
        if(isKnocked){
            return;
        }
           rb.velocity = new Vector2(0,0);
    }
   public virtual void SetVelocity(float _xVelocity, float _yVelocity){
    if(isKnocked){
      return;  
    }
    rb.velocity = new Vector2(_xVelocity,_yVelocity);
    FlipController(_xVelocity);
   }
   #endregion

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

}
