using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance;
    
    private readonly List<AudioSource> _sources = new();
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        List<AudioSource> toRemove = new();
        foreach (var source in _sources)
        {
            if (source.isPlaying) continue;
            toRemove.Add(source);
            Destroy(source);
        }
        toRemove.ForEach(source => _sources.Remove(source));
    }
    
    public AudioSource Play(AudioClip clip)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        _sources.Add(source);
        return source;
    }
}
