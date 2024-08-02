using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Camera mainCamera;  //主镜头
    ScriptMove scriptMove;  //ScriptMove脚本
    AudioSource audioSource;  //音频文件
    bool audioIsPlaying, cameraMagnify;

    void Start()
    {
        mainCamera = Camera.main;
        scriptMove = GetComponent<ScriptMove>();
        audioSource = transform.GetComponent<AudioSource>();
        mainCamera.transform.parent = transform;  //设置主镜头父类为Gris
    }

    private void Update()
    {
        if (cameraMagnify == false)
        {
            //镜头第一次移动
            mainCamera.transform.localPosition = Vector3.MoveTowards(
        mainCamera.transform.localPosition, new Vector3(-6f, 7.5f, -10f), 4f * Time.deltaTime);
            if (Vector3.Distance(mainCamera.transform.localPosition, new Vector3(-6f, 7.5f, -10f)) < 0.01f)
            {
                cameraMagnify = true;
            }
        }
        else
        {
            //镜头放大
            mainCamera.orthographicSize =
                Mathf.MoveTowards(mainCamera.orthographicSize, 5f, 0.5f * Time.deltaTime);
            if (Mathf.Approximately(mainCamera.orthographicSize, 5f))
            {
                //镜头第二次移动
                mainCamera.transform.localPosition =
                Vector3.MoveTowards(mainCamera.transform.localPosition, new Vector3(-6f, 6f, -10f), 1.3f * Time.deltaTime);
            }
            if (Vector3.Distance(mainCamera.transform.localPosition, new Vector3(-6f, 6f, -10f)) < 0.01f)
            {
                //设置脚本开始移动
                scriptMove.startMove = true;
            }
        }

        //BG2音乐从小到大播放
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

        //如果镜头到位&&镜头大小为5&&音量为1，则删除该脚本
        if (Vector3.Distance(mainCamera.transform.localPosition, new Vector3(-6f, 6f, -10f)) < 0.01f
            && mainCamera.orthographicSize == 5
            && Mathf.Approximately(audioSource.volume, 1f))
        {
            Destroy(this);
        }
    }
}

