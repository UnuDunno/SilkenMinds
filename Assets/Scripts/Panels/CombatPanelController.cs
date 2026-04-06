using UnityEngine;

public class CombatPanelController : PanelController
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject playerHandArea;
    [SerializeField] private GameObject enemyArea;

    private void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(cardPrefab, playerHandArea.transform);
        }
    }

    public void EndTurn()
    {
        ClosePanel();
    }
}
