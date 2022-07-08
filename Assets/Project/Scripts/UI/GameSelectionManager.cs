using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelectionManager : MonoBehaviour
{
    [SerializeField] private UIButton _nextButton;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int activeToggles = 0;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<UIToggle>().isOn)
                activeToggles++;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            var toggle = transform.GetChild(i).GetComponent<UIToggle>();
            toggle.interactable = toggle.isOn || activeToggles < 2;
        }
        
        _nextButton.interactable = activeToggles == 2;
    }

    public void Proceed()
    {
        string sceneName = "";
        
        for (int i = 0; i < transform.childCount; i++)
        {
            var toggle = transform.GetChild(i).GetComponent<UIToggle>();
            string value = toggle.GetComponent<Tag>().value; 
            value = char.ToUpper(value[0]) + value[1..];
            if (toggle.isOn) sceneName += value;
            if (!sceneName.Contains("&")) sceneName += "&";
        }

        SceneManager.LoadScene(sceneName);
    }
}
