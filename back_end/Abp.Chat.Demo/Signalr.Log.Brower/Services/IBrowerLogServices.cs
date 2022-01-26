using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signalr.Log.Brower.Services
{
    public interface IBrowerLogServices
    {
        void Enqueue(string message);
        //出列
        bool Dequeue(out string result);
        int GetCount();
        void Online();
        void Offline();
    }
}
