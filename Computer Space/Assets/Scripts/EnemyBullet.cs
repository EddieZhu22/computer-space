using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;

    [SerializeField] private GameObject player;
    void Start()
    {
        // find the direction of the player and shoot towards it. Remember to normalize vector for numbers between 0 and 1
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        Vector2 moveDirection = (player.transform.position - transform.position).normalized * speed;
		rb.velocity = new Vector2 (moveDirection.x, moveDirection.y);
        Destroy(gameObject, 3);
    }
}
