using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class FirstComeFirstServedPolicy : SchedulingPolicy
    {
        protected readonly Queue<int> _proccessQueue = new();


        public override int NextProcess(Dictionary<int, ProcessTableEntry> dProcessTable)
        {
            int queueSize = _proccessQueue.Count;

            for (int i = 0; i < queueSize; i++)
            {
                int iProcessId = _proccessQueue.Dequeue();
                _proccessQueue.Enqueue(iProcessId);

                ProcessTableEntry entry = dProcessTable[iProcessId];

                if (!entry.Done && !entry.Blocked)
                {
                    return iProcessId;
                }
            }

            return -1;
        }

        public override void AddProcess(int currentProcessId)
        {
            _proccessQueue.Enqueue(currentProcessId);
        }

        public override bool RescheduleAfterInterrupt()
        {
            return false;
        }
    }
}
