using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public GameObject startScreen; // 시작 화면 UI
    public Button startButton; // 시작 버튼
    public GameObject scoreText; // 스코어 텍스트 UI
    public Button exitButton; // 종료 버튼
    public Button restartButton; // 재시작 버튼
    public GameObject gameOverPanel; // 게임오버 패널

    public static bool gameStarted = false; // 게임 시작 여부

    void Start()
    {
        scoreText.SetActive(false); // 게임 시작 전에 스코어 텍스트 비활성화
        gameOverPanel.SetActive(false); // 게임오버 패널 초기 비활성화

        startButton.onClick.AddListener(StartGame); // 시작 버튼 클릭 이벤트 추가
        exitButton.onClick.AddListener(ExitGame); // 종료 버튼 클릭 이벤트 추가
        restartButton.onClick.AddListener(RestartGame); // 재시작 버튼 클릭 이벤트 추가
    }

    void StartGame()
    {
        gameStarted = true;
        startScreen.SetActive(false); // 시작 화면 비활성화
        scoreText.SetActive(true); // 스코어 텍스트 활성화
    }

    void ExitGame()
    {
        // Unity Editor에서 실행 중인 경우 플레이 모드를 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 빌드된 게임을 종료
#endif
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
