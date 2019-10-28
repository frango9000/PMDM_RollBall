using UnityEngine;

public class Rotator : MonoBehaviour
{
    
    //controlador de rotaciones
    
    //eje publico para editar el eje de rotacion desde unity
    public string rotateAxis = "z";
    public int speed = 10;

    
    //multiplicamos por delta time para regularizar las rotaciones por segundo, indiferentemente de el framerate
    private void Update()
    {
        switch (rotateAxis.ToLower())
        {
            case "z":
                transform.Rotate(speed * Time.deltaTime * new Vector3(0, 0, 45));
                break;
            case "y":
                transform.Rotate(speed * Time.deltaTime * new Vector3(0, 45, 0));
                break;
            case "x":
                transform.Rotate(speed * Time.deltaTime * new Vector3(45, 0, 0));
                break;
        }
    }
}