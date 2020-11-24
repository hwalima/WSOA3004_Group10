using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIOne : MonoBehaviour
{
   Transform target;
    public float speed=200f;
    public float nextWayPointDistance = 3f;

    Path path;
    int currentWayPoint = 0;
    bool endOfPath = false;

    Seeker seeker;
    Rigidbody2D rb2d;

    Transform findPlayer;

    bool aiAwake=false;
    float radiusOfCircle=3f;

    bool isChasingPlayer=false;

    public float maxChaseTime = 20f;
    float chaseTimer = 20f;
   // public Transform newSleepSpot;

    public GameObject areaObj1;
    public GameObject areaObj2;

    Vector2 startPosition;

    bool isRock=false;
    public Transform rockDetectObj;
    float rockdetectRadius = 0.25f;
    public LayerMask rockLayer;

    public GameObject aiChaseLight;

    bool moveToStart = false;

    Animator animator;
    //checks if there is that rock that stops the bats from pursuing the player;
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("UpdatePath", 0, 0.5f);
        InvokeRepeating("ToggleAwakenness", 0, Random.Range(0.5f,2));
        chaseTimer = maxChaseTime;
        startPosition = transform.position;
        aiChaseLight.SetActive(false);
        animator = GetComponent<Animator>();
        animator.Play("idle");
    }

    private void FixedUpdate()
    {
        if (isChasingPlayer)
        {
            if (target == null)
            {
                isChasingPlayer = false;
                StartCoroutine(FindPlayerWait());
                if (findPlayer != null)
                {
                    target = findPlayer;
                    animator.Play("chase");
                }
            }

            if (target != null)
            {
                if (path == null)
                {
                    return;
                }
                if (currentWayPoint >= path.vectorPath.Count)
                {
                    endOfPath = true;
                    return;
                }

                else
                {
                    endOfPath = false;
                }
                Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb2d.position).normalized;
                Vector2 force = direction * speed * Time.deltaTime;
                rb2d.AddForce(force);
                float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWayPoint]);
                if (distance < nextWayPointDistance)
                {
                    currentWayPoint++;
                }

                if (force.x >= 0.01f)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);

                }
                else if (force.x <= 0.01f)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }

           

          

           
            // chaseTimer -= Time.deltaTime;
            isRock = Physics2D.OverlapCircle(rockDetectObj.position, rockdetectRadius, rockLayer);
            if (isRock)
            {
              
                 isChasingPlayer = false;
                moveToStart = true;
                
            }

            if (transform.position.y > 115)
            {
                seeker.StartPath(rb2d.position, startPosition, OnPathComplete);
                isChasingPlayer = false;

            }
        }
        if (moveToStart)
        {
            seeker.StartPath(rb2d.position, startPosition, OnPathComplete);
        }
        
        if (target == null)
        {
            transform.position = startPosition;
            isChasingPlayer = false;
        }
        while (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (aiAwake)
        {
            //Collider2D[] whatIsInSphere = Physics2D.OverlapCircleAll(transform.position, radiusOfCircle);
            Vector2 theAreaObj1 = new Vector2(areaObj1.transform.position.x, areaObj1.transform.position.y);
            Vector2 theAreaObj2 = new Vector2(areaObj2.transform.position.x, areaObj2.transform.position.y);

            Collider2D[] whatIsInRectangularArea=Physics2D.OverlapAreaAll(theAreaObj1,theAreaObj2);
            //flash a light or something
            foreach (Collider2D seekPlayer in whatIsInRectangularArea)
            {
                if (seekPlayer.gameObject.tag == "Player")
                {
                    isChasingPlayer = true;                   
                }
            }
        }
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (target != null)
            {
                seeker.StartPath(rb2d.position, target.position, OnPathComplete);
            }

        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
  IEnumerator FindPlayerWait()
    {
        yield return new WaitForSeconds(0.56f);
        findPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = startPosition;
    }
    void ToggleAwakenness()
    {
        aiAwake = !aiAwake;
        if (aiAwake)
        {
            aiChaseLight.SetActive(true);
            
        }
        else
        {
            aiChaseLight.SetActive(false);
        }
    }
    

    /*  void OnDrawGizmosSelected()
      {
          if (aiAwake)
          {  // Draw a yellow sphere at the transform's position
              Gizmos.color = Color.yellow;
              Gizmos.DrawSphere(transform.position, radiusOfCircle);
          }
      }*/
}





#region OldScript
/*
   public bool chase=false;
   bool playerPositionLocked = false;
   int maxChases=3;
   int currentChases=0;
   float speed = 50f;
   GameObject player;

   Vector3 playersCurrentPosition;
   float screamTimeBeforeChasingPlayer;
   // Start is called before the first frame update
   void Start()
   {
       player = GameObject.FindGameObjectWithTag("Player");
   }

   // Update is called once per frame
   void Update()
   {
       if (chase)
       {
           if (currentChases <= maxChases)
           {
               if (!playerPositionLocked)
               {
                   playersCurrentPosition = player.transform.position;
                   playerPositionLocked = true;
                   currentChases++;
               }

               transform.position = Vector3.MoveTowards(transform.position, playersCurrentPosition, speed * Time.deltaTime);


               if (transform.position == playersCurrentPosition)
               {
                   chase = false;
                   playerPositionLocked = false;
               }
           }
           if (currentChases > maxChases)
           {
               //fly away;
           }
       }

   }

   IEnumerator DramaticPauseTime(float screamTime)
   {
       yield return new WaitForSeconds(screamTime);
       chase = true;
   }
   */


/*  if (chaseTimer <= 0)
   {
       //go find a spot to go back to sleep
       seeker.StartPath(rb2d.position, newSleepSpot.position, OnPathComplete);
       //currentWayPoint = 0;
       if (currentWayPoint >= path.vectorPath.Count)
       {
           endOfPath = true;
           chaseTimer = maxChaseTime;
           return;
       }
       else
       {
           endOfPath = false;
       }
       Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb2d.position).normalized;
       Vector2 force = direction * speed * Time.deltaTime;
       rb2d.AddForce(force);
       float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWayPoint]);
       if (distance < nextWayPointDistance)
       {
           currentWayPoint++;
       }


       isChasingPlayer = false;

   }
   */
#endregion