using UnityEngine;

public class CellBase : MonoBehaviour
{
    public event System.Action tileCollapseEvent;
    

    public GridManager GridManager { get { return FindAnyObjectByType<GridManager>(); } }
    public CommandManager CommandManager { get { return FindAnyObjectByType<CommandManager>(); } }
    
    [field: SerializeField] public int Index { get; set; } = 0;
    [field: SerializeField] public bool Collapsed { get; set; }

    public void OnCollapsed()
    {
        tileCollapseEvent?.Invoke();
    }
}