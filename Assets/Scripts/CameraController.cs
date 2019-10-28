using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;
    public GameObject player;

    //guardamos la distancia y angulo inicial entre la camara y el jugador 
    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    //en cada frame se mantiene el offset para que la camara siga al jugador
    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}