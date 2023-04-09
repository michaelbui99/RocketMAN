using UnityEngine;

namespace Utility
{
    public class ValueTypeReference<T>
    {
        private readonly T _ref;

        public ValueTypeReference(T refence)
        {
            _ref = refence;
        }

        public T Dereference()
        {
            return _ref;
        }
    }
}