using UnityEngine.UI;
using UnityEngine;

public class player : MonoBehaviour
{
    public Text currentScore;
    public Text highScore;
    int Score = 0;


    public Animator anim;
    public float jumpPower = 1000f;
    Rigidbody2D myRigidbody;
    bool isGround = false;
    bool isGameOver = false;
    public float xPos = -8f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }


    void FixedUpdate()
    {
        if (!isGameOver)
        {
            Score++;
            currentScore.text = Score.ToString();
        }
        if (Score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", Score);
            highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        }

        if (isGameOver)
        {
            return;
        }

        //if (Input.GetKey(KeyCode.Space) && isGround) //電腦空白鍵控制
        if ((Input.touchCount == 1 || Input.acceleration.z >= -0.2f) && isGround) //手機觸控及加速度控制
        {
            myRigidbody.AddForce(Vector3.up * jumpPower * Time.deltaTime * myRigidbody.mass * myRigidbody.gravityScale);
            anim.SetBool("Jump", true);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGround = true;
            anim.SetBool("Jump", false);
        }
        if (collision.collider.tag == "Challenges")
        {
            GameOver();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGround = true;
            anim.SetBool("Jump", false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGround = false;
            anim.SetBool("Jump", true);
        }
    }
   

    public void GameOver()
    {
        myRigidbody.gravityScale = 0f;
        isGameOver = true;
        FindObjectOfType<ChallengeScroller>().GameOver();
        FindObjectOfType<scroll>().xVel = 0f;
        FindObjectOfType<ScrollClouds>().xVel = 0f;
        anim.SetBool("GameOver", true);

        FindObjectOfType<Score>().GameOver();
    }
}
