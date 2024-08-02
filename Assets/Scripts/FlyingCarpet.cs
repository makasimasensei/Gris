using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCarpet : MonoBehaviour
{
    private bool moveDir = true;
    public float moveSpeed = 0.7f;
    public Vector2 startPos = new Vector2(295, 22);
    public Vector2 endPos = new Vector2(295, -1);
    private Transform carpetTrans;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        carpetTrans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDir)
        {
            sr.flipX = false;
            if (Vector2.Distance(carpetTrans.position, endPos) > 0.05f)
            {
                carpetTrans.position = Vector2.Lerp(carpetTrans.position, endPos, moveSpeed * Time.deltaTime);
            }
            else
            {
                moveDir = false;
            }
        }
        if (!moveDir)
        {
            sr.flipX = true;
            if (Vector2.Distance(carpetTrans.position, startPos) > 0.05f)
            {
                carpetTrans.position = Vector2.Lerp(carpetTrans.position, startPos, moveSpeed * Time.deltaTime);
            }
            else
            {
                moveDir = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Gris") 
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Gris")
        {
            collision.transform.SetParent(null);
        }
    }
}
