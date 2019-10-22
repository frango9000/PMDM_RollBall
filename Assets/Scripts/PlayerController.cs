using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
using UnityEngine.Experimental.PlayerLoop;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int score = 0;
    private int scoreLVL0 = 0;
    private int scoreLVL1 = 0;
    private int scoreLVL2 = 0;
    private int scoreLVL3 = 0;

    private int hearts = 0;

    public Transform startPos;
    
    
    protected Animator animator;

    public Transform[] targets;
    private bool[] targetsDanger;

    public float enemyProximity;

    public GameObject bridge01;
    public GameObject bridge12;
    public GameObject bridge23;
    public GameObject bridge34;


    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public GameObject heartUi;

    private Texture2D emptyHeart;
    private Texture2D fullHeart;
    
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        score = 0;
        SetScoreText ();
        winText.text = "";
        targetsDanger = new bool[targets.Length];
        for (int i = targetsDanger.Length - 1; i >= 0; i--)
        {
            targetsDanger[i] = false;
        }
    }

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        if(!animator.GetBool("isDead"))
            rb.AddForce (movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        String targetTag = other.gameObject.tag;
        if (targetTag.StartsWith("Coin"))
        {
            other.gameObject.SetActive(false);
            score = score + 1;
            SetScoreText();

            String[] tag = other.gameObject.tag.Split('L');

            switch (tag[1])
            {
                case "0":
                    scoreLVL0 += 1;
                    Debug.Log(scoreLVL0);
                    if (scoreLVL0 == 3)
                    {
                        bridge01.GetComponent<Animator>().SetBool("isClosed", false);
                    }
                    else if (scoreLVL0 == 5)
                    {
                        heart1.GetComponent<Animator>().SetBool("isGranted", true);
                    }
                    break;
                case "1":
                    scoreLVL1 += 1;
                    Debug.Log(scoreLVL1);
                    if (scoreLVL1 == 3)
                    {
                        bridge12.GetComponent<Animator>().SetBool("isClosed", false);
                    }
                    else if (scoreLVL1 == 5)
                    {
                        heart2.GetComponent<Animator>().SetBool("isGranted", true);
                    }
                    break;
                case "2":
                    scoreLVL2 += 1;
                    Debug.Log(scoreLVL2);
                    if (scoreLVL2 == 3)
                    {
                        bridge23.GetComponent<Animator>().SetBool("isClosed", false);
                    }
                    else if (scoreLVL2 == 5)
                    {
                        heart3.GetComponent<Animator>().SetBool("isGranted", true);
                    }
                    break;
                case "3":
                    scoreLVL3 += 1;
                    Debug.Log(scoreLVL3);
                    if (scoreLVL3 == 3)
                    {
                        bridge34.GetComponent<Animator>().SetBool("isClosed", false);
                    }
                    break;

            }
        }
        else if (targetTag.Equals("Heart"))
        {
            other.transform.parent.gameObject.SetActive(false);
            hearts += 1;
            Debug.Log("Hearts = " + hearts);
            updateHeartsUI();
        }
        else if (targetTag.StartsWith("Enemy"))
        {
            rb.velocity = Vector3.zero;
            hearts -= 1;
            animator.SetBool("isDead", true);
            

            Debug.Log("Hearts = " + hearts);
            updateHeartsUI();
        }
        
    }

    private void resurrect()
    {
        animator.SetBool("isResurrecting", true);
    }

    private void reset()
    {
        transform.position = startPos.position;
        animator.SetBool("isDead", false);
    }

    private void updateHeartsUI()
    {
//        int children = ui.transform.childCount;
        
        for (int i = 0; i < heartUi.transform.childCount; i++)
        {
            if (hearts > i)
            {
                heartUi.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }else heartUi.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
    }

    void Update ()
    {
//        Debug.Log(bridge01.GetComponent<Animator>().GetBool("youShallNotPass"));

        for (int i = 0; i < targets.Length; i++)
        {
            float distance = Vector3.Distance(gameObject.transform.position, targets[i].position);
//            Debug.Log("dist : " + (distance <= enemyProximity));
            targetsDanger[i] = distance <= enemyProximity;

        }
//        Debug.Log(Array.IndexOf(targetsDanger, true));
        if(Array.IndexOf(targetsDanger, true) > -1)
        {
//            Debug.Log("isInDanger");
            // cambiamos a true la variable del animator
            animator.SetBool("isInDanger", true);
        }
        else
        {
//            Debug.Log("notInDanger");
            // cambiamos a false la variable del animator
            animator.SetBool("isInDanger", false);
        }
    }
    void SetScoreText ()
    {
        countText.text = "Coins: " + score.ToString ();
        if (score >= 25)
        {
            winText.text = "You Win!";
        }
    }
}