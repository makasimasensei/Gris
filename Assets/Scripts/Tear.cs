using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tear : MonoBehaviour
{
    Camera mainCamera;  //主镜头
    GameObject[] currentTears;
    Vector3 cameraOriginalPos;

    public Stone stone;  //Stone脚本，Stone脚本中赋值
    public Transform[] pointsArray;  //Stone脚本中赋值
    public int tearNum;  //Stone脚本中赋值

    int index = 1;  //移动位置索引
    public bool creatTear5 = true;  //创建Tear5
    public Transform[] point5Array;  //Tear5的移动位置坐标数组，Stone中赋值
    bool tear5StartMove, readyToEmit;  //Tear5开始移动
    public bool tearEmit;  //tear向四周发射
    GameObject[] tearsAll;  //所有的tears

    private void Start()
    {
        mainCamera = Camera.main;
        currentTears= GameObject.FindGameObjectsWithTag("Tear");
        //记录主相机最开始位置，以便后面还原
        cameraOriginalPos = mainCamera.transform.position;
    }

    void Update()
    {
        if (tearEmit == false)  //判断发射信号
        {
            TearTakePlace();
        }
        if (tearEmit == true)
        {
            StartCoroutine(WaitToEmit());
        }
    }

    private void TearTakePlace()
    {
        if (tearNum <= 4)  //判断tear是否为前四个
        {
            if (Vector2.Distance(transform.position, pointsArray[index].position) < 0.01f)  //判断tear是否移动到位置坐标
            {
                if (index < pointsArray.Length - tearNum)  //判断没有移动到倒数第tearNum个位置
                {
                    index++;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, pointsArray[index].position, 3f * Time.deltaTime);
            if (creatTear5 == true  //是否创建Tear5
                && currentTears.Length == 4  //currentTears的长度在Start中创建
                && index == pointsArray.Length - tearNum
                && Vector2.Distance(transform.position, pointsArray[index].position) < 0.01f)
            {
                stone.shouldStartCreatingTear5 = true;
                creatTear5 = false;
            }
        }

        if (tearNum == 5)  //判断tear是否为第五个
        {
            if(!readyToEmit)CameraFollowTear5();
            else TearEmit(cameraOriginalPos);
        }
    }

    private void CameraFollowTear5()
    {
        //获取mainCamera的全局坐标
        Vector3 cameraPos = mainCamera.transform.position;
        //将Tear5的x，y值赋给mainCamera
        cameraPos.x = transform.position.x;
        cameraPos.y = transform.position.y;

        if (!Mathf.Approximately(mainCamera.orthographicSize, 3f))
        {
            //放大镜头
            mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, 3f, 0.8f * Time.deltaTime);
        }

        if (mainCamera.orthographicSize < 4f && cameraPos.y > 2f)
        {
            //追随Tear5位置
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, cameraPos, 3f * Time.deltaTime);
        }

        //Tear5允许移动
        if (Mathf.Approximately(mainCamera.orthographicSize, 3f)
            && Vector3.Distance(mainCamera.transform.position, cameraPos) < 0.01f)
        {
            tear5StartMove = true;
        }

        if (tear5StartMove && Vector2.Distance(transform.position, point5Array[index].position) > 0.01f)
        {
            //Tear5开始移动
            transform.position = Vector2.MoveTowards(transform.position, point5Array[index].position, 3f * Time.deltaTime);
            if (Vector2.Distance(transform.position, point5Array[index].position) < 0.01f)
            {
                readyToEmit=true;
            }
        }
    }

    private void TearEmit(Vector3 cameraOriginalPos)
    {
        if (Vector2.Distance(transform.position, point5Array[index].position) < 0.01f)
        {
            mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, 5f, 0.8f * Time.deltaTime);
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, cameraOriginalPos, 3f * Time.deltaTime);
            if (Mathf.Approximately(mainCamera.orthographicSize, 5f)
            && Vector3.Distance(mainCamera.transform.position, cameraOriginalPos) < 0.01f)
            {
                for (int i = 0; i < 5; i++)
                {
                    currentTears[i].GetComponent<Tear>().tearEmit = true;
                    currentTears[i].transform.Rotate(new Vector3(0f, 0f, 20 * i));
                }
            }
        }
    }

    IEnumerator WaitToEmit()
    {
        yield return new WaitForSeconds(1);
        transform.Translate(5f * Time.deltaTime * transform.right);
        yield return new WaitForSeconds(3);
        stone.tearEmitDone = true;
        Destroy(gameObject);
    }
}