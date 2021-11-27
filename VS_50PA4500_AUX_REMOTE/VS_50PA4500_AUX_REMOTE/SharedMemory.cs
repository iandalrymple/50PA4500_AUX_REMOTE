using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_50PA4500_AUX_REMOTE
{
    public class SharedMemory
    {
        // Privates
        private Queue<UInt32> buttonQueue =  new Queue<UInt32>();
        private readonly object listLock = new object();

        // Constructor
        public SharedMemory()
        {
            // Nothing to do
        }

        public void insertButtonValue(UInt32 inValue)
        {
            lock(listLock)
            {
                buttonQueue.Enqueue(inValue);
            }
        }

        public UInt32 getLastValue()
        {
            lock(listLock)
            {
                return buttonQueue.Dequeue();
            }
        }

        public int getCountQueue()
        {
            return buttonQueue.Count();
        }
    }
}
