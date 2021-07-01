// Copyright (c) Microsoft Corporation and contributors.  All Rights Reserved.  See License.txt in the project root for license information.
using System;
using System.Runtime.InteropServices;
using TorchSharp.Tensor;

namespace TorchSharp
{
    using impl;

    namespace impl
    {
        /// <summary>
        /// This class is used to represent a ELU module.
        /// </summary>
        public class ELU : torch.nn.Module
        {
            internal ELU(IntPtr handle, IntPtr boxedHandle) : base(handle, boxedHandle) { }

            [DllImport("LibTorchSharp")]
            private static extern IntPtr THSNN_ELU_forward(torch.nn.Module.HType module, IntPtr tensor);

            public override TorchTensor forward(TorchTensor tensor)
            {
                var res = THSNN_ELU_forward(handle, tensor.Handle);
                if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                return new TorchTensor(res);
            }

            public override string GetName()
            {
                return typeof(ELU).Name;
            }
        }
    }

    public static partial class torch
    {
        public static partial class nn
        {
            [DllImport("LibTorchSharp")]
            extern static IntPtr THSNN_ELU_ctor(double alpha, bool inplace, out IntPtr pBoxedModule);

            /// <summary>
            /// Exponential Linear Unit
            /// </summary>
            /// <param name="alpha">The α value for the ELU formulation. Default: 1.0</param>
            /// <param name="inPlace">Do the operation in-place. Default: False</param>
            /// <returns></returns>
            static public ELU ELU(double alpha = 1.0, bool inPlace = false)
            {
                var handle = THSNN_ELU_ctor(alpha, inPlace, out var boxedHandle);
                if (handle == IntPtr.Zero) { torch.CheckForErrors(); }
                return new ELU(handle, boxedHandle);
            }
        }
        public static partial class functional
        {
            /// <summary>
            /// Exponential Linear Unit
            /// </summary>
            /// <param name="x">The input tensor</param>
            /// <param name="alpha">The α value for the ELU formulation. Default: 1.0</param>
            /// <param name="inPlace">Do the operation in-place. Default: False</param>
            /// <returns></returns>
            static public TorchTensor ELU(TorchTensor x, double alpha, bool inPlace = false)
            {
                using (var m = nn.ELU(alpha, inPlace)) {
                    return m.forward(x);
                }
            }
        }
    }
}
