using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorCoreTerminal : TerminalTrigger
{
    Sound mainSong;
    public override void SomeCoolAction()
    {
        mainSong.source.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.GetSound("MainLoop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
