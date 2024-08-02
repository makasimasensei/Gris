using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song : MonoBehaviour
{
    private bool isSinging;
    private Transform outsideCircle;
    // Start is called before the first frame update
    void Start()
    {
        outsideCircle = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(120 * Time.deltaTime * Vector3.forward);
        if (isSinging)
        {
            if (transform.localScale.x <= 5)
            {
                transform.localScale += 5 * Time.deltaTime * Vector3.one;
            }
            else
            {
                outsideCircle.gameObject.SetActive(true);
                if (outsideCircle.localScale.x <= 1)
                {
                    outsideCircle.localScale += 5 *0.8f* Time.deltaTime * Vector3.one;
                }
            }
        }
        else
        {
            if (outsideCircle.localScale.x >= 0.8f)
            {
                outsideCircle.localScale -= 3 * 5 * Time.deltaTime * Vector3.one;
            }
            else
            {
                outsideCircle.gameObject.SetActive(false);
                if (transform.localScale.x > 0)
                {
                    transform.localScale -= 3 * 5 * Time.deltaTime * Vector3.one;
                    if(transform.localScale.x < 0)
                    {
                        transform.localScale = Vector3.zero;
                    }
                }
            }
        }
    }

    public void SetSingState(bool state)
    {
        isSinging = state;
    }
}
