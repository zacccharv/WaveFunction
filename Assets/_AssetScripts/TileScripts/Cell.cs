using UnityEngine;

public class Cell : MonoBehaviour
{
    public event System.Action tileCollapseEvent;

    public bool collapsed;

    public int index = 1;

    public GridManager GridManager { get { return transform.parent.gameObject.GetComponent<GridManager>(); } }
    
    public JobManager JobManager { get { return FindAnyObjectByType<JobManager>(); } }

    public void OnCollapsed()
    {
        tileCollapseEvent?.Invoke();
    }
}