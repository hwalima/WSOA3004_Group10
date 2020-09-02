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
    public Transform newSleepSpot;
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("UpdatePath", 0, 0.5f);
        InvokeRepeating("ToggleAwakenness", 0, 2);
        chaseTimer = maxChaseTime;
    }

    private void FixedUpdate()
    {
        if (isChasingPlayer)
        {
            if (target == null)
            {
                StartCoroutine(FindPlayerWait());
                if (findPlayer != null)
                {
                    target = findPlayer;
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
            chaseTimer -= Time.deltaTime;
            
        }

        if (chaseTimer <= 0)
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

        if (aiAwake)
        {
            Collider2D[] whatIsInSphere = Physics2D.OverlapCircleAll(transform.position, radiusOfCircle);
            foreach (Collider2D seekPlayer in whatIsInSphere)
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
    }
    void ToggleAwakenness()
    {
        aiAwake = !aiAwake;
    }

    void OnDrawGizmosSelected()
    {
        if (aiAwake)
        {  // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, radiusOfCircle);
        }
    }
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
#endregion
