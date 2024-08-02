using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("This class is obsolete.")]
public class Tear5 : MonoBehaviour
{
    GameObject tear5;  //第五个眼泪对象
    Transform point5Trans;  //第五个眼泪移动路径点存放组件
    Transform[] point5Array;  //第五个眼泪的移动路径点数组
    GameObject mainCamera;  //主镜头
    Stone stone; //Stone脚本
    public int tearsNum;
    public int stopNum;
    private bool cameraMoveDone = false;
    Tear tear;

    // Start is called before the first frame update
    void Start()
    {
        tear5 = Resources.Load<GameObject>("Prefabs/Tear");
        point5Trans = transform.Find("Point5");
        point5Array = new Transform[point5Trans.childCount];
        for (int i = 0; i < point5Array.Length; i++)
        {
            point5Array[i] = point5Trans.GetChild(i);
        }
        mainCamera = GameObject.Find("Main Camera");
        stone = transform.GetComponent<Stone>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tearsNum < 4)
        {
            tearsNum = stone.tearNum;
        }
        if (stopNum == 4)
        {
            CameraMove();
            if (cameraMoveDone)
            {
                StartCoroutine(WaitTime());
            }
        }
    }
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1);
        if (tearsNum == 4)
        {
            StartCreatingTear5();
        }
    }

    private void StartCreatingTear5()
    {
        tearsNum++;
        GameObject go = Instantiate(tear5, point5Array[0].position, Quaternion.identity);
        Tear tear = go.GetComponent<Tear>();
        tear.point5Array = point5Array;
        tear.tearNum = tearsNum;
    }

    private void CameraMove()
    {
        if (Vector3.Distance(mainCamera.transform.position, new Vector3(-4, 6, -6)) > 0.1f)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new Vector3(-4, 6, -6), 4f * Time.deltaTime);
        }
        if (Vector3.Distance(mainCamera.transform.position, new Vector3(-4, 6, -6)) < 0.1f)
        {
            cameraMoveDone = true;
        }
    }
}
