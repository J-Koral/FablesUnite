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
    public int squadSize = 9;

    public OwnedFable SelectedOwned { get; private set; }
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
        if (SelectedOwned == null) return;          // must pick a Fable first
        Transform slot = lane.GetNextFreeSlot();
        if (slot == null) return;                   // lane full

        var def = GameData.I.database.Get(SelectedOwned.definitionId);
        if (def == null) return;

        GameObject go = Instantiate(defenderPrefab, slot.position, Quaternion.identity);
        Unit unit = go.GetComponent<Unit>();
        unit.Configure(def, SelectedOwned.level, SelectedOwned.stars);  // stats + element from data
        unit.AssignLane(lane);

        unitsRemaining--;
        UpdateUI();
    }

    // Hooked from a generated deploy button — selects which owned Fable to place.
    public void SelectOwned(int rosterIndex)
    {
        var roster = GameData.I.player.roster;
        if (rosterIndex >= 0 && rosterIndex < roster.Count)
            SelectedOwned = roster[rosterIndex];
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (unitsRemainingText == null) return;
        string who = "none";
        if (SelectedOwned != null)
        {
            var def = GameData.I.database.Get(SelectedOwned.definitionId);
            if (def != null) who = def.displayName;
        }
        unitsRemainingText.text = "Units left: " + unitsRemaining + "   (" + who + ")";
    }
}
