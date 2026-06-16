using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Deployer : MonoBehaviour
{
    [Header("References")]
    public BattleManager battleManager;
    public GameObject defenderPrefab;
    public Camera gameCamera;
    public TMP_Text unitsRemainingText;

    [Header("Deploy Settings")]
    public Element selectedElement = Element.Water;
    public int squadSize = 9;

    private int unitsRemaining;

    private void Start()
    {
        unitsRemaining = squadSize;
        if (gameCamera == null) gameCamera = Camera.main;
        UpdateUI();
    }

    private void Update()
    {
        if (battleManager.CurrentState != BattleState.Deploy) return;
        if (unitsRemaining <= 0) return;
        if (!Input.GetMouseButtonDown(0)) return;

        // Ignore clicks that land on UI (buttons), not the battlefield.
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        PlaceAtCursor();
    }

    private void PlaceAtCursor()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f))
        {
            Lane lane = hit.collider.GetComponentInParent<Lane>();
            if (lane != null) PlaceDefender(lane);
        }
    }

    private void PlaceDefender(Lane lane)
    {
        Transform slot = lane.GetNextFreeSlot();
        if (slot == null) return; // lane full

        GameObject go = Instantiate(defenderPrefab, slot.position, Quaternion.identity);
        Unit unit = go.GetComponent<Unit>();
        unit.team = Team.Defender;
        unit.element = selectedElement;
        unit.AssignLane(lane);

        unitsRemaining--;
        UpdateUI();
    }

    // Hooked to element buttons (pass 0=Water,1=Fire,2=Grass,3=Ground,4=Electric).
    public void SelectElement(int elementIndex)
    {
        selectedElement = (Element)elementIndex;
    }

    private void UpdateUI()
    {
        if (unitsRemainingText != null)
            unitsRemainingText.text = "Units left: " + unitsRemaining + "   (" + selectedElement + ")";
    }
}
