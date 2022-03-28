using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssingnMe : MonoBehaviour
{
    public enum ControlType
    {
        player,
        enemy
    }

    public ControlType currentControl = ControlType.enemy;

    void Awake()
    {
        switch (currentControl)
        {
            case ControlType.player:
                PlayerController.instance.FinishLine = this.gameObject;
                break;
            case ControlType.enemy:
                AiController.instance.FinishLine = this.gameObject;
                break;
        }
        
    }
}
