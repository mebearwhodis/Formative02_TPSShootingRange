using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    [SerializeField] GameObject targetPrefab;
    [SerializeField] private TextMeshProUGUI _targetCount;

    private int _totalTargets;
    private int _activeTargets;

    private void Start()
    {
        // Get the initial count of active targets
        _activeTargets = GameObject.FindGameObjectsWithTag("Target").Length;
        _totalTargets = _activeTargets;

        _targetCount.text = "Remaining Targets: " + _activeTargets + "/" + _totalTargets;
    }

    private void Update()
    {
        // Check the current count of active targets
        _activeTargets = GetActiveTargetsCount();

        // Update the UI text elements
        _targetCount.text = "Remaining targets: " + _activeTargets + "/" + _totalTargets;
        //_targetCount.text = "Remaining targets: " + string.Format("{0:00}/{1:00}", _activeTargets, _totalTargets);

        // Check if all targets are destroyed
        if (_activeTargets == 0)
        {
            _targetCount.text = "No target remaining - press Select to reset";
        }
    }

    private int GetActiveTargetsCount()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        int count = 0;

        foreach (GameObject target in targets)
        {
            Collider collider = target.GetComponent<Collider>();

            if (target.activeSelf && collider != null && collider.enabled)
            {
                count++;
            }
        }

        return count;
    }
}