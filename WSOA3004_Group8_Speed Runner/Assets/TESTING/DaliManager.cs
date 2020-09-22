using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaliManager : MonoBehaviour
{
    public GameObject DaliFam1;
    //public GameObject DaliFam1Ativator;
    bool daliFam1Activated=false;


    public GameObject DaliFam2;
    bool daliFam2Activated = false;

    public GameObject DaliFam3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!daliFam1Activated)

        {
            Vector2 f1p1 = new Vector2(168.22f, 47f);
            Vector2 f1p2 = new Vector2(172.9f, 41.98f);
            Collider2D[] findPlayer = Physics2D.OverlapAreaAll(f1p1, f1p2);
            if (findPlayer != null)
            {
                for (int i = 0; i < findPlayer.Length; i++)
                {
                   if(findPlayer[i].gameObject.tag == "Player")
                    {
                        DaliFam1.SetActive(true);
                        daliFam1Activated = true;
                    }
                }
            }
        }


        if (!daliFam2Activated)
        {
            Vector2 f1p1 = new Vector2(193.9f,0.8f);
            Vector2 f1p2 = new Vector2(197.9f,15.7f);
            Collider2D[] findPlayer = Physics2D.OverlapAreaAll(f1p1, f1p2);
          
            if (findPlayer != null)
            {
                for (int i = 0; i < findPlayer.Length; i++)
                {
                    if (findPlayer[i].gameObject.tag == "Player")
                    {
                        DaliFam2.SetActive(true);
                        daliFam2Activated = true;
                        StartCoroutine(SecondFamilySet());
                    }
                }
            }
        }
    }

    IEnumerator SecondFamilySet()
    {
        daliFam2Activated = true;
        yield return new WaitForSeconds(0.5f);
        DaliFam3.SetActive(true);
    }
}
