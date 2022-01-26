using System.Collections.Concurrent;

namespace Signalr.Log.Brower.Services
{
    public class BrowerLogServices : IBrowerLogServices
    {
        //线程安全的队列
        private ConcurrentQueue<string> queue;
        //当前连接的用户总数
        private int CollectionCount = 0;
        public BrowerLogServices()
        {
            queue = new ConcurrentQueue<string>();
        }
        //入列
        public void Enqueue(string message)
        {
            if (IsNeedEnqueue())
            {
                queue.Enqueue(message);
            }
        }
        //出列
        public bool Dequeue(out string result)
        {
            return queue.TryDequeue(out result);
        }
        public int GetCount()
        {
            return queue.Count;
        }
        public void Online()
        {
            CollectionCount++;
        }
        public void Offline()
        {
            CollectionCount--;
            ClearQueueIfNoUser();
        }

        private void ClearQueueIfNoUser()
        {
            //如果当前已经没有链接的用户了将队列所有的数据移除
            if (CollectionCount == 0)
            {
                queue.Clear();
            }
        }

        /// <summary>
        /// 如果总线用户数大于0，则日志入队列
        /// </summary>
        /// <returns></returns>
        public bool IsNeedEnqueue()
        {
            return CollectionCount > 0;
        }

    }
}
