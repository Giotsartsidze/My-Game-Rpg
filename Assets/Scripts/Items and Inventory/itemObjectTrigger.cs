using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemObjectTrigger : MonoBehaviour
{
    private itemObject myItemObject => GetComponentInParent<itemObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<CharacterStats>().isDead)
            {
                return;
            }
            myItemObject.PickUpItem();
        }
    }
}
