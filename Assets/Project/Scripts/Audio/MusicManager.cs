using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [ReadOnly] public BeatData currentBeatData;
    [ReadOnly] public AudioSource audioSource;
    public float volume = 0.75f;
    public List<BeatData> music = new();
    
    private readonly Queue<BeatData> _queue = new();
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        audioSource = new GameObject("Music").AddComponent<AudioSource>();
        audioSource.volume = volume;
        QueueMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (_queue.Count == 0) QueueMusic();
            var next = _queue.Dequeue();
            audioSource.clip = next.music;
            currentBeatData = next;
            audioSource.Play();
        }
    }

    private void QueueMusic()
    {
        music.ForEach(clip => _queue.Enqueue(clip));
    }
}
