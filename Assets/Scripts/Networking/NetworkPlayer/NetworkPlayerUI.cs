using UnityEngine;
using TMPro;

public class NetworkPlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promtText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateText(string promptMessage)
    {
        promtText.text = promptMessage;
    }
}
