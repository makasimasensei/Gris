using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove2 : MonoBehaviour
{
    private bool startPosLerp;
    private Vector3 targetPos;
    private float targetSize;
    private Color targetColor;
    private bool startColorLerp;
    private bool startSizeLerp;

    // Update is called once per frame
    void LateUpdate()
    {
        if (startPosLerp)
        {
            if (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime);
            }
            else
            {
                startPosLerp = false;
            }
        }
        if(startSizeLerp)
        {
            if(Mathf.Abs(Camera.main.orthographicSize-targetSize)>0.01f)
            {
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, Time.deltaTime);
            }
            else
            {
                startSizeLerp = false;
            }
        }
        if (startColorLerp)
        {
            if (!Equals(Camera.main.backgroundColor, targetColor))
            {
                Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, targetColor, Time.deltaTime);
            }
            else
            {
                startColorLerp = false;
            }
        }
    }

    public void SetPos(Vector3 pos)
    {
        startPosLerp = true;
        targetPos = pos;
    }

    public void SetSize(float size)
    {
        startSizeLerp = true;
        targetSize = size;
    }

    public void SetColor(Color color)
    {
        startColorLerp = true;
        targetColor = color;
    }
}
