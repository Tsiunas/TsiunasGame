using UnityEngine;
using System.Collections;

public static class DebugGlobals
{
    public static bool canDebug = true;
}


public abstract class DebugScript : MonoBehaviour
{
    protected string textoBoton;
    
    protected KeyCode k1;
    protected KeyCode k2;
    // Use this for initialization
    private void OnEnable()
    {
        if (DebugGlobals.canDebug && Application.isEditor)
            ConfigurarOnEnable();
        else
            Destroy(this.gameObject);
        
    }

    internal virtual void ConfigurarOnEnable() { }

    void Start()
    {
        if (DebugGlobals.canDebug && Application.isEditor)
            ConfigurarInicio();
        else
            Destroy(this.gameObject);
    }

    internal virtual void ConfigurarInicio() { }

    // Update is called once per frame
    protected void Update()
    {
        if (!DebugGlobals.canDebug || !Application.isEditor)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoOnSpace();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DoOnEnter();
        }
        if (Input.GetKeyDown(k1))
        {
            DoOnK1();
        }
        if (Input.GetKeyDown(k2))
        {
            DoOnK2();
        }




    }

    private void OnGUI()
    {

        if(!string.IsNullOrEmpty(textoBoton))
            if (GUILayout.Button(textoBoton))
            {
                DoOnButton();
            }


    }

    protected abstract void DoOnButton();
    protected abstract void DoOnEnter();
    protected abstract void DoOnSpace();
    protected abstract void DoOnK1();
    protected abstract void DoOnK2();

}
