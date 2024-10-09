using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private ParticleSystem coinParticleEffect; // 파티클 시스템 참조
    [SerializeField] private AudioClip coinPickupSound; // 코인 효과음 참조

    private AudioSource audioSource;

    private void Awake()
    {
        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌 여부 확인
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Coin picked up!");
            // 점수 증가
            GameManager.inst.IncrementScore();

            // 파티클 이펙트 재생
            var particleEffect = Instantiate(coinParticleEffect, transform.position, Quaternion.identity);
            Destroy(particleEffect.gameObject, 2f); // 파티클 이펙트를 2초 후에 파괴

            // 효과음 재생
            audioSource.PlayOneShot(coinPickupSound);

            // 코인 오브젝트 파괴
            Destroy(gameObject, coinPickupSound.length); // 효과음 재생 후 오브젝트 파괴
        }
    }

    private void Update()
    {
        // 코인 오브젝트 회전
        transform.Rotate(0, 0, turnSpeed);
    }
}
