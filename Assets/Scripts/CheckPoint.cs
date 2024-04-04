using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStatus;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    [ContextMenu("Generate Checkpoint id")]
    private void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     if(collision.GetComponent<Player>() != null)
        {
            ActivateCheckPoint();
        }
    }

    public void ActivateCheckPoint()
    {
        if(activationStatus == false)
            AudioManager.instance.PlaySFX(5, transform); // i have bug here every time i collide with checkpoints it sounds 
        
        
        activationStatus = true;
        anim.SetBool("active", true);
    }
}
