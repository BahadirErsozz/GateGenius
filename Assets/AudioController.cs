using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioSource audioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(audioSource);
    }
    private void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat("musicVolume");
        Debug.Log("ses seviyesi" + audioSource.volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
