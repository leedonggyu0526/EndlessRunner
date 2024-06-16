using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;

    public float speed = 5;
    [SerializeField] Rigidbody rb;

    float horizontalInput;
    [SerializeField] float horizontalMultiplier = 2;

    public float speedIncreasePerPoint = 0.1f;

    [SerializeField] float jumpForce = 400f;
    [SerializeField] LayerMask groundMask;

    private float lastJumpTime;
    public float jumpCooldown = 1f; // 점프 쿨다운 시간 (초 단위)

    [SerializeField] private GameObject prefab;
    Animator anim;
    private BackgroundMusicController backgroundMusicController;
    private AudioSource audioSource;
    public AudioClip deathClip;

    private void Start()
    {
        backgroundMusicController = FindObjectOfType<BackgroundMusicController>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!alive) return;

        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastJumpTime + jumpCooldown)
        {
            Jump();
        }

        if (transform.position.y < -5)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            RotatePlayerAndCamera();
        }
    }

    public void Die()
    {
        alive = false;
        // Restart the game
        Invoke("Restart", 2);
        backgroundMusicController.StopMusic();
        PlayDeathSound();
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Jump()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.1f, groundMask);
        anim.SetBool("isGrounded", !isGrounded); // Update the "isGrounded" boolean variable in the animation controller

        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time; // 마지막 점프 시간 업데이트
        }
    }

    void RotatePlayerAndCamera()
    {
        if(CheckObjectsBelow().name == "Road_Left_Corner")
        {   
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, -90f, 0f));
            transform.rotation = targetRotation;

            Quaternion cameraTargetRotation = Quaternion.Euler(Camera.main.transform.eulerAngles + new Vector3(0f, -90f, 0f));
            Camera.main.transform.rotation = cameraTargetRotation;
        }else if(CheckObjectsBelow().name == "Road_Right_Corner")
        {
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, 90f, 0f));
            transform.rotation = targetRotation;

            Quaternion cameraTargetRotation = Quaternion.Euler(Camera.main.transform.eulerAngles + new Vector3(0f, 90f, 0f));
            Camera.main.transform.rotation = cameraTargetRotation;
        }
    }
    private GameObject CheckObjectsBelow()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            GameObject objectBelow = hit.collider.gameObject;
            return objectBelow;
        }
        else
        {
            return null;
        }
    }
    void PlayDeathSound()
    {
        audioSource.clip = deathClip;
        audioSource.Play();

    }
}
