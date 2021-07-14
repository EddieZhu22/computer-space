using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject UFO1;
    [SerializeField] private float difficultyCurve;
    private float timeMove;

    private bool isMoving;

    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private Vector2 endPos;

    private float rand;
    private float screenWidth;
	private float screenHeight, forceUp, restartSeq;

    private AudioSource source;

    [SerializeField] AudioClip[] clips;

    void Start()
    {
        source = GetComponent<AudioSource>();
        transform.position = new Vector2(Random.Range(-5,5),Random.Range(-5,5));
    }

    void Update()
    {
        //Enemy "AI"
        // Basically every so often it lerps to a random location 3 tiles in front or to the side. It's pretty cool in my opinion.
        if(isMoving == false){
            startTime = Time.time;
            if(forceUp == 0){
                endPos = new Vector2(Random.Range(0, 2) == 0 ? transform.position.x - 3 : transform.position.x + 3,Random.Range(0, 2) == 0 ? transform.position.y + 3 : transform.position.y - 3);
            }
            if(forceUp == 1){
                endPos = new Vector2(Random.Range(0, 2) == 0 ? transform.position.x - 3 : transform.position.x + 3,transform.position.y + 3);
                forceUp = 0;
            }
            if(forceUp == -1){
                endPos = new Vector2(Random.Range(0, 2) == 0 ? transform.position.x - 3 : transform.position.x + 3,transform.position.y - 3);
                forceUp = 0;
            }
            if(forceUp == 2){
                endPos = new Vector2(transform.position.x - 3, Random.Range(-1, 2) == 0 ? transform.position.y + 3 : transform.position.y - 3);
            }
            if(forceUp == -2){
                endPos = new Vector2(transform.position.x + 3, Random.Range(-1, 2) == 0 ? transform.position.y + 3 : transform.position.y - 3);
            }
            isMoving = true;

            journeyLength = Vector3.Distance(transform.position, endPos);

            rand = Random.Range(0,5);

            StartCoroutine(MoveTime());
        }
        
        float distCovered = (Time.time - startTime) * speed;

        float fractionOfJourney = distCovered / journeyLength;

        var viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Checks on What to do when approaching an edge

        if(transform.position.x > 12)
        {
            forceUp = 2;
        }
        if(transform.position.x < -12)
        {
            forceUp = -2;
        }
        if(transform.position.y > 2)
        {
            forceUp = -1;
        }
        if(transform.position.y < -4)
        {
            forceUp = 1;
        }
        // Lerp action
        transform.position = Vector3.Lerp(transform.position,endPos, fractionOfJourney);
    }
    //Move Timer change difficulty curve or number to change it.
    // If you want to remove it, remove the difficulty curve variable from the script
    private IEnumerator MoveTime()
    {
        if(rand > 2 * difficultyCurve)
        {
            yield return new WaitForSeconds(0.5f * difficultyCurve);
            Shoot();
        }
        yield return new WaitForSeconds(rand * difficultyCurve);
        isMoving = false;
    }
    //Shooting
    private void Shoot()
    {
        if(restartSeq != 1){
            Instantiate(bullet,transform.position,Quaternion.identity);
            source.clip = clips[0];
            source.Play();
        }
    }
    //Restarting
    public void Restart()
    {
        source.clip = clips[1];
        source.Play();
        if(GameObject.FindObjectOfType<GameUI>().GetComponent<GameUI>().playerScore % 3 == 0){
            difficultyCurve -= 0.1f;
        }
        StartCoroutine(IRestart());
    }
    // Restart Timer
    // Destroy Effect
    private IEnumerator IRestart()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        UFO1.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        UFO1.GetComponent<PolygonCollider2D>().enabled = false;
        restartSeq = 1;
        // Wait Time
        yield return new WaitForSeconds(2f);
        restartSeq = 0;
        UFO1.GetComponent<SpriteRenderer>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        GetComponent<PolygonCollider2D>().enabled = true;
        UFO1.GetComponent<PolygonCollider2D>().enabled = true;
        endPos = new Vector2(Random.Range(-5,5),Random.Range(-5,5));
    }
}
