using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class BackTrackReader : StreamReader
    {
        RingBuffer<int> backRingBuffer;

        public BackTrackReader(Stream reader, int backSize) : base(reader)
        {
            backRingBuffer = new RingBuffer<int>(backSize);
        }

        private int backCnt = 0;
        public override int Read()
        {
            int readChar;
            if (backCnt == 0)
            {
                readChar = base.Read();
                backRingBuffer.AddItem(readChar);
            }
            else
            {
                readChar = backRingBuffer.GetPreviousItem(-backCnt);
                backCnt--;
            }
            return readChar;
        }

        public void ReturnToPreviousChar(int backTrackCharCount)
        {
            backCnt = backTrackCharCount;
        }


        private class RingBuffer<T>
        {
            private T[] buffer;
            private int currentInsertPosition;
            public int BufferSize;

            public RingBuffer(int ringBufferSize)
            {
                buffer = new T[ringBufferSize];
                BufferSize = ringBufferSize;
                currentInsertPosition = 0;
            }
            
            public void AddItem(T item)
            {
                if(currentInsertPosition < BufferSize - 1)
                {
                    buffer[currentInsertPosition] = item;
                }
                else
                {
                    currentInsertPosition = 0;
                    buffer[currentInsertPosition] = item;
                }
                currentInsertPosition++;
            }

            //current = 0, previous < 0
            public T GetPreviousItem(int minusPos)
            {
                if(-minusPos > (BufferSize - 1))
                {
                    System.Diagnostics.Debug.WriteLine("Buffer Size Backtrack exceeded - wrong element was aquired");
                }
                minusPos = minusPos % (BufferSize - 1);
                int toGetPos = currentInsertPosition + minusPos;

                if(toGetPos < 0)
                {
                    toGetPos = (BufferSize - 1) + toGetPos;
                }

                return buffer[toGetPos];
            }

        }

    }
}
