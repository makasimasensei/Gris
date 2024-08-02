using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : MonoBehaviour
{
    private SpriteRenderer[] srs;
    private float scaleSpeed = 0.5f;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        srs = GetComponentsInChildren<SpriteRenderer>();
        sr = GameObject.Find("GradientBG").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0;i<transform.childCount;i++)
        {
            transform.GetChild(i).localScale += scaleSpeed * Time.deltaTime * Vector3.one;
        }
        for (int i = 0; i < srs.Length; i++)
        {
            srs[i].color -= new Color(0, 0, 0, 0.25f) * Time.deltaTime;
        }
        sr.color -= new Color(0, 0, 0, 0.25f) * Time.deltaTime;
        if (srs[0].color.a < 0)
        {
            Destroy(gameObject);
        }
    }
}
