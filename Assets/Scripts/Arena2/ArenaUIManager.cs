using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArenaUIManager : Singleton<ArenaUIManager>
{
    public UnityEvent onLeaveButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void OnClickLeaveButton() {
        onLeaveButton.Invoke();
    }
}
