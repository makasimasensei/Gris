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
    float moveFactor;  //�����ƶ�������
    float fastMoveFactor;  //���ҿ����ƶ�����
    bool leftShift = false;  //��shift�Ƿ���
    bool isGrounded = true;  //�Ƿ��ڵ�����
    bool isGroundedBefore = true;  //��ǰ�Ƿ��ڵ���
    readonly float jumpFactor = 700f;  //��Ծ�߶�����
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

        //�����ƶ�
        if (leftShift)
        {
            FastMove();
            animator.SetBool("shift", leftShift);
        }
        //��ͨ�ƶ�
        else
        {
            Move();
            animator.SetBool("shift", leftShift);
        }
    }

    //��ͨ�ƶ�
    private void Move()
    {
        //�����Ƿ��ڵ���״̬
        animator.SetBool("isGrounded", isGrounded);
        //���ô��󲥷��ܲ�����
        animator.SetFloat("RunX", 0);

        //����Gris����ĳ���
        if (moveFactor > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveFactor == 0)
        {
        }
        else { spriteRenderer.flipX = false; }

        //�ж��ڵ���&&���ƶ�
        if (isGrounded == true && Mathf.Abs(moveFactor) > 0)
        {
            if (timer > 1f)
            {
                timer = 0;  //��ʱ������
                AudioSource.PlayClipAtPoint(moveClip, transform.position);  //�ƶ���Ƶ
            }
            else { timer += Time.fixedDeltaTime; }
        }
        //���ü�ʱ��
        else
        {
            timer = 0;
        }

        //�ƶ�����
        Vector2 moveDirection = Vector2.right * moveFactor;
        //�ƶ��ٶ�
        Vector2 moveVelocity = moveDirection * moveSpeed;
        //��Ծ�ٶ�
        Vector2 jumpVelocity = new Vector2(0, rigidbody2d_Gris.velocity.y);

        //�򶯻�����������Gris��y���ٶȣ��Էֱ𲥷����������䶯��
        animator.SetFloat("MoveY", rigidbody2d_Gris.velocity.y);
        //�ϳ��ٶ�
        rigidbody2d_Gris.velocity = moveVelocity + jumpVelocity;
        //�򶯻�����������Gris��x���ٶȣ��Կ��Ʋ�����·����
        animator.SetFloat("MoveX", Mathf.Abs(rigidbody2d_Gris.velocity.x));
    }

    private void FastMove()
    {
        //�����Ƿ��ڵ���״̬
        animator.SetBool("isGrounded", isGrounded);
        //���ô��󲥷���·����
        animator.SetFloat("MoveX", 0);

        //����Gris����ĳ���
        if (moveFactor > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveFactor == 0)
        {
        }
        else { spriteRenderer.flipX = false; }

        //�ж��ڵ���&&���ƶ�
        if (Mathf.Abs(moveFactor) > 0 && isGrounded == true)
        {
            if (timer > 0.5f)
            {
                timer = 0;
                AudioSource.PlayClipAtPoint(moveClip, transform.position);
            }
            else { timer += Time.fixedDeltaTime; }
        }
        //���ü�ʱ��
        else
        {
            timer = 0;
        }

        //�ƶ�����
        Vector2 jumpVelocity = new Vector2(0, rigidbody2d_Gris.velocity.y);
        //�ƶ��ٶ�
        Vector2 fastMoveDirection = Vector2.right * fastMoveFactor;
        //��Ծ�ٶ�
        Vector2 fastMoveVelocity = fastMoveDirection * moveSpeed;

        //�򶯻�����������Gris��y���ٶȣ��Էֱ𲥷����������䶯��
        animator.SetFloat("MoveY", rigidbody2d_Gris.velocity.y);
        //�ϳ��ٶ�
        rigidbody2d_Gris.velocity = fastMoveVelocity + jumpVelocity;
        //�򶯻�����������Gris��x���ٶȣ��Կ��Ʋ�����·����
        animator.SetFloat("RunX", Mathf.Abs(rigidbody2d_Gris.velocity.x));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�ж��Ƿ���ײ��Ground����Plane
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Plane"))
        {
            //�ж���ײ�ĵ��Ƿ���Gris�·�&&�Ƿ���½
            if (collision.collider.ClosestPoint(transform.position).y < transform.position.y && isGrounded == false)
            {
                isGrounded = true;
                AudioSource.PlayClipAtPoint(landingClip, transform.position);
            }
            isGroundedBefore = isGrounded;
        }
    }
}
