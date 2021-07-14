using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed, thrust, maxThrust;
    [SerializeField] GameObject bullet;

    [SerializeField] GameObject ps;
    [SerializeField] Transform shotPoint;
    
    private int shotTime, restartSeq;

    private AudioSource source;

    [SerializeField] AudioClip[] clips;
    void Start()
    {
        source = GetComponent<AudioSource>();

        // Setting Max Thrust for next round (when dead)

        maxThrust = thrust;
        // Just me messing around with try catch
        try{
            rb = GetComponent<Rigidbody2D>();
        }
        catch{
            Debug.Log("Add rigid body");
        }
    }

    void Update()
    {
        // If the player isnt in it's destroy state
        if(restartSeq == 0)
        {
            // Movement!
            if(Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0,0,1 * Time.deltaTime * 100);
            }
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0,0,-1* Time.deltaTime * 100);
            }
            if(Input.GetKey(KeyCode.UpArrow))
            {
                if(thrust > 0)
                {
                    ps.SetActive(true);
                    AddForceAtAngle(speed * Time.deltaTime * 100, transform.eulerAngles.z);
                    thrust--;
                }

            }
            else
            {
                ps.SetActive(false);
            }
            // shooting
            if(Input.GetKey(KeyCode.Space))
            {
                Shoot();
            }
        }
        else if(restartSeq == 1)
        {
            transform.Rotate(0,0,10 * Time.deltaTime * 100);
        }

    }
    // Find vectors when moving
    // split into x and y components. Change the speed variable to change how fast the player changes velocity.
    private void AddForceAtAngle(float force, float angle)
    {
        float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * force;
        float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * force;
        rb.AddForce(new Vector2(ycomponent, xcomponent));
    }
    //Shooting
    private void Shoot()
    {
        if(shotTime == 0)
        {
            source.clip = clips[0];
            source.Play();
            StartCoroutine(IShootTimer());
        }
    }
    // Restarting
    private void Restart()
    {
        GameObject.FindObjectOfType<GameUI>().GetComponent<GameUI>().enemyScore++;
        transform.position = new Vector2(Random.Range(-5,5),Random.Range(-5,5));
        thrust = maxThrust;
        rb.velocity = Vector2.zero;
        GetComponent<PolygonCollider2D>().enabled = true;
    }
    // Collision detection of Enemy
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.name.Contains("Bullet"))
        {
            source.clip = clips[1];
            source.Play();
            StartCoroutine(IRestart());
            Destroy(coll.gameObject);
        }
    }
    //Shoot Timer
    private IEnumerator IShootTimer()
    {
        Instantiate(bullet,shotPoint.position,Quaternion.identity);
        shotTime = 1;
        yield return new WaitForSeconds(2f);
        shotTime = 0;
    }
    // Restart Timer
    private IEnumerator IRestart()
    {
        restartSeq = 1;
        ps.SetActive(false);
        GetComponent<PolygonCollider2D>().enabled = false;
        yield return new WaitForSeconds(3f);
        restartSeq = 0;
        Restart();
    }
}
