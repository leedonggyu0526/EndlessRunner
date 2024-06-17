using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;
    bool isGrounded = false;

    public float speed = 5;
    [SerializeField] Rigidbody rb;

    float horizontalInput;
    [SerializeField] float horizontalMultiplier = 2;

    public float speedIncreasePerPoint = 0.3f;

    [SerializeField] float jumpForce = 400f;
    [SerializeField] LayerMask groundMask;

    private float lastJumpTime;
    public float jumpCooldown = 1f; // 점프 쿨다운 시간 (초 단위)

    private BackgroundMusicController backgroundMusicController;
    private AudioSource audioSource;
    public AudioClip deathClip;

    private Animator animator;

    private void Start()
    {
        backgroundMusicController = FindObjectOfType<BackgroundMusicController>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (!alive || !GameStarter.gameStarted) return;

        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);

        if (isGrounded && rb.velocity.magnitude > 0)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastJumpTime + jumpCooldown)
        {
            //Debug.Log("Jump");
            Jump();
        }

        if (transform.position.y < -5)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            //Debug.Log("Rotate");
            RotatePlayerAndCamera();
        }
    }

    public void Die()
    {   
        if(alive)
        {
            alive = false;
            GameStarter.GameOver();
            backgroundMusicController.StopMusic();
            PlayDeathSound();
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Jump()
    {
        if (!animator.GetBool("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time; // 마지막 점프 시간 업데이트
            animator.SetBool("Jump", true);
            isGrounded = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        animator.SetBool("Jump", false);
        isGrounded = true;
        if (collision.gameObject.CompareTag("Obstacle"))
        {   
            //Debug.Log(collision.gameObject.tag);
            animator.SetTrigger("HitObstacle"); // HitObstacle 트리거 설정하여 넘어지는 애니메이션 재생
            Die(); // 죽음 처리
        }
    }

    void RotatePlayerAndCamera()
    {
        GameObject belowObject = CheckObjectsBelow();
        //Debug.Log(belowObject);
        if (belowObject == null) {
            Debug.Log("isNull");
        }else if(belowObject.name == "Road_Left_Corner")
        {   
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, -90f, 0f));
            transform.rotation = targetRotation;

            Quaternion cameraTargetRotation = Quaternion.Euler(Camera.main.transform.eulerAngles + new Vector3(0f, -90f, 0f));
            Camera.main.transform.rotation = cameraTargetRotation;
        }else if(belowObject.name == "Road_Right_Corner")
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
        bool isHit = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out hit);
        if (isHit)
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
