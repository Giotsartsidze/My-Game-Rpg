using UnityEngine;

public class RoomGate : MonoBehaviour
{
    private Collider2D gateCollider;
    private SpriteRenderer sr;

    private void Awake()
    {
        gateCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Close()
    {
        gateCollider.enabled = true;
        if (sr != null) sr.enabled = true;
    }

    public void Open()
    {
        gateCollider.enabled = false;
        if (sr != null) sr.enabled = false;
    }
}
