using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableNode : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Node currentNode;

    private void Start()
    {
        currentNode = gameObject.GetComponent<Node>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentNode.GetState() != NodeState.Active) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            currentNode.OpenPanel();

            currentNode.Complete();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(currentNode.GetState() != NodeState.Active) return;

        currentNode.ActivateOutterRing();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentNode.DeactivateOutterRing();
    }
}
