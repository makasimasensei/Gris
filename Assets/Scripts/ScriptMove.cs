using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ScriptMove : MonoBehaviour
{
    Transform targetPos;  //脚本移动目标位置
    Stone stone;  //Stone脚本，Stone脚本中赋值
    Animator animator;  //动画控制器
    readonly float move_percent = 0.005f;  //脚本移动到目标位置速度
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
