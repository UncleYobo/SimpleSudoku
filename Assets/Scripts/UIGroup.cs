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
        Group.alpha = 0f;
    }

    public void ToggleGroup()
    {
        isEnabled = !isEnabled;

        Group.interactable = isEnabled;
        Group.blocksRaycasts = isEnabled;
    }

    void LateUpdate()
    {
        if (isEnabled)
        {
            if (Group.alpha < 1f) Group.alpha += Time.deltaTime * 2f;
        }
        else
        {
            if (Group.alpha > 0f) Group.alpha -= Time.deltaTime * 2f;
        }
    }

    public void OpenGroup()
    {
        isEnabled = true;
        Group.interactable = isEnabled;
        Group.blocksRaycasts = isEnabled;
    }
    public void CloseGroup()
    {
        isEnabled = false;
        Group.interactable = isEnabled;
        Group.blocksRaycasts = isEnabled;
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
