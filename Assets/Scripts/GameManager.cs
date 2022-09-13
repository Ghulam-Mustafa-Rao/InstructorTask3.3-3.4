using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float xBoundaries = 38;
    public Vector3 mousePosition;
    public Vector2 position;
    public bool gameOver = false;
    public GameObject bulletPrefab;
    public GameObject bulletSpwaner;
    public float bulletYOffset = 10;
    GameObject bullet;
    public float playerSpeed = 1;
    public static GameManager gameManager;
    public int waveNo = 1;
    public GameObject CoinPrefab;
    public GameObject CoinHolder;
    public int coinsGahtered = 0;
    public TextMeshProUGUI coinsText;
    public ParticleSystem gameOverEffect;

    public AudioClip gameOverClip;
    public AudioClip coinCollectedClip;
    public AudioClip stoneDestroyedClip;
    public AudioSource playerAudioSource;

    public Slider slider;
    public TextMeshProUGUI previousScoreText;
    public TextMeshProUGUI nextScoreText;
    public GameObject gameOverTextObject;

    int playerlevel = 1;
    public TextMeshProUGUI playerlevelText;
    int previousScore = 0;
    int nextScore = 10000;


    public int score = 0;

    public Animator animator;
    bool gameOverCalled = false;

    public int bulletShoot = 0;
    public int playerLifes = 3;

    public TextMeshProUGUI playerLivestext;
    public bool pauseAllballs = false;
    public TextMeshProUGUI pauseTimer;
    public bool startTimer = false;

    public GameObject background;
    public Sprite[] backgroundSprites;
    int backgroundSpriteCounter = 0;
    int totalTime = 6;
    float counter = 0;
    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        slider.GetComponent<Slider>().maxValue = nextScore;
        slider.GetComponent<Slider>().minValue = previousScore;
        previousScoreText.text = previousScore.ToString();
        nextScoreText.text = nextScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {
            transform.Translate(Vector3.right * playerSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
        }
        else
        {
            //idle state
            animator.SetBool("walkleft_b", false);
            animator.SetBool("walkright_b", false);
            animator.SetBool("shoot_b", false);
            animator.SetBool("idle_b", true);
        }

        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKeyUp(KeyCode.RightArrow))
        {
            //walk right 
            animator.SetBool("walkleft_b", false);
            animator.SetBool("idle_b", false);
            animator.SetBool("shoot_b", false);
            animator.SetBool("walkright_b", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKeyUp(KeyCode.LeftArrow))
        {
            //walk left
            animator.SetBool("idle_b", false);
            animator.SetBool("walkright_b", false);
            animator.SetBool("shoot_b", false);
            animator.SetBool("walkleft_b", true);
        }
        else
        {
            //idle state
            animator.SetBool("walkleft_b", false);
            animator.SetBool("walkright_b", false);
            animator.SetBool("shoot_b", false);
            animator.SetBool("idle_b", true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Shoot bullet
            SpwanBullets();
            animator.SetBool("walkleft_b", false);
            animator.SetBool("idle_b", false);
            animator.SetBool("walkright_b", false);
            animator.SetBool("shoot_b", true);
        }

        coinsText.text = "Coins : " + coinsGahtered;


        //set  Level and level Score
        if (score > nextScore)
        {
            previousScore = nextScore;
            playerlevel++;
            backgroundSpriteCounter++;
            if (backgroundSpriteCounter >= backgroundSprites.Length)
            {
                backgroundSpriteCounter = 0;
            }
            background.GetComponent<Image>().sprite = backgroundSprites[backgroundSpriteCounter];
            nextScore += 5000 * playerlevel;


        }

        //set slider value
        slider.maxValue = nextScore;
        slider.minValue = previousScore;
        slider.value = score;
        previousScoreText.text = previousScore.ToString();
        nextScoreText.text = nextScore.ToString();
        playerlevelText.text = "Level : " + playerlevel;


        //Game Over
        if (playerLifes <= 0)
        {
            playerLifes = 0;
            playerLivestext.text = "Lives : " + 0;
            if (!gameOverCalled)
            {
                gameOverEffect.Play();
                gameOverCalled = true;
                StartCoroutine(gameOverCo());
            }

            gameOverTextObject.SetActive(true);
        }

        //Pause power timer
        if (startTimer)
        {
            counter += Time.deltaTime;

            if (counter >= 1)
            {
                counter = 0;
                totalTime--;
                pauseTimer.text = "Time Left : " + totalTime;

            }
        }
        else
        {
            pauseTimer.text = "";
        }

        playerLivestext.text = "Lives : " + playerLifes;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LeftBound"))
        {
            transform.position = new Vector3(transform.position.x + 2, transform.position.y, 80);
        }
        if (collision.gameObject.CompareTag("RightBound"))
        {
            transform.position = new Vector3(transform.position.x - 2, transform.position.y, 80);
        }
    }
    private void FixedUpdate()
    {

    }

    void SpwanBullets()
    {

        bullet = Instantiate(bulletPrefab, new Vector3(bulletSpwaner.transform.position.x, bulletSpwaner.transform.position.y, 0), bulletPrefab.transform.rotation);

        //bullet.transform.SetParent(bulletSpwaner.transform);

        bullet.transform.position = new Vector3(bulletSpwaner.transform.position.x, bulletSpwaner.transform.position.y + bulletYOffset, 0);
        bullet.transform.localScale = new Vector3(10, 10, 1);
        bullet.GetComponent<MoveBullet>().moveBulletUp = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coins"))
        {
            playerAudioSource.PlayOneShot(coinCollectedClip);
            coinsGahtered++;
            Destroy(collision.gameObject);
        }
    }


    IEnumerator gameOverCo()
    {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
    }

    public void pauseAllBallsTimer()
    {
        StartCoroutine(pauseAllBallsTimerCo());
    }
    IEnumerator pauseAllBallsTimerCo()
    {
        totalTime = 6;
        counter = 0;
        startTimer = true;
        pauseTimer.text = "Time Left : " + totalTime;

        yield return new WaitForSeconds(6f);

        startTimer = false;
        pauseAllballs = false;
    }

}
