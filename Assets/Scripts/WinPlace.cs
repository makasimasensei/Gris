using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPlace : MonoBehaviour
{
    Transform grisTrans;  //Gris的Transform组件
    AudioClip audioNormal, audioClipJudge, audioClipToNextLevel;  //音频
    AudioSource audioSource;  //音频播放组件
    CameraMove2 cameraMove2;  //CameraMove2脚本
    RenderColor renderColor;
    GrisMove grisMove;
    ToNextLevel toNextLevel;
    AsyncOperation asyncOperation;
    bool audioPlay, changeMusicToBG3, changeMusicToBG4, isCoroutineRunning;

    // Start is called before the first frame update
    void Start()
    {
        grisTrans = GameObject.Find("Gris").GetComponent<Transform>();
        audioNormal = Resources.Load<AudioClip>("Gris/Audioclips/BG2");
        audioClipJudge = Resources.Load<AudioClip>("Gris/Audioclips/BG3");
        audioClipToNextLevel = Resources.Load<AudioClip>("Gris/Audioclips/BG4");
        audioSource = GameObject.Find("Gris").GetComponent<AudioSource>();
        cameraMove2 = Camera.main.GetComponent<CameraMove2>();
        renderColor = GameObject.Find("RenderColor").GetComponent<RenderColor>();
        toNextLevel = grisTrans.GetComponent<ToNextLevel>();
        asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (changeMusicToBG3)
        {
            ChangeMusic(audioClipJudge);
            Invoke(nameof(PlayNormalMusic), 30);
        }
        if (changeMusicToBG4)
        {
            ChangeMusic(audioClipToNextLevel);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Gris")
        {
            if (JudgeTearNum())
            {
                if (audioSource.clip.name != audioClipToNextLevel.name)
                {
                    changeMusicToBG4 = true;
                    StartCoroutine(nameof(LoadNextLevel));
                }
                else
                {
                    changeMusicToBG4 = false;
                }
            }
            else
            {
                if (audioSource.clip.name != audioClipJudge.name)
                {
                    changeMusicToBG3 = true;
                }
                else
                {
                    changeMusicToBG3 = false;
                }
            }
        }
    }

    private bool JudgeTearNum()
    {
        for (int i = 0; i < grisTrans.childCount - 1; i++)
        {
            if (grisTrans.GetChild(i).childCount <= 0)
            {
                return false;
            }
        }
        return true;
    }

    private void ChangeMusic(AudioClip audio)
    {
        if (audioPlay == false)
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0.01f, 0.25f * Time.deltaTime);
            if (Mathf.Approximately(audioSource.volume, 0.01f))
            {
                audioSource.clip = audio;
                audioSource.Play();
                audioSource.loop = false;
                audioPlay = true;
            }
        }
        else
        {
            if (!Mathf.Approximately(audioSource.volume, 1f))
            {
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, 1f, 0.25f * Time.deltaTime);
            }
        }
    }

    private void PlayNormalMusic()
    {
        ChangeMusic(audioNormal);
        audioSource.loop = true;
        audioPlay = false;
    }

    IEnumerator LoadNextLevel()
    {
        if (isCoroutineRunning)
        {
            yield break;
        }
        isCoroutineRunning = true;
        grisMove = grisTrans.GetComponent<GrisMove>();
        grisMove.enabled = false;
        grisTrans.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        toNextLevel.enabled = true;
        toNextLevel.SetRigidBodyType(RigidbodyType2D.Kinematic);
        toNextLevel.PlayAnimation("GrisCry");
        yield return new WaitForSeconds(1.167f);
        toNextLevel.StartMove(new Vector3(328, 6, 0));
        toNextLevel.PlayAnimation("GrisFly");
        CameraChangeArea cca = GameObject.Find("fog").GetComponent<CameraChangeArea>();
        cca.enabled = false;
        yield return new WaitForSeconds(1);
        Instantiate(Resources.Load<GameObject>(@"Prefabs/Red"));
        cameraMove2.SetColor(new Color((float)252 / 255, (float)236 / 255, (float)228 / 255));
        cameraMove2.SetPos(new Vector3(-8.8f, 5f, -10f));
        cameraMove2.SetSize(10);

        yield return new WaitForSeconds(1);
        renderColor.StartChangeAlphaCutOff();
        yield return new WaitForSeconds(2);
        renderColor.StartChangeColorAlpha();
        yield return new WaitForSeconds(1);
        renderColor.StartChangeScale();
        Application.Quit();


        yield return new WaitForSeconds(7); 
        cca.enabled = true;
        toNextLevel.StartMove(new Vector3(328, 1.7f, 0));
        yield return new WaitForSeconds(2);
        toNextLevel.PlayAnimation("ToIdle");
        yield return new WaitForSeconds(2);
        cameraMove2.SetPos(new Vector3(14, 6, -10));
        cameraMove2.SetSize(5);
        yield return new WaitForSeconds(2);
        //asyncOperation.allowSceneActivation = true;
    }
}
