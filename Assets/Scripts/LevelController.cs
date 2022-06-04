using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] float ballsAvailable = 5f;
    [SerializeField] float bombsAvailable = 5f;
    [SerializeField] float timeSlowFactor = 0.5f;
    [SerializeField] float gameOverWaitTime = 5f;
    float normalTime;

    [SerializeField] float[] timeBetweenScrolls;



    [SerializeField] bool canClimberMove = true;
    [SerializeField] bool canBumperMove = true;
    [SerializeField] bool isBallFlying = false;
    [SerializeField] bool isBumperStuck = false;
   

    bool isSwitchingModesPossible = true;
    bool isClimberInWinZone = false;
    bool isBumperInWinZone = false;

    //Cahced Refs
    Cannonball cannonball;
    Cannon cannon;
    SceneLoader sceneLoader;
    CameraScroller cameraScroller;
    Climber climber;
    [SerializeField] TextMeshProUGUI ballsAvailableText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI bombsAvailableText;
    [SerializeField] Button nextLevelButton;
    [SerializeField] GameObject cannonSprite;
    [SerializeField] SpriteRenderer shader;
   
    




    // Start is called before the first frame update
    void Start()
    {
        cameraScroller = FindObjectOfType<CameraScroller>();
        cannonball = FindObjectOfType<Cannonball>();
        cannon = FindObjectOfType<Cannon>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        climber = FindObjectOfType<Climber>();
        cannonball.gameObject.SetActive(false);
        cannonSprite.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);

        //eventually I could take these both and put them in a set up screen function with the timer also
        bombsAvailableText.text = bombsAvailable.ToString();
        ballsAvailableText.text = ballsAvailable.ToString();

        Time.timeScale = 1;


    }

    void Update()
    {
        ShowCannonIfTriggered();
        UpdateTimer();
        WinLevel();
        Debug.Log("isBumperStuck = " + isBumperStuck);
    }

    private void UpdateTimer()
    {
        float minutes = Mathf.FloorToInt(cameraScroller.GetCurrentTimeRemaining() / 60);
        float seconds = Mathf.FloorToInt(cameraScroller.GetCurrentTimeRemaining() % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetClimberInWinZone(bool isClimberInWinZone)
    {
        this.isClimberInWinZone = isClimberInWinZone;
    }

    public void SetBumperInWinZone(bool isBumperInWinZone)
    {
        this.isBumperInWinZone = isBumperInWinZone;
    }

    public float[] GetTimeBetweenScrolls()
    {
        return timeBetweenScrolls;
    }

    public bool GetIsBumperStuck()
    {
        return isBumperStuck;
    }

    public void SetIsBumperStuck(bool isBumperStuckInput)
    {
       // Debug.Log("Set isBumperStuck called");
        isBumperStuck = isBumperStuckInput;
    }

    public bool GetCanBumperMove()
    {
        return canBumperMove;
    }

    public void SetCanBumperMove(bool canBumperMoveInput)
    {
        canBumperMove = canBumperMoveInput;
    }

   

    public bool GetCanClimberMove()
    {
        return canClimberMove;
    }

    public void SetCanClimberMove(bool canClimberMoveInput)
    {
        canClimberMove = canClimberMoveInput;
    }

    public float GetBombsAvailable()
    {
        return bombsAvailable;
    }

 

    void ShowCannonIfTriggered()
    {
        if (isSwitchingModesPossible)
        {
            cannonSprite.SetActive(true);
        }
        else
        {
            cannonSprite.SetActive(false);
        }
    }
    
    public void UpdateBombsAvailable()
    {
        bombsAvailable--;
        bombsAvailableText.text = bombsAvailable.ToString();
    }

    
    public void UpdateBallsAvailable()
    {
        ballsAvailable--;
        ballsAvailableText.text = ballsAvailable.ToString();
    }

    public void BallLost()
    {
        if (ballsAvailable == 0)
        {
            GameOver();
        }
        else if (ballsAvailable < 0)
        {
            Debug.Log("Error: Negative Balls");
        }
    }

    private void WinLevel()
    {
        if (isClimberInWinZone && isBumperInWinZone)
        {
            Debug.Log("Yay! You win!");
            //SetIsBumperStuck(true);
            cameraScroller.SetCanScroll(false);
            Time.timeScale = 0;
            nextLevelButton.gameObject.SetActive(true);

        }
    }

    public void LoadNextScene()
    {
        sceneLoader.LoadNextScene();
    }

    public void ProcessDeath()
    {
        normalTime = Time.timeScale;
       // Time.timeScale = timeSlowFactor;
        //FadeToGray();
        StartCoroutine(GameOver());

    }

    IEnumerator GameOver()
    {

        yield return new WaitForSecondsRealtime(gameOverWaitTime);
        Debug.Log("Game Over");
        Time.timeScale = normalTime;
        sceneLoader.LoadGameOverScreen();

    }

    //this fade method is not working currently. It just jump to full gray.
    private void FadeToGray()
    {
        float progress = 0.0f;
        Color shade = shader.color;
        shade.a = 0f;
        shader.color = shade;

        while (progress < 1)
        {
            Color currentShade = shader.color;
            shader.color = new Color(currentShade.r, currentShade.g, currentShade.b,
                Mathf.Lerp(currentShade.a, 255, progress));
            Debug.Log("Current Shade is " + Mathf.Lerp(currentShade.a, 125, progress));
            progress += Time.deltaTime; //undoes the timeslow so that the fade runs at normal speed
            Debug.Log("Progress is " + progress);
            

        }
    }


  
}
