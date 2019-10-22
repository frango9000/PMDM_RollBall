using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{

	public GameObject rawHeartObject1;
	public GameObject rawHeartObject2;
	public GameObject rawHeartObject3;
	
	public RawImage rawHeartImage1;
	public RawImage rawHeartImage2;
	public RawImage rawHeartImage3;

	public Texture emptyHeart;
	public Texture fullHeart;
	
	
	// Use this for initialization
	void Start () {
		rawHeartImage1 = (RawImage)rawHeartObject1.GetComponent<RawImage>();
		rawHeartImage2 = (RawImage)rawHeartObject2.GetComponent<RawImage>();
		rawHeartImage3 = (RawImage)rawHeartObject3.GetComponent<RawImage>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
