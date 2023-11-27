using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.velocity = new Vector2(0, -2.5f);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag != "Bullet")
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}
