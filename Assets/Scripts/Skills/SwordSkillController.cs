using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{   
    [SerializeField] private float returnSeep = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D  cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;


    private void Awake() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _palyer){
        player = _palyer;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
    }

    public void ReturnSword(){
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
        anim.SetBool("Rotation", true);
    }

    private void Update() {
        if(canRotate){
            transform.right = rb.velocity;
        }
        if(isReturning){
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position,returnSeep * Time.deltaTime );
            if(Vector2.Distance(transform.position,player.transform.position)<1){
                player.CatchTheSword();
            }
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision) {

        anim.SetBool("Rotation",false);
        canRotate = false;
        cd.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
    }
}
