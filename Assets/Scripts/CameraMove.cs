using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Camera mainCamera;  //����ͷ
    ScriptMove scriptMove;  //ScriptMove�ű�
    AudioSource audioSource;  //��Ƶ�ļ�
    bool audioIsPlaying, cameraMagnify;

    void Start()
    {
        mainCamera = Camera.main;
        scriptMove = GetComponent<ScriptMove>();
        audioSource = transform.GetComponent<AudioSource>();
        mainCamera.transform.parent = transform;  //��������ͷ����ΪGris
    }

    private void Update()
    {
        if (cameraMagnify == false)
        {
            //��ͷ��һ���ƶ�
            mainCamera.transform.localPosition = Vector3.MoveTowards(
        mainCamera.transform.localPosition, new Vector3(-6f, 7.5f, -10f), 4f * Time.deltaTime);
            if (Vector3.Distance(mainCamera.transform.localPosition, new Vector3(-6f, 7.5f, -10f)) < 0.01f)
            {
                cameraMagnify = true;
            }
        }
        else
        {
            //��ͷ�Ŵ�
            mainCamera.orthographicSize =
                Mathf.MoveTowards(mainCamera.orthographicSize, 5f, 0.5f * Time.deltaTime);
            if (Mathf.Approximately(mainCamera.orthographicSize, 5f))
            {
                //��ͷ�ڶ����ƶ�
                mainCamera.transform.localPosition =
                Vector3.MoveTowards(mainCamera.transform.localPosition, new Vector3(-6f, 6f, -10f), 1.3f * Time.deltaTime);
            }
            if (Vector3.Distance(mainCamera.transform.localPosition, new Vector3(-6f, 6f, -10f)) < 0.01f)
            {
                //���ýű���ʼ�ƶ�
                scriptMove.startMove = true;
            }
        }

        //BG2���ִ�С���󲥷�
        if (audioIsPlaying == false)
        {
            if (Mathf.Approximately(audioSource.volume, 0f))
            {
                audioSource.clip = Resources.Load<AudioClip>(@"Gris/Audioclips/BG2");
                audioSource.Play();
                audioSource.loop = true;
                audioIsPlaying = true;
            }
        }
        else
        {
            if (!Mathf.Approximately(audioSource.volume, 1f))
            {
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, 1f, 0.25f * Time.deltaTime);
            }
        }

        //�����ͷ��λ&&��ͷ��СΪ5&&����Ϊ1����ɾ���ýű�
        if (Vector3.Distance(mainCamera.transform.localPosition, new Vector3(-6f, 6f, -10f)) < 0.01f
            && mainCamera.orthographicSize == 5
            && Mathf.Approximately(audioSource.volume, 1f))
        {
            Destroy(this);
        }
    }
}

