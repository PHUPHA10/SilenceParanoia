using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private Stack<GameObject> menuStack = new Stack<GameObject>();

    void Start()
    {
        // หาหน้าแรกที่ Active อยู่
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                menuStack.Push(child.gameObject);
                break;
            }
        }
    }

    public void OpenMenu(GameObject menu)
    {
        if (menuStack.Count > 0)
            menuStack.Peek().SetActive(false);

        menu.SetActive(true);
        menuStack.Push(menu);
    }

    public void GoBack()
    {
        if (menuStack.Count <= 1)
            return;

        menuStack.Pop().SetActive(false);
        menuStack.Peek().SetActive(true);
    }
}
