using System.Collections;
using UnityEngine;

public interface IManager
{
    public IEnumerator Initialize(GameContext ctx);
    public void Inject(GameContext ctx);
}
