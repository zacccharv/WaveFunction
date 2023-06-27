using UnityEngine;

public class Cell : MonoBehaviour
{
    public event System.Action tileCollapseEvent;
    public bool collapsed;
    public int index = 1;
    public GridManager GridManager { get { return transform.parent.gameObject.GetComponent<GridManager>(); } }

    public void OnCollapsed()
    {
        tileCollapseEvent?.Invoke();
    }
}