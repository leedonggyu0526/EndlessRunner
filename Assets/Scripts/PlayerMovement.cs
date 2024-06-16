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

    public float speedIncreasePerPoint = 0.1f;

    [SerializeField] float jumpForce = 400f;
    [SerializeField] LayerMask groundMask;

    private float lastJumpTime;
    public float jumpCooldown = 1f; // 점프 쿨다운 시간 (초 단위)

    [SerializeField] private GameObject prefab;
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
        GameStarter.gameStarted = false; // 게임 시작 상태를 false로 설정
        FindObjectOfType<GameStarter>().gameOverPanel.SetActive(true);
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

        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time; // 마지막 점프 시간 업데이트
            animator.SetBool("Jump", true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        animator.SetBool("Jump", false);
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            animator.SetTrigger("HitObstacle"); // HitObstacle 트리거 설정하여 넘어지는 애니메이션 재생
            Die(); // 죽음 처리
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
