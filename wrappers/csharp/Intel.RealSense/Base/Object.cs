﻿// License: Apache 2.0. See LICENSE file in root directory.
// Copyright(c) 2017 Intel Corporation. All Rights Reserved.

namespace Intel.RealSense.Base
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Base class for disposable objects with native resources
    /// </summary>
    public abstract class Object : IDisposable
    {
        // TODO: rename, kept for backwards compatiblity
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal RefCountedDeleterHandle m_instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="Object"/> class.
        /// </summary>
        /// <param name="ptr">native pointer</param>
        /// <param name="deleter">optional deleter</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="ptr"/> is null</exception>
        protected Object(IntPtr ptr, Deleter deleter)
        {
            if (ptr == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(ptr));
            }

            m_instance = new RefCountedDeleterHandle(ptr, deleter);
        }

        protected Object(Object other)
        {
            if (other.m_instance == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            m_instance = other.m_instance;
            m_instance.Retain();
        }

        /// <summary>
        /// Gets the native handle
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when <see cref="DeleterHandle.IsInvalid"/></exception>
        public IntPtr Handle
        {
            get
            {
                if (m_instance.IsInvalid)
                {
                    throw new ObjectDisposedException(GetType().Name);
                }

                return m_instance.Handle;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            m_instance.Dispose();

            //Reset the instance ref to an invalid handle
            m_instance = new RefCountedDeleterHandle();
        }

        internal void Reset(IntPtr ptr, Deleter deleter)
        {
            m_instance.Reset(ptr, deleter);
        }
    }
}
