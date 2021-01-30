using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGroup : MonoBehaviour
{
    public CanvasGroup Group;

    private bool isEnabled;

    private void Start()
    {
        CloseGroup();
    }

    public void ToggleGroup()
    {
        isEnabled = !isEnabled;

        Group.interactable = isEnabled;
        Group.blocksRaycasts = isEnabled;

        if (isEnabled)
        {
            Group.alpha = 1f;
        } else
        {
            Group.alpha = 0f;
        }
    }

    public void OpenGroup()
    {
        isEnabled = true;
        Group.interactable = isEnabled;
        Group.blocksRaycasts = isEnabled;
        Group.alpha = 1f;
    }
    public void CloseGroup()
    {
        isEnabled = false;
        Group.interactable = isEnabled;
        Group.blocksRaycasts = isEnabled;
        Group.alpha = 0f;
    }

    public void QuitGame()
    {
        GameObject.Find("MGMT").GetComponent<GameplayManager>().QuitGame();
    }
    public void NewGame(string difficulty)
    {
        GameObject.Find("MGMT").GetComponent<GameplayManager>().NewGame(difficulty);
    }
}
