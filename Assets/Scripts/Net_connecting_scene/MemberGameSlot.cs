using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NetSystem;
using Localisation;

public class MemberGameSlot : MonoBehaviour
{
    readonly Color green = new Color(0.44f, 1, 0.41f, 1);
    readonly int big = 25;
    readonly Color blue = new Color(0.22f, 0.83f, 0.33f, 1);
    readonly int small = 20;
    readonly Color red = new Color(0.83f, 0.22f, 0.266f, 1);


    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text body;
    [SerializeField] TMP_Text turn;
    [SerializeField] Image EmotionImage;
    [SerializeField] Text ButtonText;
    [SerializeField] Button enterButton;

    public NetworkGame Game { get; private set; }
    public Button EnterButton { get => enterButton; private set => enterButton = value; }

    public void Fill(NetworkGame game)
    {
        this.Game = game;

      //  title.text = (string)game.GroupName;

       // body.text = $"TODO: fill something here, maybe the turn number or time was created, etc";

        // todo fill emotion image

        bool? ourTurn = NetUtility.AllowedToTakeTurn(game.CurrentGameData);

        switch (ourTurn)
        {
            case null:
                turn.text = Localiser.GetText(TextID.GameSelect_PlayerSlot_Fill_Error); //"Error!";
                turn.color = red;
                turn.fontSize = big;
                EnterButton.interactable = false;
                break;

            case false:
                turn.text = Localiser.GetText(TextID.GameSelect_PlayerSlot_Fill_CompanionTurn);// "Companion's Turn";
                turn.color = blue;
                turn.fontSize = small;
                ButtonText.text = Localiser.GetText(TextID.GameSelect_PlayerSlot_Fill_Veiw); //"Veiw";
                EnterButton.interactable = true;

                if (!GameCore.GameManager.DoLiveUpdate)
                {
                    EnterButton.gameObject.SetActive(false);
                }

                break; 
            
            case true:
                turn.text = Localiser.GetText(TextID.GameSelect_PlayerSlot_Fill_YourTurn); //"Your Turn!";
                turn.color = green;
                turn.fontSize = big;
                ButtonText.text = Localiser.GetText(TextID.GameSelect_PlayerSlot_Fill_Play);//"Play";
                EnterButton.interactable = true;
                break;
        }

        

    }

    private void OnDestroy()
    {
        EnterButton.onClick.RemoveAllListeners();
    }

    //public void EnterGame()
    //{
    //   // APIOperationCallbacks<NetworkGame> callbacks = new APIOperationCallbacks<NetworkGame>(EnterGameSucess, EnterGameFailure);
    // //   NetworkHandler.Instance.ResumeMemberGame(Game);
    //}

}
