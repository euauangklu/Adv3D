using System;
using UnityEngine.Events;

namespace GDD
{
    public interface IBackButton
    {
        public void AddEvent(UnityAction action);
    }
}