using UnityEngine;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    public Texture emptyHeart;
    public Texture fullHeart;

    public RawImage rawHeartImage1;
    public RawImage rawHeartImage2;
    public RawImage rawHeartImage3;

    public GameObject rawHeartObject1;
    public GameObject rawHeartObject2;
    public GameObject rawHeartObject3;


    // Use this for initialization
    private void Start()
    {
        rawHeartImage1 = rawHeartObject1.GetComponent<RawImage>();
        rawHeartImage2 = rawHeartObject2.GetComponent<RawImage>();
        rawHeartImage3 = rawHeartObject3.GetComponent<RawImage>();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}