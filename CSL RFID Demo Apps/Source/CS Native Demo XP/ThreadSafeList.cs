/*
Copyright (c) 2023 Convergence Systems Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

using CSLibrary.Constants;
using CSLibrary.Structures;

namespace CS203_CALLBACK_API_DEMO
{
    public enum SortIndex
    {
        INDEX,
        PC,
        EPC,
        RSSI,
        COUNT
    }

    class ThreadSafeList
    {
        public SortIndex SortMethod = SortIndex.EPC;
        public bool Ascending = true;

        private List<TagCallbackInfo> myLocalList = new List<TagCallbackInfo>();
        private object myLock = new object();

        public void Insert(TagCallbackInfo newElement)
        {

            lock (myLock)
            {
                TagCallbackInfo info = myLocalList.Find(delegate(TagCallbackInfo found) { return (found.epc.CompareTo(newElement.epc) == 0); });
                if (info != null)
                {
                    myLocalList[info.index].rssi = newElement.rssi;
                }
                else
                {
                    //Finally do the insert by delegating to "InsertAt"
                    newElement.index = myLocalList.Count;
                    myLocalList.Add(newElement);
                }
            }
        }

        private int CompareFunction(TagCallbackInfo element1, TagCallbackInfo element2)
        {
            return element2.epc.CompareTo(element1.epc);
        }

        public List<TagCallbackInfo> Clone()
        {
            lock (myLock)
            {
                return myLocalList;
            }
        }

        public List<TagCallbackInfo> Items
        {
            get { lock (myLock) return myLocalList; }
            set { lock (myLock) myLocalList = value; }
        }

        public List<TagCallbackInfo> GetSortedList()
        {
            List<TagCallbackInfo> tmpList = Items;

            switch (SortMethod)
            {
                case SortIndex.EPC:
                    tmpList.Sort(new LvEpcSorter(Ascending));
                    break;
                case SortIndex.COUNT:
                    tmpList.Sort(new LvCntSorter(Ascending));
                    break;
                case SortIndex.INDEX:
                    tmpList.Sort(new LvIndexSorter(Ascending));
                    break;
                case SortIndex.RSSI:
                    tmpList.Sort(new LvRssiSorter(Ascending));
                    break;
            }
            return tmpList;
        }

        private class LvEpcSorter : IComparer<TagCallbackInfo>
        {
            private bool Ascending = false;
            public LvEpcSorter(bool ascending)
            {
                Ascending = ascending;
            }

            public int Compare(TagCallbackInfo obj1, TagCallbackInfo obj2)
            {
                int nResult = obj2.epc.CompareTo(obj1.epc);
                if (nResult != 0)
                {
                    nResult = Ascending ? nResult : -nResult;
                }
                return nResult;
            }
        }
        private class LvCntSorter : IComparer<TagCallbackInfo>
        {
            private bool Ascending = false;
            public LvCntSorter(bool ascending)
            {
                Ascending = ascending;
            }
            public int Compare(TagCallbackInfo obj1, TagCallbackInfo obj2)
            {
                int nResult = ((TagCallbackInfo)obj2).count.CompareTo(((TagCallbackInfo)obj1).count);
                if (nResult != 0)
                {
                    nResult = Ascending ? -nResult : nResult;
                }
                return nResult;
            }
        }
        private class LvRssiSorter : IComparer<TagCallbackInfo>
        {
            private bool Ascending = false;
            public LvRssiSorter(bool ascending)
            {
                Ascending = ascending;
            }
            public int Compare(TagCallbackInfo obj1, TagCallbackInfo obj2)
            {
                int nResult = ((TagCallbackInfo)obj2).rssi.CompareTo(((TagCallbackInfo)obj1).rssi);
                if (nResult != 0)
                {
                    nResult = Ascending ? -nResult : nResult;
                }
                return nResult;
            }
        }
        private class LvIndexSorter : IComparer<TagCallbackInfo>
        {
            private bool Ascending = false;
            public LvIndexSorter(bool ascending)
            {
                Ascending = ascending;
            }
            public int Compare(TagCallbackInfo obj1, TagCallbackInfo obj2)
            {
                int nResult = ((TagCallbackInfo)obj2).index.CompareTo(((TagCallbackInfo)obj1).index);
                if (nResult != 0)
                {
                    nResult = Ascending ? -nResult : nResult;
                }
                return nResult;
            }
        }
    }
}
