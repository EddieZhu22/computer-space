using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;

    [SerializeField] private GameObject player;
    void Start()
    {
        // Find player angle.
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        transform.eulerAngles = player.transform.eulerAngles;
        Destroy(gameObject, 3);
    }

    void Update()
    {
        // Find player angle.
        float force = speed; 
        float angle = player.transform.eulerAngles.z;
        float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * force;
        float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * force;
        rb.velocity = new Vector3(ycomponent,xcomponent);
        transform.eulerAngles = player.transform.eulerAngles;
    }
    //Collisions
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.name.Contains("UFO"))
        {
            coll.gameObject.GetComponent<UFO>().Restart();
            GameObject.FindObjectOfType<GameUI>().GetComponent<GameUI>().playerScore++;
            Destroy(gameObject);
        }
    }
}
