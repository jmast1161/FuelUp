using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private Rigidbody2D rigidBody;


    private Vector2 moveDirection;

    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    private float fireForce = 20f;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private AudioSource fireAudioSource;

    public event Action<Player> PlayerHit;

    public event Action<Player> ShieldPickup;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2 - 1f;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2 - 1f;
    }

    // Update is called once per frame
    void Update()
    {
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void FixedUpdate() 
    {
        rigidBody.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void LateUpdate() 
    {
        var viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 - objectWidth, screenBounds.x + objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y,  screenBounds.y * -1 - objectHeight, screenBounds.y + objectHeight);
        transform.position = viewPos;
    }

    private void OnCollisionEnter2D(Collision2D col) 
    {
        if (col.gameObject.tag == "Meteor")
        {
            Destroy(gameObject);
            PlayerHit?.Invoke(this);
        }

        if(col.gameObject.tag == "Shield")
        {
            ShieldPickup?.Invoke(this);
        }
    }

    private void Fire()
    {
        var collider = GetComponent<PolygonCollider2D>();
        var height = collider.bounds.size.y;
        var bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x, transform.position.y + height), Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * fireForce, ForceMode2D.Impulse);
        fireAudioSource.Play();
    }
}
