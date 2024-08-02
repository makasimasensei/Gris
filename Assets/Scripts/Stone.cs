using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    GameObject mainCamera;
    GameObject gris;  //����
    GrisMove grisMove;  //GrisMove�ű�
    CameraMove cameraMove;  //��ͷ�Ŵ�ű�
    Rigidbody2D rb;  //���Ǹ���
    GameObject tears;  //����Ԥ����
    Transform pointsTrans;  //�����ƶ�·���������
    Transform[] pointsArray;  //�����ƶ�·��������
    Transform point5Trans;  //����������ƶ�·���������
    Transform[] point5Array;  //�����������ƶ�·��������
    public bool startCreatingTears, shouldStartCreatingTear5;  //����Tears������shouldStartCreatingTear5
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
        //��ʼ����tear
        if (startCreatingTears)
        {
            InvokeRepeating(nameof(CreatingTears), 1, 3);  //�ӳ�0�����CreatingTears����, �ظ�ʱ��3��
            startCreatingTears = false;
        }

        //���������ĸ���ʱ��ֹͣ����
        if (tearNum == 4)
        {
            CancelInvoke();
        }

        //��ʼ���������tear
        if (shouldStartCreatingTear5 == true)
        {
            Invoke(nameof(CreatingTear5), 0);
            shouldStartCreatingTear5 = false;
        }

        //�������
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

