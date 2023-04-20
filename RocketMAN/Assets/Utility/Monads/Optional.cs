#nullable enable
using System;

namespace Utility
{
    public class Optional<T> where T : class
    {
        private readonly T? _wrapped;
        private readonly bool _hasValue;

        public Optional(T? elm)
        {
            if (elm is null)
            {
                _hasValue = false;
                return;
            }

            _wrapped = elm;
            _hasValue = true;
        }

        public static Optional<T> Empty()
        {
            return new Optional<T>(null);
        }

        public static Optional<T> From(T elm)
        {
            return new Optional<T>(elm);
        }

        public T GetOrThrow(Func<Exception> exceptionFactory)
        {
            return _hasValue ? _wrapped! : throw exceptionFactory();
        }

        public T GetOrElseGet<TReturn>(Func<TReturn> returnFactory) where TReturn : T
        {
            return _hasValue ? _wrapped! : returnFactory();
        }

        public T UnsafeGet()
        {
            return _hasValue ? _wrapped! : null;
        }

        public Optional<T> Bind(Func<T, T> bind)
        {
            if (!_hasValue)
            {
                return Empty();
            }

            T res = bind(_wrapped!);
            return From(res);
        }
    }
}