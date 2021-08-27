
using System;

namespace Infinite_story
{
    public interface ITestInterface<T>
    {
        event Action<T> MyTestEvent;
    }
}
