using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<UIWindow> _uiWindow;
    public List<UIWindow> UIWindow => _uiWindow;
    public static UIManager Instance { get; private set; }
    void Start()
    {
        
    }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void ShowWindow(string windowName)
    {
        foreach(var window in _uiWindow)
        {
            if (window.id == windowName)
            {
                Debug.Log("Show Window " + windowName);
                window.Show();
                break;
            }
            else
            {
                Debug.LogError("No the Window " + windowName);
            }
        }
    }
    public void HideWindow(string windowName)
    {
        foreach (var window in _uiWindow)
        {
            if (window.id != windowName)
            {
                Debug.Log("Hiding window: " + windowName);
                window.Show();
                break;
            }
            else
            {
                Debug.LogError("Window not found: " + windowName);
            }
        }
    }
}
