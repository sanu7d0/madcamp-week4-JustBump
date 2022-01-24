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
    [SerializeField] private float backlightRotSpeed;

    public UnityEvent onConnectButtonClickedListener;

    protected override void Awake()
    {
        base.Awake();

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    void Update() {
        backlights.transform.Rotate(Vector3.forward, backlightRotSpeed * Time.deltaTime);
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
