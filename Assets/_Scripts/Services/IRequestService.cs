using System;
using System.Collections;

namespace HomeReval.Services
{
    interface IRequestService
    {
        IEnumerator Get(string path, /*string json,*/ Action<string> success, Action<string> error);
        IEnumerator Post(string path, string json, Action<string> success, Action<string> error);
    }
}
