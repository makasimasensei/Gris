using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TearPet : MonoBehaviour
{
    private Transform targetTrans;
    private float speed;
    private float timer;
    private readonly float timeval = 0.1f;
    private bool startShake;
    // Start is called before the first frame update
    void Start()
    {
        Transform grisTrans = GameObject.Find("Gris").GetComponent<Transform>();
        for (int i = 0; i < grisTrans.childCount; i++)
        {
            if (grisTrans.GetChild(i).childCount <= 0)
            {
                targetTrans = grisTrans.GetChild(i);
                GameObject go = new GameObject();
                go.transform.SetParent(targetTrans);
                break;
            }
        }
        speed = 1.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (startShake)
        {
            PetShake();
        }
        else
        {
            PetMove();
        }
    }

    private void PetMove()
    {
        if (Vector2.Distance(transform.position, targetTrans.position) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, targetTrans.position, speed * Time.fixedDeltaTime);
        }
        else
        {
            startShake = true;
        }
    }

    private void PetShake()
    {
        if (Vector2.Distance(transform.position, targetTrans.position) > 0.03f)
        {
            startShake = false;
        }
        else
        {
            if (timer >= timeval)
            {
                timer = 0;
                transform.position = targetTrans.position + Mathf.PingPong(Time.time, 0.02f) * Vector3.one;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }
}
