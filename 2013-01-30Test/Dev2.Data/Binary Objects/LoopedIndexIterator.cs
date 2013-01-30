﻿using System;

namespace Dev2.Data.Binary_Objects
{
    public class LoopedIndexIterator : IIndexIterator
    {
        private readonly int _loopedIdx;
        private int _curPos = 0;
        private readonly int _itrCnt;

        public int Count
        {
            get { return _itrCnt;  }
        }

        internal LoopedIndexIterator(int val, int itrCnt)
        {
            _loopedIdx = val;
            _itrCnt = itrCnt;
        }

        public bool HasMore()
        {
            return (_curPos < _itrCnt);
        }

        public int FetchNextIndex()
        {
            _curPos++;
            return _loopedIdx;
        }

        public int MaxIndex()
        {
            return _loopedIdx;
        }

        public int MinIndex()
        {
            return _loopedIdx;
        }

        public void AddGap(int idx)
        {
            throw new NotImplementedException();
        }

        public void RemoveGap(int idx)
        {
            throw new NotImplementedException();
        }
        
        public IIndexIterator Clone()
        {
            return new LoopedIndexIterator(_curPos, _itrCnt);
        }

    }
}
