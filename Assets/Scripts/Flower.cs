using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public bool bloom;
    private Animator animator;
    private GameObject tearItem;
    private AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        tearItem = Resources.Load<GameObject>("Prefabs/Tear");
        animator = GetComponent<Animator>();
        audioClip = Resources.Load<AudioClip>("Gris/Audioclips/Bloom");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Song")
        {
            if (!bloom)
            {
                animator.Play("Bloom");
                bloom = true;
                AudioSource.PlayClipAtPoint(audioClip, transform.position);
                Invoke(nameof(CreateTearItem), 1.5f);
            }
        }
    }
    private void CreateTearItem()
    {
        Instantiate(tearItem, transform.position + transform.up * 0.6f, Quaternion.identity);
        enabled = false;
    }
}
