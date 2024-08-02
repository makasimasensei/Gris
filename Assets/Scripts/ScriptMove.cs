using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ScriptMove : MonoBehaviour
{
    Transform targetPos;  //�ű��ƶ�Ŀ��λ��
    Stone stone;  //Stone�ű���Stone�ű��и�ֵ
    Animator animator;  //����������
    readonly float move_percent = 0.005f;  //�ű��ƶ���Ŀ��λ���ٶ�
    public bool startMove;

    void Start()
    {
        targetPos = GameObject.Find("TargetPos").transform;
        stone = GameObject.Find("Girl1").GetComponent<Stone>();
        animator = GetComponent<Animator>();
        animator.Play("GrisIdle");
    }

    // Update is called once per frame
    void Update()
    {
        if (startMove)
        {
            animator.Play("GrisWalk 0");
            transform.position = Vector2.MoveTowards(transform.position, targetPos.position, move_percent);
            if (Vector2.Distance(transform.position, targetPos.position) < 0.1f)
            {
                stone.startCreatingTears = true;
                animator.SetBool("animtorOver", true);
                Destroy(this);
            }
        }
    }
}
