using System;
using Tsiunas.Mechanics;

internal interface IHungerObserved
{
    event Action<HungerManager.HungerStates> OnHungerStateChange;
}