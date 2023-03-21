using System.Collections;
using System.Collections.Generic;
using UI.ButtonControllers;
using UnityEngine;

public class ReturnToPreviousButtonController : AButtonController
{
    public override void OnClick()
    {
        _canvasController.ReturnToPrevious();
    }
}
