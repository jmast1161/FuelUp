using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidBody;

    private Vector2 velocty;

    public void Init(Vector2 velocity) =>
        this.velocty = velocity;

    public event Action<Meteor> MeteorHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.velocity = velocty;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
            MeteorHit?.Invoke(this);
        }
    }

    private void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}
