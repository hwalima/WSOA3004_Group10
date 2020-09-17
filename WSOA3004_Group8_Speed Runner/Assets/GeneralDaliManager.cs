using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneralDaliManager : MonoBehaviour
{
    public GameObject babyDali;
    public GameObject momDali;
    public GameObject dadDali;
    


    int attackNo = 0;

    List<DaliHeadControl> orderOfAttack = new List<DaliHeadControl>();

    public bool next=false;

    bool beginAttacking = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!beginAttacking)
        {
            if (babyDali.GetComponent<DaliHeadControl>().isAttacking==true)
            {
                Debug.Log("here");
                orderOfAttack.Add(babyDali.GetComponent<DaliHeadControl>());
                orderOfAttack.Add(momDali.GetComponent<DaliHeadControl>());
                orderOfAttack.Add(dadDali.GetComponent<DaliHeadControl>());
                beginAttacking = true;
            }
            else if (momDali.GetComponent<DaliHeadControl>().isAttacking==true)
            {                
                orderOfAttack.Add(momDali.GetComponent<DaliHeadControl>());
                orderOfAttack.Add(babyDali.GetComponent<DaliHeadControl>());
                orderOfAttack.Add(dadDali.GetComponent<DaliHeadControl>());
                beginAttacking = true;
            }
            else if (dadDali.GetComponent<DaliHeadControl>().isAttacking==true)
            {
                orderOfAttack.Add(dadDali.GetComponent<DaliHeadControl>());
                orderOfAttack.Add(babyDali.GetComponent<DaliHeadControl>());
                orderOfAttack.Add(momDali.GetComponent<DaliHeadControl>());
                beginAttacking = true;
            }
            

        }

        if (orderOfAttack.Count > 0)
        {

            if (next)
            {
                orderOfAttack.Remove(orderOfAttack[attackNo]);
                orderOfAttack[attackNo].FindPlayer();
                next = false;
               
            }
            for (int i = 0; i < orderOfAttack.Count-1; i++)
            {
                if (i == attackNo)
                {
                    orderOfAttack[i].isAttacking = true;
                    orderOfAttack[i].waitBeforeAttacking = false;
                }
                else
                {
                    orderOfAttack[i].isAttacking = false;
                    orderOfAttack[i].waitBeforeAttacking = true;
                }

            }

           
        }

        if (orderOfAttack.Count <= 0)
        {
           
            if (beginAttacking == true)
            {
                Debug.Log("there are " + orderOfAttack.Count + " objects left");
                Destroy(gameObject);
            }
        }

        
    }
}
