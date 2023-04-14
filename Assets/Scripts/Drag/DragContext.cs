using System;

public class DragContext<T>
{
    private T currentDraggable;
    private IDroppable<T> container;
    public event Action<T> OnStartDrag;
    public event Action<T> OnDrag;
    public event Action<T> OnEndDrag;
    public void StartDrag(T draggable)
    {
        currentDraggable = draggable;
        OnStartDrag?.Invoke(currentDraggable);
    }

    public void EndDrag()
    {
        OnEndDrag?.Invoke(currentDraggable);
        if (container != null)
        {
            container.OnDrop(currentDraggable);
        }
        currentDraggable = default(T);
    }

    public void ProcessDrag()
    {
        OnDrag?.Invoke(currentDraggable);
    }
    public void EnterContainer(IDroppable<T> container)
    {
        this.container = container;
    }

    public void ExitContainer(IDroppable<T> container)
    {
        if (this.container == container)
        {
            this.container = null;
        }
    }
}

public interface IDroppable<T>
{
    void OnDrop(T item);
}