using UnityEngine;

public class Cell : MonoBehaviour
{
    public event System.Action tileCollapseEvent;

    public bool collapsed;

    public int index = 1;

    public GridManager GridManager { get { return FindAnyObjectByType<GridManager>(); } }
    public CommandManager CommandManager { get { return FindAnyObjectByType<CommandManager>(); } }

    public void OnCollapsed()
    {
        tileCollapseEvent?.Invoke();
    }
}