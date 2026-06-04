using UnityEngine;
using System;

public class Kanye : MonoBehaviour
{
    public GameObject WallUI;
    public int kanyeNumber;
    public Texture KanyeJpeg;
    public bool kanyeWasRun = false;
    System.Random random = new System.Random();
    private bool previousScreenToggled = false;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Renderer renderer = WallUI.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Background.ifScreenToggled && !kanyeWasRun)
        {
            kanyeWasRun = true;
            kanyeNumber = random.Next(1, 101);
        }
        if (Background.ifScreenToggled && !previousScreenToggled)
        {
            kanyeWasRun = false;
        }
        previousScreenToggled = Background.ifScreenToggled;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            kanyeNumber = random.Next(1, 101);
        }
        if (kanyeNumber == 55)
        {
            Debug.Log("Kanye");
            kanyeTheTexture();
        }
    }
    public void kanyeTheTexture()
    {
        GetComponent<Renderer>().material.mainTexture = KanyeJpeg;
        WallUI.GetComponent<Renderer>().material.mainTexture = KanyeJpeg;
    }
}
