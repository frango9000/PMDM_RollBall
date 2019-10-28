using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    //animator que controla el status del jugador ( en peligro, ko, reviviendo)
    protected Animator animator;

    //puebntes que se abriran al pasar de nivel
    public GameObject bridge01;
    public GameObject bridge12;
    public GameObject bridge23;
    public GameObject bridge34;
    public Text countText;

    

    //distancia al enemigo que determina el peligro del jugador
    public float enemyProximity;


    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    //cantidad de vidas del jugador
    private int hearts;


    //representacion Ui de las vidas restantes
    public GameObject heartUi;

    //luces de cada seccion que se iran encendiendo segun se avance de nivel
    public Light lightL1;
    public Light lightL2;
    public Light lightL3;
    public Light lightL4;

    private Rigidbody rb;
    
    //puntuacion total
    private int score;
    
    //puntuaciones por nivel para comprbar avances de nivel y entrega de vidas extra
    private int scoreLVL0;
    private int scoreLVL1;
    private int scoreLVL2;
    private int scoreLVL3;
    
    //velocidad del jugador
    public float speed;

    //posicion inical ( al perder una vida volvemos aqui )
    public Transform startPos;

    //array de enemigos
    public Transform[] targets;
    //array que determina el estatus de peligro de cada enemigo 
    private bool[] targetsDanger;
    
    //texto de Game Over/Win
    public Text winText;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        score = 0;
        SetScoreText();
        winText.text = "";
        //inicialmente el targets danger se inicializa con el mismo tamaño de targets y se llena de falsos
        targetsDanger = new bool[targets.Length];
        for (var i = targetsDanger.Length - 1; i >= 0; i--) targetsDanger[i] = false;
    }

    private void FixedUpdate()
    {
        //fuerzas a aplicarse a el jugador
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector3(moveHorizontal, 0.0f, moveVertical); 
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //verificaciones de colision
        var targetTag = other.gameObject.tag;
        //si tocamos una moneda verificamos a que nivel pretenece la moneda para asignar puntuaciones apropiadamente
        if (targetTag.StartsWith("Coin"))
        {
            //deshabilitamos la moneda que tocamos
            other.gameObject.SetActive(false);
            score = score + 1;
            SetScoreText();

            var tag = other.gameObject.tag.Split('L');

            switch (tag[1])
            {
                case "0"://moneda de nivel 0
                    scoreLVL0 += 1;
                    if (scoreLVL0 == 1)
                    {
                        //activamos la apertura del puente
                        bridge01.GetComponent<Animator>().SetBool("isClosed", false);
                        //encendemos la luza del siguiente nivel
                        lightL1.gameObject.SetActive(true);
                    }
                    else if (scoreLVL0 == 3)
                    {
                        //activamos la vida extra del nivel
                        heart1.GetComponent<Animator>().SetBool("isGranted", true);
                    }

                    break;
                case "1":
                    scoreLVL1 += 1;
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
                    //el enemigo perseguidor esta dormido hastga que recogemos la primera moneda del L3
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
            //si entramos en contacto con un corazon (vida extra)
            other.transform.parent.gameObject.SetActive(false);
            hearts += 1;
            Debug.Log("Hearts = " + hearts);
            updateHeartsUI();
        }
        else if (targetTag.StartsWith("Enemy"))
        {
            //si entramos en contacto con un enemigo
            //congelamos el objeto reducimos las vidas e iniciamos la animacion con isDead
            rb.velocity = Vector3.zero;
            hearts -= 1;
            animator.SetBool("isDead", true);
            rb.constraints = RigidbodyConstraints.FreezeAll;


            Debug.Log("Hearts = " + hearts);
            //actualizamos las vidas en el ui
            updateHeartsUI();
        }
        else if (targetTag.Equals("LevelCoin"))
        {
            //objetivo final del juego
            rb.constraints = RigidbodyConstraints.FreezeAll;
            winText.text = "You win";
            other.gameObject.SetActive(false);
        }
    }

    private void resurrect()
    {
        //si quedan vidas restantes, el jugador volvera al inicio, con una vida menos
        //si no, se acaba el juego
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
        //reactivacion del jugador con una vida menos
        if (hearts > -1)
        {
            rb.constraints = RigidbodyConstraints.None;
            animator.SetBool("isDead", false);
        }
    }

    private void updateHeartsUI()
    {
        //contamos cuantas vidas restan y activamos las imagenes correspondientes en el ui
        for (var i = 0; i < heartUi.transform.childCount; i++)
            if (hearts > i)
                heartUi.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            else heartUi.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
    }

    private void Update()
    {
        //verificamos la distancia con todos los enemigos
        //se establecera true en targetsDanger para cada enemigo dentro de la distancia de peligro
        for (var i = 0; i < targets.Length; i++)
        {
            var distance = Vector3.Distance(gameObject.transform.position, targets[i].position);
            targetsDanger[i] = distance <= enemyProximity;
        }

        //si hay algun true dentro de targetsDanger activamos la animacion de enPeligro
        if (Array.IndexOf(targetsDanger, true) > -1)
            animator.SetBool("isInDanger", true);
        else
            animator.SetBool("isInDanger", false);
    }

    private void SetScoreText()
    {
        //Texto de puntiacion
        countText.text = "Coins: " + score;
        if (score >= 25) winText.text = "You Win!";
    }
}