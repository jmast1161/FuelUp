using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip fireClip;

    [SerializeField]
    private AudioClip hitClip;

    //[SerializeField]
    //private Collider2D collider;

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

        if(Input.GetKeyDown(KeyCode.Space))
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
        if (col.gameObject.name == "Enemy")
        {   
            Destroy(gameObject);
            // game over
        }
    }

    private void Fire()
    {
        var collider = GetComponent<PolygonCollider2D>();
        var center = collider.bounds.center.x;
        var height = collider.bounds.size.y;
        Debug.Log("center: " + center.ToString());
        Debug.Log("height: " + height.ToString());
        Debug.Log("transform x: " + transform.position.x.ToString());
        Debug.Log("transform.y: " + transform.position.y.ToString());
        var bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x, transform.position.y + height), Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * fireForce, ForceMode2D.Impulse);
        //audioSource.PlayOneShot(fireClip);
    }
}
