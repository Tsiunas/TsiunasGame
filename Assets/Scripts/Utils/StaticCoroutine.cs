using System.Collections;

public class StaticCoroutine : Singleton<StaticCoroutine>
{
    public StaticCoroutine()
    {
        base.uniqueToAllApp = true;
    }

    IEnumerator Perform(IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);
    }

    /// <summary>
    /// Place your lovely static IEnumerator in here and witness magic!
    /// </summary>
    /// <param name="coroutine">Static IEnumerator</param>
    public static void DoCoroutine(IEnumerator coroutine)
    {
        Instance.StartCoroutine(Instance.Perform(coroutine)); //this will launch the coroutine on our instance
    }
}