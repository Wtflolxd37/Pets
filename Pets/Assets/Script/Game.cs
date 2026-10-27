using Dino.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public TextMeshProUGUI money;
    private float _counter = 1;
    private int _moneyCurrent = 0;
    private TimerBase time;
    private void Start()
    {
        time = GetComponent<TimerBase>();
        Container();
    }
    public void FixedUpdate()
    {
        money.text = _moneyCurrent.ToString();
    }
    public void Container()
    {
        time.StartLoopTimer(_counter, "", true, 1f);
        Debug.Log($"{_counter}");
        time.OnTimerUpdate += AddCoins;
    }

    public void AddCoins(float time)
    {
        _moneyCurrent += 1;
        Debug.Log(_moneyCurrent);    
    }

    public void Purchase()
    {
        int cost;
        if(_moneyCurrent >= 50f)
        {
            cost = 50;
            _moneyCurrent -= cost;
        }
        else { Debug.Log("No se puede comprar"); }
    }
}
