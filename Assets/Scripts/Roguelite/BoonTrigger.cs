using UnityEngine;

public class BoonTrigger : MonoBehaviour
{
    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.GetComponent<Player>() == null) return;

        triggered = true;
        BoonManager.instance.OfferBoons();
    }
}
