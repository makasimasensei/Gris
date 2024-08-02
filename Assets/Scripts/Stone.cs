using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    GameObject mainCamera;
    GameObject gris;  //主角
    GrisMove grisMove;  //GrisMove脚本
    CameraMove cameraMove;  //镜头放大脚本
    Rigidbody2D rb;  //主角刚体
    GameObject tears;  //眼泪预制体
    Transform pointsTrans;  //眼泪移动路径点存放组件
    Transform[] pointsArray;  //眼泪移动路径点数组
    Transform point5Trans;  //第五个眼泪移动路径点存放组件
    Transform[] point5Array;  //第五个眼泪的移动路径点数组
    public bool startCreatingTears, shouldStartCreatingTear5;  //创建Tears，创建shouldStartCreatingTear5
    public int tearNum;
    public bool tearEmitDone = false;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        gris = GameObject.Find("Gris");
        grisMove = Resources.Load<GrisMove>("Scripts/GrisMove");
        cameraMove = gris.GetComponent<CameraMove>();
        rb = gris.GetComponent<Rigidbody2D>();
        tears = Resources.Load<GameObject>("Prefabs/Tear");
        pointsTrans = transform.Find("Points");
        pointsArray = new Transform[pointsTrans.childCount];
        for (int i = 0; i < pointsArray.Length; i++)
        {
            pointsArray[i] = pointsTrans.GetChild(i);
        }
        point5Trans = transform.Find("Point5");
        point5Array = new Transform[point5Trans.childCount];
        for (int i = 0; i < point5Array.Length; i++)
        {
            point5Array[i] = point5Trans.GetChild(i);
        }
    }

    private void Update()
    {
        //开始创建tear
        if (startCreatingTears)
        {
            InvokeRepeating(nameof(CreatingTears), 1, 3);  //延迟0秒调用CreatingTears函数, 重复时间3秒
            startCreatingTears = false;
        }

        //创建到第四个的时候停止创建
        if (tearNum == 4)
        {
            CancelInvoke();
        }

        //开始创建第五个tear
        if (shouldStartCreatingTear5 == true)
        {
            Invoke(nameof(CreatingTear5), 0);
            shouldStartCreatingTear5 = false;
        }

        //发射完成
        if (tearEmitDone == true)
        {
            gris.AddComponent<GrisMove>();
            mainCamera.GetComponent<CameraMove2>().enabled = true;
            Destroy(this);
        }
    }

    private void CreatingTears()
    {
        tearNum++;
        GameObject go = Instantiate(tears, pointsArray[0].position, Quaternion.identity);
        Tear tear = go.GetComponent<Tear>();
        tear.pointsArray = pointsArray;
        tear.tearNum = tearNum;
        tear.stone = this;
    }

    private void CreatingTear5()
    {
        tearNum++;
        GameObject go = Instantiate(tears, point5Array[0].position, Quaternion.identity);
        Tear tear = go.GetComponent<Tear>();
        tear.point5Array = point5Array;
        tear.tearNum = tearNum;
        tear.stone = this;
    }
}

