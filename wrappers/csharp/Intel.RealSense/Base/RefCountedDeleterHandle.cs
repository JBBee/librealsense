// License: Apache 2.0. See LICENSE file in root directory.
// Copyright(c) 2017 Intel Corporation. All Rights Reserved.

namespace Intel.RealSense.Base
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Security;


    /// <summary>
    /// Native handle with deleter delegate to release unmanaged resources
    /// The unmanaged resource is released when the refcount reaches 0
    /// </summary>
    // TODO: CriticalFinalizerObject & CER
    //[DebuggerDisplay("{deleter?.Method.Name,nq}", Name = nameof(Deleter))]
    internal class RefCountedDeleterHandle : DeleterHandle
    {
        private int refCount;

        public RefCountedDeleterHandle(IntPtr ptr, Deleter deleter) : base(ptr, deleter)
        {
            Retain();
        }

        public RefCountedDeleterHandle() : base(IntPtr.Zero, null)
        {
            SetHandleAsInvalid();
        }

        public void Retain()
        {
            refCount++;
        }

        protected override void Dispose(bool disposing)
        {
            if (Handle == IntPtr.Zero)
            {
                return;
            }

            refCount--;
            if (refCount == 0)
            {
                base.Dispose(disposing);
            }
        }

        ~RefCountedDeleterHandle()
        {
            Dispose(false);
        }
    }
}