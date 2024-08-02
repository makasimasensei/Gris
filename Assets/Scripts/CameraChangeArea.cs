using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeArea : MonoBehaviour
{
    private Vector3 pos = new Vector3(26f, 15f, -10f);
    private float size = 12;
    private Color color;
    private CameraMove2 cm2;
    private bool cameraNormal = true;
    private Transform grisTrans;
    private CameraMove cm;
    // Start is called before the first frame update
    void Start()
    {
        cm2 = Camera.main.GetComponent<CameraMove2>();
        grisTrans = GameObject.Find("Gris").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        cm = GameObject.Find("Gris").GetComponent<CameraMove>();
        if (cameraNormal && cm == null)
        {
            if (grisTrans.position.y > 10)
            {
                CameraMountain();
            }
            else
            {
                cm2.SetPos(new Vector3(-6, 6, -10));
                cm2.SetSize(5);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Gris")
        {
            cameraNormal = false;
            if (pos != Vector3.zero)
            {
                cm2.SetPos(pos);
            }
            if (size != 0)
            {
                cm2.SetSize(size);
            }
            if (color != Color.clear)
            {
                cm2.SetColor(color);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Gris")
        {
            cameraNormal = true;
        }
    }

    private void CameraMountain()
    {
        cm2.SetPos(pos);
        cm2.SetSize(size);
    }
}
