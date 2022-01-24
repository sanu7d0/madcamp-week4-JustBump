using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LauncherUIManager : Singleton<LauncherUIManager>
{
    [SerializeField] Texture2D cursorTexture;

    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject progressLabel;

    public UnityEvent onConnectButtonClickedListener;

    protected override void Awake()
    {
        base.Awake();

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
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
