using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseController : MonoBehaviour
{

    //stuff to make the lillies bounce
    public Vector3 startPt; //starting size of the lillypad, you can set in the inspector
    public Vector3 EndPt; //ending size of the lillypad, same as above
    private float animationTimePosition;
    public float duration = 0.5f; //both speed and duration effects the bounce speed of the lillypads, idk i'm tired so just tweak them to get a feel
    public float speed = 2f;

    //particle stuff


    IEnumerator Start() //changing from void to numerator allows the coroutine to loop
    {
        while (true)
        {
            //grow lillypad
            yield return BounceLilly(startPt, EndPt, duration);
            //shrink lillypad
            yield return BounceLilly(EndPt, startPt, duration);
        }
    }

    // Update is called once per frame
    void Update()
    {


    }


    private IEnumerator BounceLilly(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f; //zero is the start point
        float rate = (1.0f / time) * speed; //one is the end point, and over time rate is being multiplied by time and by the speed we set in the inspector

        while (i < 1.0f) //goes until it reaches one??
        {
            i += Time.deltaTime * rate; //just your typical lerping stuff
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null; //needs to just return something or the numerator will be angry >:~(
        }
    }
}

