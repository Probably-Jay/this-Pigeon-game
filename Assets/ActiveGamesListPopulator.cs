using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetSystem;
using UnityEngine;

// jay 21/04

public class ActiveGamesListPopulator : MonoBehaviour
{
    [SerializeField] SceneInterface.ConnectingSceneInterface sceneInterface;
    [SerializeField] Transform content;
    [SerializeField] GameObject gameSlotPrefab;
    [SerializeField] float slotHeight;
    [SerializeField] float slotPadding;

    private List<MemberGameSlot> elements = new List<MemberGameSlot>();


    public void Populate(ReadOnlyCollection<NetSystem.NetworkGame> games)
    {
        if (!ChangesToList(games))
        {
            return;
        }

        ClearCurrentDisplay();

        RePopulate(games);
    }

    private void RePopulate(ReadOnlyCollection<NetworkGame> games)
    {
        for (int i = 0; i < games.Count; i++)
        {
            NetworkGame game = games[i];

            var slotObject = Instantiate(gameSlotPrefab, content);
            float yPos = -(slotHeight/2f + (slotHeight * i)+slotPadding);
            var pos = new Vector3(310, yPos, 0);
            slotObject.GetComponent<RectTransform>().localPosition = pos;
            


            var slot = slotObject.GetComponent<MemberGameSlot>();
            slot.Fill(game);

            slot.EnterButton.onClick.AddListener(() => sceneInterface.ResumeGame(game));

            elements.Add(slot);
        }
    }

    private void ClearCurrentDisplay()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            Destroy(elements[i].gameObject);
        }

        elements.Clear();
    }

    private bool ChangesToList(ReadOnlyCollection<NetworkGame> games)
    {
        if(games.Count != elements.Count)
        {
            return true;
        }

        for (int i = 0; i < games.Count; i++)
        {
            if(games[i].GroupEntityKey.Id != elements[i].Game.GroupEntityKey.Id)
            {
                return true;
            }
        }

        return false;
    }
}
