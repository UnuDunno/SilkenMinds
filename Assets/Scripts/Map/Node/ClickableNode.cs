using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableNode : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Node CurrentNode;

    private void Start()
    {
        CurrentNode = gameObject.GetComponent<Node>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurrentNode.GetState() != NodeState.Active) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CurrentNode.OpenPanel();

            CurrentNode.Complete();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(CurrentNode.GetState() != NodeState.Active) return;

        CurrentNode.ActivateOutterRing();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CurrentNode.DeactivateOutterRing();
    }
}
