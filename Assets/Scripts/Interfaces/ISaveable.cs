using System;
using R3;

namespace Interfaces
{
    public interface ISaveable : IDisposable
    {
        string SaveKey { get; }
        string ToJson();
        void FromJson(string json);
        Observable<Unit> OnChanged { get; }
    }
}