using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialMessages : MonoBehaviour
{
    public string[] Messages;
    [HideInInspector]
    public int CurrentMessageIndex = 0;  // Default to first
    public bool ShowMessages = true;
    public Text tutorialText;

    // Use this for initialization
    void Start()
    {
        // Load messages, if you did not make them in the Editor
        string mess1 = "This story starts with a couple, a wife, Sharon, and a husband, Leonardo, who are so close they can communicate through telepathy. They live in a small rural village in a farmland, where they spend their time peacefully and in harmony with people. \nInvidia, the goddess of jealousy, annoyed by their strong bond, shuts the couple in two different towers of several floors, and challenges them to reach the top in order to be released; each floor has several problems to solve, enemies to beat and difficulties to overcome. \nAnd that’s the easy part! In fact, to test their bond, Invidia swap the sight of the couple, so that Leonardo can only see what Sharon is seeing and Sharon can only see through Leonardo’s eyes... ";
        string mess2 = "So, it begins the journey of Sharon and Leonardo, so as to reunite and defeat the goddess. \nDuring the game, the two protagonists have to pass several levels, in which they have to guess riddles, surpass obstacles, and defeat enemies. \nAs said, you'll see your game partner's levels, and they'll see yours; to surpass each level, you'll have to talk and guide each other towards the end. Beware that levers and buttons in one's screen may produce effects on the other's. \nSo, let's the game begin!";
        string mess3 = "Controls \n" +
                       "-----------------------------------------------\n" +
                       "Left/Right arrow - Move \n" +
                       "Space - Jump \n" +
                       "Up/Down - Climb ladders \n" +
                       "Space + Up - Jump and then climb \n" +
                       "E - Interact/Activate \n" +
                       "Esc - Menu' \n"+
                       "In order to finish the level both players must be on the open brick door \n"+
                       "----------------------------------------------- ";

        Messages = new string[] { mess1, mess2, mess3 };
        tutorialText.text = Messages[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))// Space Click to move forward
        { 
            CurrentMessageIndex++;
            tutorialText.text = Messages[CurrentMessageIndex];
        }

        // Turn off messages if we get to the last one.
        if (CurrentMessageIndex > Messages.GetUpperBound(0))
        {
            ShowMessages = false;
            print("Endend");
            SceneManager.LoadScene("Launcher");
        }

        // Bounds checking
        CurrentMessageIndex = Mathf.Clamp(CurrentMessageIndex, Messages.GetLowerBound(0), Messages.GetUpperBound(0));
    }

    //void OnGUI()
    //{
    //    // Now show the text
    //    if (ShowMessages)
    //        GUI.TextArea(new Rect(200, 70, 500, 200), Messages[CurrentMessageIndex]);
    //}
}