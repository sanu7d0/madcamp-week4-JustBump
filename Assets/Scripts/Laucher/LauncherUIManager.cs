using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class LauncherUIManager : Singleton<LauncherUIManager>
{
    [SerializeField] Texture2D cursorTexture;

    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject progressLabel;

    [SerializeField] private Image backlights;
    [SerializeField] private Image backlights_far;
    [SerializeField] private float backlightRotSpeed;
    [SerializeField] RectTransform imageGroup;

    public UnityEvent onConnectButtonClickedListener;

    protected override void Awake()
    {
        base.Awake();

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        OnControlPanel();
    }

    void Start() {
        imageGroup.localScale = Vector3.one * 0.3f;
    }

    void Update() {
        if (imageGroup.localScale.magnitude <= 1f) {
            imageGroup.localScale *= 2f * Time.deltaTime;
        }

        backlights.transform.Rotate(Vector3.forward, backlightRotSpeed * Time.deltaTime);
        backlights_far.transform.Rotate(Vector3.forward, -backlightRotSpeed * 0.3f * Time.deltaTime);
    }

    public void OnControlPanel() { 
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void OnProgressLabel() {
        controlPanel.SetActive(false);
        progressLabel.SetActive(true);
    }


    public void OnConnnectButtonClicked() {
        OnProgressLabel();
        onConnectButtonClickedListener.Invoke();
    }
}
