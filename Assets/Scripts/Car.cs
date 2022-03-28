
using UnityEngine;

public class Car : MonoBehaviour
{
    public enum Color
    {
        red,
        orange,
        green,
        pink,
        yellow
    }

    public Color currentColor = Color.red;

    public enum CleaningState
    {
       bubble,
       dirty,
       cleany,
       waterly
    }

    public CleaningState currentCleaningState = CleaningState.bubble;
}
