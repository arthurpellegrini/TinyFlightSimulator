using System.Collections;
using UnityEngine;

namespace Common
{
    public abstract class Manager<T> : SingletonGameStateObserver<T> where T : Component
    {
        protected bool m_IsReady;
        public bool IsReady => m_IsReady;

        protected virtual IEnumerator Start()
        {
            m_IsReady = false;
            yield return StartCoroutine(InitCoroutine());
            m_IsReady = true;
        }

        protected abstract IEnumerator InitCoroutine();
    }
}