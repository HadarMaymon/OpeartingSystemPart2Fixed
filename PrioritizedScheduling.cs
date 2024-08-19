using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class PrioritizedScheduling :SchedulingPolicy
    {
        private int quantum;
        private Dictionary<int, Queue<int>> processQ;
        private SortedSet<int> prior;

        public PrioritizedScheduling(int iQuantum)
        {
            this.quantum = iQuantum;
            this.processQ = new Dictionary<int, Queue<int>>();
            this.prior = new SortedSet<int>();
        }

        public override int NextProcess(Dictionary<int, ProcessTableEntry> dProcessTable)
        {
            foreach (var processEntry in dProcessTable)
            {
                int processId = processEntry.Key;
                var process = processEntry.Value;

                if (!process.Done && !process.Blocked)
                {
                    if (!processQ.TryGetValue(process.Priority, out var queue))
                    {
                        queue = new Queue<int>();
                        processQ[process.Priority] = queue;
                        prior.Add(process.Priority);
                    }

                    if (!queue.Contains(processId))
                    {
                        queue.Enqueue(processId);
                    }
                }
            }

            foreach (int priority in prior.Reverse())
            {
                if (processQ.TryGetValue(priority, out var queue) && queue.Count > 0)
                {
                    int pid = queue.Dequeue();

                    var selectedProcess = dProcessTable[pid];
                    if (!selectedProcess.Done && !selectedProcess.Blocked)
                    {
                        selectedProcess.Quantum = quantum;
                        queue.Enqueue(pid);
                        return pid;
                    }
                }
            }

            return -1;


        }

        public override void AddProcess(int iProcessId)
        {
        }

        public override bool RescheduleAfterInterrupt()
        {
            return true;
        }
    }
}
