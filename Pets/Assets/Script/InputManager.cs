using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #region Tap
    public event Action<bool> OntapEvent;
    private bool _isTouching = false;
    #endregion
    #region Continuos
    public event Action<Vector2> OnContinuousEvent;
    #endregion

    void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _isTouching = true;
            OntapEvent?.Invoke(true);
            Debug.Log("Touch started");
        }
        else if(context.canceled)
        {
            OntapEvent?.Invoke(false);
            _isTouching = false;
            Debug.Log("Touch enden");
        }
    }

    public void OnContiouns(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        Debug.Log($"Continuos input value: {value}");
        OnContinuousEvent?.Invoke(value);
    }
}
