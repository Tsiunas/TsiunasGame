public class LerpUIElementTsiuna : LerpUIElement {

    private UIDragBehaviourTsiunas compTsiuna;
    public override void Start()
    {
        base.Start();
        compTsiuna = GetComponent<UIDragBehaviourTsiunas>();
        if (compTsiuna != null)
            compTsiuna.enabled = false;

        OnEndLerp += EndLerp;
    }

    private void OnDestroy()
    {
        OnEndLerp -= EndLerp;
    }

    void EndLerp()
    {
        NotificationCenter.DefaultCenter().PostNotification(this, "AddTsiuna", this.gameObject);
    }
}
