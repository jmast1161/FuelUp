using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag != "Shield")
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}
