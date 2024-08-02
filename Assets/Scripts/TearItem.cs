using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearItem : MonoBehaviour
{
    private Transform insideCircleTrans;
    private Transform outsideCircleTrans;
    private float localSpeed = 0.1f;
    private float rotateSpeed = 120;
    private SpriteRenderer sr;
    private bool getTear = false;
    private SpriteRenderer tearSR;
    private Sprite sprite;
    private GameObject tearPet;
    private AudioClip tearClip;
    // Start is called before the first frame update
    void Start()
    {
        insideCircleTrans = transform.Find("InsideCircle");
        outsideCircleTrans = transform.Find("OutSideCircle");
        sr = outsideCircleTrans.GetComponent<SpriteRenderer>();
        tearSR = GetComponent<SpriteRenderer>();
        sprite = Resources.Load<Sprite>("Gris/Sprites/Item/" + gameObject.name);
        tearPet = Resources.Load<GameObject>("Prefabs/TearPet");
        tearClip = Resources.Load<AudioClip>("Gris/Audioclips/Tear");
    }

    // Update is called once per frame
    void Update()
    {
        if (insideCircleTrans != null)
        {
            insideCircleTrans.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);
        }

        if (outsideCircleTrans != null)
        {
            HandleOutsideCircle();
        }

        if (getTear)
        {
            tearSR.color -= new Color(0, 0, 0, localSpeed * 6 * Time.deltaTime);
            if (tearSR.color.a <= 0)
            {
                Instantiate(tearPet, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Gris" && getTear == false)
        {
            Destroy(insideCircleTrans.gameObject);
            getTear = true;
            tearSR.sprite = sprite;
            AudioSource.PlayClipAtPoint(tearClip, transform.position);
        }
    }
    private void HandleOutsideCircle()
    {
        outsideCircleTrans.Rotate(rotateSpeed / 8 * Time.deltaTime * Vector3.forward);
        outsideCircleTrans.localScale += localSpeed * Time.deltaTime * Vector3.one;
        sr.color -= new Color(0, 0, 0, localSpeed * 2 * Time.deltaTime);
        if (outsideCircleTrans.localScale.x > 0.5f)
        {
            outsideCircleTrans.localScale = Vector3.zero;
            if (getTear) { Destroy(outsideCircleTrans.gameObject); }
        }
        if (sr.color.a <= 0)
        {
            sr.color = new Color(0, 0, 0, 1);
        }
    }
}
