using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;

public class RenderColor : MonoBehaviour
{
    public RenderData[] renderDatas;
    public bool startChangeAlphaCutOff, startChangeColorAlpha, startChangeScaleAlpha;

    void Start()
    {
        renderDatas = new RenderData[3];
        for (int i = 0; i < renderDatas.Length; i++)
        {
            renderDatas[i] = new RenderData();
        }
        GetData(transform.GetChild(0), 0);
        GetData(transform.GetChild(1), 1);
        GetData(transform.GetChild(2), 2);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAlphaBGCuteOff();
        ChangeColorAlphaValue();
        ChangeScale();
    }

    public void StartChangeScale()
    {
        startChangeScaleAlpha = true;
    }
        private void ChangeScale()
    {
        if (startChangeScaleAlpha)
        {
            for (int i = 0; i < renderDatas[2].spriteRenderer.Length; i++)
            {
                ChangeAlphaValue(renderDatas[2].spriteRenderer[i], renderDatas[2].aValues[i]);
            }
            for (int i = 0; i < renderDatas[2].trans.Length; i++)
            {
                ChangeScaleValue(renderDatas[2].trans[i], renderDatas[2].scales[i]);
            }
        }
    }
    private void ChangeScaleValue(Transform transform, Vector3 targetValue)
    {
        if (transform.localScale.x <= targetValue.x)
        {
            transform.localScale += Vector3.one * 0.25f * Time.deltaTime;
        }
    }
    public void StartChangeColorAlpha()
    {
        startChangeColorAlpha = true;
        for (int i = 0; i < renderDatas[1].spriteRenderer.Length; i++)
        {
            renderDatas[1].spriteRenderer[i].color += new UnityEngine.Color(0, 0, 0, 1);
        }
        for (int i = 0; i < renderDatas[1].trans.Length; i++)
        {
            renderDatas[1].trans[i].localScale = renderDatas[1].scales[i];
        }
    }
    private void ChangeColorAlphaValue()
    {
        if (startChangeColorAlpha)
        {
            for (int i = 0; i < renderDatas[1].spriteMask.Length; i++)
            {
                if (renderDatas[1].spriteMask[i].alphaCutoff >= renderDatas[1].alpha[i])
                {
                    renderDatas[1].spriteMask[i].alphaCutoff -= 0.06f * Time.deltaTime;
                }
            }
            for (int i = 0; i < renderDatas[1].spriteRenderer.Length; i++)
            {
                ChangeAlphaValue(renderDatas[1].spriteRenderer[i], renderDatas[1].aValues[i]);
            }
        }
    }
    private void ChangeAlphaValue(SpriteRenderer sr, float targetValue)
    {
        if (sr.color.a <= targetValue)
        {
            sr.color += new UnityEngine.Color(0, 0, 0, 0.15f) * Time.deltaTime;
        }
    }
    public void StartChangeAlphaCutOff()
    {
        startChangeAlphaCutOff = true;
        for (int i = 0; i < renderDatas[0].spriteRenderer.Length; i++)
        {
            renderDatas[0].spriteRenderer[i].color += new UnityEngine.Color(0, 0, 0, 1);
        }
        for (int i = 0; i < renderDatas[0].trans.Length; i++)
        {
            renderDatas[0].trans[i].localScale = renderDatas[0].scales[i];
        }
    }
    private void ChangeAlphaBGCuteOff()
    {
        if (startChangeAlphaCutOff)
        {
            for (int i = 0; i < renderDatas[0].spriteMask.Length; i++)
            {
                ChangeAlphaCutOffValue(renderDatas[0].spriteMask[i]);
            }
        }
    }
    private void ChangeAlphaCutOffValue(SpriteMask sm)
    {
        if (sm.alphaCutoff >= 0.2f)
        {
            sm.alphaCutoff -= 0.08f * Time.deltaTime;
        }
    }

    public void GetData(Transform targetTrans, int index)
    {
        renderDatas[index].trans = targetTrans.GetComponentsInChildren<Transform>();
        renderDatas[index].spriteRenderer = targetTrans.GetComponentsInChildren<SpriteRenderer>();
        renderDatas[index].scales = new Vector3[renderDatas[index].trans.Length];
        renderDatas[index].aValues = new float[renderDatas[index].spriteRenderer.Length];
        for (int i = 0; i < renderDatas[index].spriteRenderer.Length; i++)
        {
            renderDatas[index].aValues[i] = renderDatas[index].spriteRenderer[i].color.a;
            renderDatas[index].spriteRenderer[i].color = new UnityEngine.Color(renderDatas[index].spriteRenderer[i].color.r,
    renderDatas[index].spriteRenderer[i].color.g, renderDatas[index].spriteRenderer[i].color.b, 0);
        }

        for (int i = 0; i < renderDatas[index].trans.Length; i++)
        {
            renderDatas[index].scales[i] = renderDatas[index].trans[i].localScale;
            renderDatas[index].trans[i].localScale = Vector3.zero;
        }
        renderDatas[index].spriteMask = targetTrans.GetComponentsInChildren<SpriteMask>();
        renderDatas[index].alpha = new float[renderDatas[index].spriteMask.Length];
        for (int i = 0; i < renderDatas[index].spriteMask.Length; i++)
        {
            renderDatas[index].alpha[i] = renderDatas[index].spriteMask[i].alphaCutoff;
            renderDatas[index].spriteMask[i].alphaCutoff = 1;
        }
    }
}

[System.Serializable]
public class RenderData
{
    public SpriteMask[] spriteMask;
    public SpriteRenderer[] spriteRenderer;
    public Vector3[] scales;
    public float[] aValues;
    public Transform[] trans;
    public float[] alpha;
}
