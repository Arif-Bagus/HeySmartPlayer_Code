using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public static TextManager Script;

    private GameManager Game;
    public Text StageText;

    private void Awake()
    { Script = this; }

    public void PrintStage()
    { StageText.text = "Stage : " + Game.Level; }

    public void ObjectsAssignment()
    { Game = GameManager.Script;}
}
