using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    public static SFXHandler Instance;
    
    public List<AudioClipRef> sfx = new();
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public AudioClip GetSFX(string tag)
    {
        return (from sfxRef in sfx where sfxRef.tag == tag select sfxRef.clip).FirstOrDefault();
    }
    
    [System.Serializable]
    public class AudioClipRef
    {
        public string tag;
        public AudioClip clip;
    }
}
