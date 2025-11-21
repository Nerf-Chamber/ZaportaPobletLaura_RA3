using TMPro;
using UnityEngine;

public class GameOverMenu : BaseMenu
{
    [SerializeField] private TextMeshProUGUI GameOverText;
    [SerializeField] private TextMeshProUGUI GameOverStateText;

    private string win = "Congrats! You won :D";
    private string loose = "Nooooo, you lost :c";

    // Garanteix que el menú només s'obre una vegada
    private bool wasNotInGame = false;

    private void Update()
    {
        if ((Player.isDead || Player.didWin) && !wasNotInGame)
        {
            wasNotInGame = true;
            ShowMenu();
        }
    }
    public override void Restart()
    {
        wasNotInGame = false;     
        base.Restart();      
    }
    protected override void ShowMenu()
    {
        base.ShowMenu();
        if (Player.isDead)
        {
            GameOverText.color = Color.red;
            GameOverStateText.text = loose;
        } 
        else
        {
            GameOverText.color = Color.green;
            GameOverStateText.text = win;
        }
    }
}