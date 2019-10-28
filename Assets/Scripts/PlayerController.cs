using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    protected Animator animator;

    public GameObject bridge01;
    public GameObject bridge12;
    public GameObject bridge23;
    public GameObject bridge34;
    public Text countText;

    private Texture2D emptyHeart;

    public float enemyProximity;
    private Texture2D fullHeart;


    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    private int hearts;


    public GameObject heartUi;

    public Light lightL1;
    public Light lightL2;
    public Light lightL3;
    public Light lightL4;

    private Rigidbody rb;
    private int score;
    private int scoreLVL0;
    private int scoreLVL1;
    private int scoreLVL2;
    private int scoreLVL3;
    public float speed;

    public Transform startPos;

    public Transform[] targets;
    private bool[] targetsDanger;
    public Text winText;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        score = 0;
        SetScoreText();
        winText.text = "";
        targetsDanger = new bool[targets.Length];
        for (var i = targetsDanger.Length - 1; i >= 0; i--) targetsDanger[i] = false;
    }

    private void FixedUpdate()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if (!animator.GetBool("isDead"))
            rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        var targetTag = other.gameObject.tag;
        if (targetTag.StartsWith("Coin"))
        {
            other.gameObject.SetActive(false);
            score = score + 1;
            SetScoreText();

            var tag = other.gameObject.tag.Split('L');

            switch (tag[1])
            {
                case "0":
                    scoreLVL0 += 1;
                    Debug.Log(scoreLVL0);
                    if (scoreLVL0 == 1)
                    {
                        bridge01.GetComponent<Animator>().SetBool("isClosed", false);
                        lightL1.gameObject.SetActive(true);
                    }
                    else if (scoreLVL0 == 3)
                    {
                        heart1.GetComponent<Animator>().SetBool("isGranted", true);
                    }

                    break;
                case "1":
                    scoreLVL1 += 1;
                    Debug.Log(scoreLVL1);
                    if (scoreLVL1 == 1)
                    {
                        bridge12.GetComponent<Animator>().SetBool("isClosed", false);
                        lightL2.gameObject.SetActive(true);
                    }
                    else if (scoreLVL1 == 3)
                    {
                        heart2.GetComponent<Animator>().SetBool("isGranted", true);
                    }

                    break;
                case "2":
                    scoreLVL2 += 1;
                    Debug.Log(scoreLVL2);
                    if (scoreLVL2 == 1)
                    {
                        bridge23.GetComponent<Animator>().SetBool("isClosed", false);
                        lightL3.gameObject.SetActive(true);
                    }
                    else if (scoreLVL2 == 3)
                    {
                        heart3.GetComponent<Animator>().SetBool("isGranted", true);
                    }

                    break;
                case "3":
                    scoreLVL3 += 1;
                    Debug.Log(scoreLVL3);
                    if (scoreLVL3 == 1) targets[targets.Length - 1].GetComponent<NavMeshAgent>().speed = 3.5f;

                    if (scoreLVL3 == 3)
                    {
                        bridge34.GetComponent<Animator>().SetBool("isClosed", false);
                        lightL4.gameObject.SetActive(true);
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
            rb.constraints = RigidbodyConstraints.FreezeAll;


            Debug.Log("Hearts = " + hearts);
            updateHeartsUI();
        }
        else if (targetTag.Equals("LevelCoin"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            winText.text = "You win";
            other.gameObject.SetActive(false);
        }
    }

    private void resurrect()
    {
        if (hearts > -1)
        {
            animator.SetBool("isResurrecting", true);
            transform.position = startPos.position;
        }
        else
        {
            winText.text = "Game Over";
        }

        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void reset()
    {
        if (hearts > -1)
        {
            rb.constraints = RigidbodyConstraints.None;
            animator.SetBool("isDead", false);
        }
    }

    private void updateHeartsUI()
    {
//        int children = ui.transform.childCount;

        for (var i = 0; i < heartUi.transform.childCount; i++)
            if (hearts > i)
                heartUi.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            else heartUi.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
    }

    private void Update()
    {
//        Debug.Log(bridge01.GetComponent<Animator>().GetBool("youShallNotPass"));

        for (var i = 0; i < targets.Length; i++)
        {
            var distance = Vector3.Distance(gameObject.transform.position, targets[i].position);
//            Debug.Log("dist : " + (distance <= enemyProximity));
            targetsDanger[i] = distance <= enemyProximity;
        }

//        Debug.Log(Array.IndexOf(targetsDanger, true));
        if (Array.IndexOf(targetsDanger, true) > -1)
            //            Debug.Log("isInDanger");
            // cambiamos a true la variable del animator
            animator.SetBool("isInDanger", true);
        else
            //            Debug.Log("notInDanger");
            // cambiamos a false la variable del animator
            animator.SetBool("isInDanger", false);
    }

    private void SetScoreText()
    {
        countText.text = "Coins: " + score;
        if (score >= 25) winText.text = "You Win!";
    }
}