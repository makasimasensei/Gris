using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tear : MonoBehaviour
{
    Camera mainCamera;  //����ͷ
    GameObject[] currentTears;
    Vector3 cameraOriginalPos;

    public Stone stone;  //Stone�ű���Stone�ű��и�ֵ
    public Transform[] pointsArray;  //Stone�ű��и�ֵ
    public int tearNum;  //Stone�ű��и�ֵ

    int index = 1;  //�ƶ�λ������
    public bool creatTear5 = true;  //����Tear5
    public Transform[] point5Array;  //Tear5���ƶ�λ���������飬Stone�и�ֵ
    bool tear5StartMove, readyToEmit;  //Tear5��ʼ�ƶ�
    public bool tearEmit;  //tear�����ܷ���
    GameObject[] tearsAll;  //���е�tears

    private void Start()
    {
        mainCamera = Camera.main;
        currentTears= GameObject.FindGameObjectsWithTag("Tear");
        //��¼������ʼλ�ã��Ա���滹ԭ
        cameraOriginalPos = mainCamera.transform.position;
    }

    void Update()
    {
        if (tearEmit == false)  //�жϷ����ź�
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
        if (tearNum <= 4)  //�ж�tear�Ƿ�Ϊǰ�ĸ�
        {
            if (Vector2.Distance(transform.position, pointsArray[index].position) < 0.01f)  //�ж�tear�Ƿ��ƶ���λ������
            {
                if (index < pointsArray.Length - tearNum)  //�ж�û���ƶ���������tearNum��λ��
                {
                    index++;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, pointsArray[index].position, 3f * Time.deltaTime);
            if (creatTear5 == true  //�Ƿ񴴽�Tear5
                && currentTears.Length == 4  //currentTears�ĳ�����Start�д���
                && index == pointsArray.Length - tearNum
                && Vector2.Distance(transform.position, pointsArray[index].position) < 0.01f)
            {
                stone.shouldStartCreatingTear5 = true;
                creatTear5 = false;
            }
        }

        if (tearNum == 5)  //�ж�tear�Ƿ�Ϊ�����
        {
            if(!readyToEmit)CameraFollowTear5();
            else TearEmit(cameraOriginalPos);
        }
    }

    private void CameraFollowTear5()
    {
        //��ȡmainCamera��ȫ������
        Vector3 cameraPos = mainCamera.transform.position;
        //��Tear5��x��yֵ����mainCamera
        cameraPos.x = transform.position.x;
        cameraPos.y = transform.position.y;

        if (!Mathf.Approximately(mainCamera.orthographicSize, 3f))
        {
            //�Ŵ�ͷ
            mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, 3f, 0.8f * Time.deltaTime);
        }

        if (mainCamera.orthographicSize < 4f && cameraPos.y > 2f)
        {
            //׷��Tear5λ��
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, cameraPos, 3f * Time.deltaTime);
        }

        //Tear5�����ƶ�
        if (Mathf.Approximately(mainCamera.orthographicSize, 3f)
            && Vector3.Distance(mainCamera.transform.position, cameraPos) < 0.01f)
        {
            tear5StartMove = true;
        }

        if (tear5StartMove && Vector2.Distance(transform.position, point5Array[index].position) > 0.01f)
        {
            //Tear5��ʼ�ƶ�
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