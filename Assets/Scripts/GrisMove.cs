using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrisMove : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidbody2d_Gris;
    AudioClip jumpClip, moveClip, landingClip;
    //Song song;
    float moveFactor;  //左右移动的因子
    float fastMoveFactor;  //左右快速移动因子
    bool leftShift = false;  //左shift是否按下
    bool isGrounded = true;  //是否在地面上
    bool isGroundedBefore = true;  //先前是否在地上
    readonly float jumpFactor = 700f;  //跳跃高度因子
    readonly float moveSpeed = 2.5f;
    float timer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2d_Gris = GetComponent<Rigidbody2D>();
        jumpClip = Resources.Load<AudioClip>("Gris/Audioclips/Jump");
        moveClip = Resources.Load<AudioClip>("Gris/Audioclips/Move");
        landingClip = Resources.Load<AudioClip>("Gris/Audioclips/Land");
    }

    void Update()
    {
        moveFactor = Input.GetAxisRaw("Horizontal");
        fastMoveFactor = Input.GetAxisRaw("Horizontal") * 2f;
        leftShift = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            rigidbody2d_Gris.AddForce(Vector2.up * jumpFactor);
            isGrounded = false;
            isGroundedBefore = isGrounded;
            AudioSource.PlayClipAtPoint(jumpClip, transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        //if (rigidbody2d_Gris.velocity.y >= -7f && rigidbody2d_Gris.velocity.y <= -5f)
        //{
        //    isGrounded = false;
        //    isGroundedBefore = isGrounded;
        //}

        //快速移动
        if (leftShift)
        {
            FastMove();
            animator.SetBool("shift", leftShift);
        }
        //普通移动
        else
        {
            Move();
            animator.SetBool("shift", leftShift);
        }
    }

    //普通移动
    private void Move()
    {
        //传递是否在地面状态
        animator.SetBool("isGrounded", isGrounded);
        //不让错误播放跑步动画
        animator.SetFloat("RunX", 0);

        //控制Gris人物的朝向
        if (moveFactor > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveFactor == 0)
        {
        }
        else { spriteRenderer.flipX = false; }

        //判断在地面&&在移动
        if (isGrounded == true && Mathf.Abs(moveFactor) > 0)
        {
            if (timer > 1f)
            {
                timer = 0;  //计时器清零
                AudioSource.PlayClipAtPoint(moveClip, transform.position);  //移动音频
            }
            else { timer += Time.fixedDeltaTime; }
        }
        //重置计时器
        else
        {
            timer = 0;
        }

        //移动方向
        Vector2 moveDirection = Vector2.right * moveFactor;
        //移动速度
        Vector2 moveVelocity = moveDirection * moveSpeed;
        //跳跃速度
        Vector2 jumpVelocity = new Vector2(0, rigidbody2d_Gris.velocity.y);

        //向动画控制器传递Gris的y轴速度，以分别播放起跳和下落动画
        animator.SetFloat("MoveY", rigidbody2d_Gris.velocity.y);
        //合成速度
        rigidbody2d_Gris.velocity = moveVelocity + jumpVelocity;
        //向动画控制器传递Gris的x轴速度，以控制播放走路动画
        animator.SetFloat("MoveX", Mathf.Abs(rigidbody2d_Gris.velocity.x));
    }

    private void FastMove()
    {
        //传递是否在地面状态
        animator.SetBool("isGrounded", isGrounded);
        //不让错误播放走路动画
        animator.SetFloat("MoveX", 0);

        //控制Gris人物的朝向
        if (moveFactor > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveFactor == 0)
        {
        }
        else { spriteRenderer.flipX = false; }

        //判断在地面&&在移动
        if (Mathf.Abs(moveFactor) > 0 && isGrounded == true)
        {
            if (timer > 0.5f)
            {
                timer = 0;
                AudioSource.PlayClipAtPoint(moveClip, transform.position);
            }
            else { timer += Time.fixedDeltaTime; }
        }
        //重置计时器
        else
        {
            timer = 0;
        }

        //移动方向
        Vector2 jumpVelocity = new Vector2(0, rigidbody2d_Gris.velocity.y);
        //移动速度
        Vector2 fastMoveDirection = Vector2.right * fastMoveFactor;
        //跳跃速度
        Vector2 fastMoveVelocity = fastMoveDirection * moveSpeed;

        //向动画控制器传递Gris的y轴速度，以分别播放起跳和下落动画
        animator.SetFloat("MoveY", rigidbody2d_Gris.velocity.y);
        //合成速度
        rigidbody2d_Gris.velocity = fastMoveVelocity + jumpVelocity;
        //向动画控制器传递Gris的x轴速度，以控制播放走路动画
        animator.SetFloat("RunX", Mathf.Abs(rigidbody2d_Gris.velocity.x));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //判断是否碰撞到Ground或者Plane
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Plane"))
        {
            //判断碰撞的点是否在Gris下方&&是否着陆
            if (collision.collider.ClosestPoint(transform.position).y < transform.position.y && isGrounded == false)
            {
                isGrounded = true;
                AudioSource.PlayClipAtPoint(landingClip, transform.position);
            }
            isGroundedBefore = isGrounded;
        }
    }
}
