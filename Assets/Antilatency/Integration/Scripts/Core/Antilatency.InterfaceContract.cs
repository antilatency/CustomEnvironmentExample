// Copyright (c) 2019 ALT LLC
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of source code located below and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

//Unity & Mono fix
#if ENABLE_IL2CPP && !__MonoCS__
    #define __MonoCS__
#endif

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Reflection;

#if __MonoCS__
using AOT;
#endif

namespace Antilatency {
    public static class Utils {
        public static void SafeDispose<T>(ref T disposable) where T : class, IDisposable {
            T target = disposable;
            if (target != null) {
                disposable = null;
                (target as IDisposable).Dispose();
            }
        }

        public static IntPtr DetachInterfacePointer<T>(T antilatencyInterface) where T : Antilatency.InterfaceContract.IUnsafe {
            if (antilatencyInterface == null) {
                return IntPtr.Zero;
            }

            return (antilatencyInterface as Antilatency.InterfaceContract.Details.IUnsafeWrapper).Detach();
        }
    }
}

namespace Antilatency.InterfaceContract {
    
    public enum ExceptionCode : uint {
        Ok = 0,
        NotImplemented = 0x80004001,
        NoInterface = 0x80004002,
        ErrorPointer = 0x80004003,
        UnknownFailure = 0x80004005,
        InvalidHandle = 0x80070006,
        InvalidArg = 0x80070057,
        OutOfMemory = 0x8007000E,
        Pending = 0x8000000A,
        AsyncOperationNotStarted = 0x80000019,
        RpcTimeout = 0x8001011F,
        RpcDisconnected = 0x80010108
    };

    public class Exception : System.Exception {
        public Exception() {
            _code = ExceptionCode.UnknownFailure;
        }
        public Exception(ExceptionCode code) {
            _code = code;
        }
        public Exception(ExceptionCode code, string message) : base(message) {
            _code = code;
        }
        public Exception(string message) : base(message) {
            _code = ExceptionCode.UnknownFailure;
        }
        public ExceptionCode code {
            get {
                return _code;
            }
        }

        private ExceptionCode _code;
    };

    [System.Serializable]
    public struct Bool {
        public int internalValue;

        public Bool(bool value) {
            internalValue = value ? 1 : 0;
        }

        public bool Value {
            get { return internalValue != 0; }
            set { internalValue = value ? 1 : 0; }
        }

        public override string ToString() {
            return internalValue != 0 ? bool.TrueString : bool.FalseString;
        }

        public override int GetHashCode() {
            return internalValue != 0 ? 1 : 0;
        }

        public override bool Equals(object obj) {
            if (!(obj is Bool)) {
                return false;
            }
                
            var value = (Bool)obj;

            return value.Value == Value;
        }

        public static bool operator ==(Bool a, Bool b) {
            return a.Value == b.Value;
        }

        public static bool operator !=(Bool a, Bool b) {
            return a.Value != b.Value;
        }

        public static implicit operator Bool(bool value) {
            return new Bool(value);
        }

        public static implicit operator bool(Bool value) {
            return value.Value;
        }
    }

    [Guid("ffffffff-ffff-ffff-0C00-000000000064")]
    public interface IUnsafe : IDisposable, IEquatable<object> {
        T QueryInterface<T>() where T : class, IUnsafe;
        ExceptionCode QueryInterface(ref Guid guid, out IntPtr result);
    }

    [Guid("00000000-0000-0000-C000-000000000046")]
    public interface IInterface : IUnsafe {
        uint AddRef();
        uint Release();
    }

    namespace Details {
        public class IUnsafeWrapper : IUnsafe {

            private IUnsafeRemap.VMT _vmt;

            protected IntPtr _object;

            public virtual void Dispose() {
                GC.SuppressFinalize(this);
            }

            public IntPtr Detach() {
                var tmp = _object;
                _object = IntPtr.Zero;
                return tmp;
            }
			
			public void Attach(IntPtr vmt) {
                if(_object != IntPtr.Zero) {
                    throw new System.Exception("Cannot attach pointer to wrapper in not-detached state");
                }
                _object = vmt;
            }

            public static implicit operator bool(IUnsafeWrapper obj) {
                return !object.ReferenceEquals(obj, null) && obj._object != IntPtr.Zero;
            }

            public static bool operator ==(IUnsafeWrapper left, IUnsafeWrapper right) {
                bool leftNull = object.ReferenceEquals(left, null) || (left._object == IntPtr.Zero);
                bool rightNull = object.ReferenceEquals(right, null) || (right._object == IntPtr.Zero);

                if (leftNull) {
                    return rightNull;
                }

                return !rightNull && (right._object == left._object);
            }

            public static bool operator !=(IUnsafeWrapper left, IUnsafeWrapper right) {
                return !(left == right);
            }

            public IUnsafeWrapper() : base() { }

            public IUnsafeWrapper(IntPtr obj) {
                _object = obj;
                _vmt = LoadVMT<IUnsafeRemap.VMT>(0);
            }

            public IntPtr NativePointer {
                get { return _object; }
            }

            public T QueryInterface<T>() where T : class, IUnsafe {
                var wrapperType = Details.Utils.GetWrapperType<T>();
                if (wrapperType == null) {
                    return null;
                }

                Guid guid = typeof(T).GUID;
                IntPtr ptr = IntPtr.Zero;
                QueryInterface(ref guid, out ptr);
                if (ptr == IntPtr.Zero) {
                    return null;
                }

                return (T)Activator.CreateInstance(wrapperType, ptr);
            }

            public ExceptionCode QueryInterface(ref Guid guid, out IntPtr result) {
                return _vmt.QueryInterface(_object, ref guid, out result);
            }

            public bool Valid {
                get { return _object != IntPtr.Zero; }
            }

            protected int GetTotalNativeMethodsCount() {
                return 1;
            }

            protected T LoadVMT<T>(int firstMethodIndex) where T : new() {
                if (_object != IntPtr.Zero) {
                    IntPtr vtbl = Marshal.ReadIntPtr(_object);
                    vtbl = new IntPtr(vtbl.ToInt64() + firstMethodIndex * IntPtr.Size);
                    return (T)Marshal.PtrToStructure(vtbl, typeof(T));
                } else {
                    return new T();
                }
            }

            public override bool Equals(object obj) {
                if (obj == null || !(obj is IUnsafeWrapper)) {
                    return false;
                }
                return (obj as IUnsafeWrapper)._object == _object;
            }

            public override int GetHashCode() {
                return _object.GetHashCode();
            }

            protected void HandleExceptionCode(ExceptionCode code) {
                if (code != ExceptionCode.Ok) {
                    var exceptionData = QueryInterface<Antilatency.InterfaceContract.IExceptionData>();
                    throw new Antilatency.InterfaceContract.Exception(code, exceptionData.getMessage());
                }
            }
        }


        public class IUnsafeRemap : InterfacedObject.LifetimeControlAccess, IDisposable {
            public class NativeInterfaceVmt : IDisposable {

                private List<object> _managedVmtBlocks;

                private IntPtr _handle;
                public IntPtr Handle {
                    get { return _handle; }
                }
                public NativeInterfaceVmt(List<object> vmtBlocks) {
                    _managedVmtBlocks = vmtBlocks;

                    var fullSize = _managedVmtBlocks.Select(v => Marshal.SizeOf(v)).Sum();
                    _handle = Marshal.AllocHGlobal(fullSize);

                    var writeHandle = _handle;
                    foreach (object obj in _managedVmtBlocks) {
                        Marshal.StructureToPtr(obj, writeHandle, false);
                        Utils.IncrementPointer(ref writeHandle, Marshal.SizeOf(obj));
                    }
                }
                public void Dispose() {
                    GC.SuppressFinalize(this);
                    FreeMemory();
                }

                ~NativeInterfaceVmt() {
                    FreeMemory();
                }
                private void FreeMemory() {
                    var tmp = _handle;
                    if (tmp != IntPtr.Zero) {
                        _handle = IntPtr.Zero;
                        Marshal.FreeHGlobal(tmp);
                    }
                }
            }

            public struct VMT {
                public delegate ExceptionCode QueryInterfaceDelegate(IntPtr _this, ref Guid guid, out IntPtr result);
#pragma warning disable 0649
                public QueryInterfaceDelegate QueryInterface;
#pragma warning restore 0649
            }

            private IntPtr _handle;
            public IntPtr Handle {
                get { return _handle; }
            }

            private struct Data {
                public IntPtr Vmt;
                public IntPtr Context;
                public IntPtr LifetimeId;
            }

            public static object GetContext(IntPtr remap) {
                var contextPtr = Marshal.ReadIntPtr(Utils.IncrementPointer(remap, IntPtr.Size));
                return GCHandle.FromIntPtr(contextPtr).Target;
            }

            public static int GetLifetimeID(IntPtr remap) {
                var contextPtr = Marshal.ReadIntPtr(Utils.IncrementPointer(remap, IntPtr.Size * 2));
                return contextPtr.ToInt32();
            }

            public static void SetLifetimeID(IntPtr nativeObject, ushort id) {
                Marshal.WriteIntPtr(Utils.IncrementPointer(nativeObject, IntPtr.Size * 2), new IntPtr((int)id));
            }

            public void Dispose() {
                FreeMemroy();
                GC.SuppressFinalize(this);
            }

            private void FreeMemroy() {
                var tmp = _handle;
                if (tmp != IntPtr.Zero) {
                    _handle = IntPtr.Zero;
                    Marshal.FreeHGlobal(tmp);
                }
            }

            public readonly static NativeInterfaceVmt NativeVmt;

            public IUnsafeRemap() { }

            static IUnsafeRemap() {
                var vmtBlocks = new List<object>();
                AppendVmt(vmtBlocks);
                NativeVmt = new NativeInterfaceVmt(vmtBlocks);
            }

            protected static void AppendVmt(List<object> buffer) {
                var vmt = new VMT();
                vmt.QueryInterface = (IntPtr _this, ref Guid guid, out IntPtr result) => {
                    var obj = Antilatency.InterfaceContract.Details.IInterfaceRemap.GetContext(_this) as InterfacedObject;
                    var lifetimeId = Antilatency.InterfaceContract.Details.IInterfaceRemap.GetLifetimeID(_this);
                    return Antilatency.InterfaceContract.InterfacedObject.LifetimeControlAccess.QueryLifetimeInterfaceByArrayId(obj, (ushort)lifetimeId, ref guid, out result);
                };
                buffer.Add(vmt);
            }

            protected void AllocateNativeInterface(System.IntPtr vmt, System.IntPtr context, ushort lifetimeId) {
                _handle = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Data)));
                var data = new Data() { Vmt = vmt, Context = context, LifetimeId = new IntPtr(lifetimeId) };
                Marshal.StructureToPtr(data, _handle, false);
            }

            public IUnsafeRemap(System.IntPtr context, ushort lifetimeId) {
                AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
            }

            protected static ExceptionCode handleRemapException(System.Exception ex, IntPtr objPtr) {
                var obj = Antilatency.InterfaceContract.Details.IInterfaceRemap.GetContext(objPtr) as InterfacedObject;
                var data = obj.QueryInterface<Antilatency.InterfaceContract.IExceptionData>();

                data.setMessage(ex.Message);

                if (ex is Antilatency.InterfaceContract.Exception) {
                    return (ex as Antilatency.InterfaceContract.Exception).code;
                } else if (ex is System.OutOfMemoryException) {
                    return ExceptionCode.OutOfMemory;
                } else if (ex is System.NotImplementedException) {
                    return ExceptionCode.NotImplemented;
                } else {
                    return ExceptionCode.UnknownFailure;
                }
            }
        }

        public class IInterfaceWrapper : IUnsafeWrapper, IInterface {


            private IInterfaceRemap.VMT _VMT = new IInterfaceRemap.VMT();
            protected new int GetTotalNativeMethodsCount() {
                return base.GetTotalNativeMethodsCount() + typeof(IInterfaceRemap.VMT).GetFields().Length;
            }

            public IInterfaceWrapper() : base() {

            }

            public IInterfaceWrapper(System.IntPtr obj) : base(obj) {
                _VMT = LoadVMT<IInterfaceRemap.VMT>(base.GetTotalNativeMethodsCount());
            }

            public IntPtr NewNativePointer {
                get {
                    if (_object != IntPtr.Zero) {
                        AddRef();
                    }
                    return _object;
                }
            }

            ~IInterfaceWrapper() {
                ReleaseObject(true);
            }

            private void ReleaseObject(bool fromFinalizer) {
                var tmp = _object;
                if (tmp != IntPtr.Zero) {
                    _object = IntPtr.Zero;
                    _VMT.Release(tmp);
                }
            }

            public static implicit operator bool(IInterfaceWrapper obj) {
                return !object.ReferenceEquals(obj, null) && obj._object != IntPtr.Zero;
            }

            public static bool operator ==(IInterfaceWrapper left, IInterfaceWrapper right) {
                bool leftNull = object.ReferenceEquals(left, null) || (left._object == IntPtr.Zero);
                bool rightNull = object.ReferenceEquals(right, null) || (right._object == IntPtr.Zero);

                if (leftNull) {
                    return rightNull;
                }

                return !rightNull && (right._object == left._object);
            }

            public override bool Equals(object obj) {
                if (obj == null || !(obj is IInterfaceWrapper)) {
                    return false;
                }
                return (obj as IInterfaceWrapper)._object == _object;
            }

            public override int GetHashCode() {
                return _object.GetHashCode();
            }


            public static bool operator !=(IInterfaceWrapper left, IInterfaceWrapper right) {
                return !(left == right);
            }

            public uint AddRef() {
                return _VMT.AddRef(_object);
            }

            public uint Release() {
                return _VMT.Release(_object);
            }

            public override void Dispose() {
                ReleaseObject(false);
                base.Dispose();
            }
        }

        public class IInterfaceRemap : IUnsafeRemap {
            public new struct VMT {
                public delegate uint AddRefDelegate(IntPtr _this);
                public delegate uint ReleaseDelegate(IntPtr _this);
#pragma warning disable 0649
                public AddRefDelegate AddRef;
                public ReleaseDelegate Release;
#pragma warning restore 0649
            }
            public new static readonly NativeInterfaceVmt NativeVmt;
            private static VMT _vmt;

            public uint AddRef() {
                return _vmt.AddRef(Handle);
            }

            public uint Release() {
                return _vmt.Release(Handle);
            }

            static IInterfaceRemap() {
                var vmtBlocks = new System.Collections.Generic.List<object>();
                AppendVmt(vmtBlocks);
                NativeVmt = new NativeInterfaceVmt(vmtBlocks);
            }

            protected static new void AppendVmt(List<object> buffer) {
                Antilatency.InterfaceContract.Details.IUnsafeRemap.AppendVmt(buffer);
                _vmt = new VMT();
                _vmt.AddRef = (IntPtr _this) => {
                    var obj = Antilatency.InterfaceContract.Details.IInterfaceRemap.GetContext(_this) as InterfacedObject;
                    var lifetimeId = Antilatency.InterfaceContract.Details.IInterfaceRemap.GetLifetimeID(_this);
                    return AddRefLifetime(obj, lifetimeId);
                };
                _vmt.Release = (IntPtr _this) => {
                    var obj = Antilatency.InterfaceContract.Details.IInterfaceRemap.GetContext(_this) as InterfacedObject;
                    var lifetimeId = Antilatency.InterfaceContract.Details.IInterfaceRemap.GetLifetimeID(_this);
                    return ReleaseLifetime(obj, lifetimeId);
                };
                buffer.Add(_vmt);
            }

            public IInterfaceRemap() { }
            public IInterfaceRemap(System.IntPtr context, ushort lifetimeId) {
                AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
            }
        }
    }

    [Guid("97122ff5-ceaa-4c40-9627-12aab74a5daf")]
    public interface IExceptionData : Antilatency.InterfaceContract.IUnsafe {
        string getMessage();
        void setMessage(string message);
    }
    namespace Details {
        public class IExceptionDataWrapper : Antilatency.InterfaceContract.Details.IUnsafeWrapper, IExceptionData {
            private IExceptionDataRemap.VMT _VMT = new IExceptionDataRemap.VMT();
            protected new int GetTotalNativeMethodsCount() {
                return base.GetTotalNativeMethodsCount() + typeof(IExceptionDataRemap.VMT).GetFields().Length;
            }
            public IExceptionDataWrapper(System.IntPtr obj) : base(obj) {
                _VMT = LoadVMT<IExceptionDataRemap.VMT>(base.GetTotalNativeMethodsCount());
            }

            public void setMessage(string message) {
                var valueMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(message);
                HandleExceptionCode(_VMT.setMessage(_object, valueMarshaler));
                valueMarshaler.Dispose();
            }

            public string getMessage() {
                string result;
                var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create();
                HandleExceptionCode(_VMT.getMessage(_object, resultMarshaler));
                result = resultMarshaler.value;
                resultMarshaler.Dispose();
                return result;
            }
        }
        public class IExceptionDataRemap : Antilatency.InterfaceContract.Details.IUnsafeRemap {
            public new struct VMT {
                public delegate Antilatency.InterfaceContract.ExceptionCode getMessageDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate message);
                public delegate Antilatency.InterfaceContract.ExceptionCode setMessageDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate message);
#pragma warning disable 0649
                public getMessageDelegate getMessage;
                public setMessageDelegate setMessage;
#pragma warning restore 0649
            }
            public new static readonly NativeInterfaceVmt NativeVmt;
            static IExceptionDataRemap() {
                var vmtBlocks = new System.Collections.Generic.List<object>();
                AppendVmt(vmtBlocks);
                NativeVmt = new NativeInterfaceVmt(vmtBlocks);
            }
            protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
                Antilatency.InterfaceContract.Details.IUnsafeRemap.AppendVmt(buffer);
                var vmt = new VMT();
                vmt.getMessage = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate message) => {
                    try {
                        var obj = GetContext(_this) as IExceptionData;
                        message.assign(obj.getMessage());
                    } catch (System.Exception ex) {
                        return handleRemapException(ex, _this);
                    }
                    return Antilatency.InterfaceContract.ExceptionCode.Ok;
                };
                vmt.setMessage = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate message) => {
                    try {
                        var obj = GetContext(_this) as IExceptionData;
                        obj.setMessage(message);
                    } catch (System.Exception ex) {
                        return handleRemapException(ex, _this);
                    }
                    return Antilatency.InterfaceContract.ExceptionCode.Ok;
                };
                buffer.Add(vmt);
            }
            public IExceptionDataRemap() { }
            public IExceptionDataRemap(System.IntPtr context, ushort lifetimeId) {
                AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
            }
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class LifetimeControllerAttribute : System.Attribute {
        public ushort Id;
        public Type Interface;

        public LifetimeControllerAttribute(ushort Id, Type Interface) {
            this.Id = Id;
            this.Interface = Interface;
        }
    }

    public abstract class InterfacedObject : IInterface, IExceptionData {
        
        private static Dictionary<IntPtr, InterfacedObject> _trackedObjects = new Dictionary<IntPtr, InterfacedObject>();

        private class LifetimeController {
            public ushort LifetimeID;
            public int Refcount;
            public Dictionary<Guid, Details.IUnsafeRemap> Interfaces = new Dictionary<Guid, Details.IUnsafeRemap>();
        }

        private LifetimeController[] _lifetimeControllers;

        private static Dictionary<int, uint> _lifetimeCounters = new Dictionary<int, uint>();

        //private static Dictionary<int, InterfacedObject> _trackedObjects = new Dictionary<IntPtr, InterfacedObject>();

        private int _refCount = 0;
        private GCHandle _thisHandle;

        public class LifetimeControlAccess {
            public static uint AddRefLifetime(InterfacedObject obj, int lifetimeId) {
                return obj.AddRefLifetime(lifetimeId);
            }

            public static uint ReleaseLifetime(InterfacedObject obj, int lifetimeId) {
                return obj.ReleaseLifetime(lifetimeId);
            }

            public static ExceptionCode QueryLifetimeInterfaceByArrayId(InterfacedObject obj, ushort lifetimeArrayIndex, ref Guid guid, out IntPtr result) {
                return obj.QueryLifetimeInterfaceByArrayId(lifetimeArrayIndex, ref guid, out result);
            }
        }

        protected virtual void PartialDestroy(int lifetimeId) { }

        private uint AddRefLifetime(int lifetimeArrayId) {
            uint result = AddRef();
            result = (uint)Interlocked.Increment(ref _lifetimeControllers[lifetimeArrayId].Refcount);
            return result;
        }

        private uint ReleaseLifetime(int lifetimeArrayId) {
            uint result = (uint)Interlocked.Decrement(ref _lifetimeControllers[lifetimeArrayId].Refcount);
            if (result == 0) {
                PartialDestroy(_lifetimeControllers[lifetimeArrayId].LifetimeID);
            }
            Release();
            return result;
        }

        public InterfacedObject() {
            _thisHandle = GCHandle.Alloc(this);
            _trackedObjects.Add(GCHandle.ToIntPtr(_thisHandle), this);
            //implement interfaces
            var thisType = this.GetType();
            var interfaces = thisType.GetInterfaces();

            var lifetimeAttributes = thisType.GetCustomAttributes<LifetimeControllerAttribute>();

            List<LifetimeController> controllers = new List<LifetimeController>();
            foreach (var i in interfaces) {
                //Implement interface if remap type exists
                var remapType = Antilatency.InterfaceContract.Details.Utils.GetRemapType(i);
                if (remapType != null) {
                   

                    //Find lifetime attribute for interface
                    var lifetimeAttribute = lifetimeAttributes.Where(v => v.Interface == i).FirstOrDefault();

                    ushort interfaceLifetimeId = lifetimeAttribute == null ? (ushort)0 : lifetimeAttribute.Id;

                    //Create remap
                    var remap = Activator.CreateInstance(remapType, GCHandle.ToIntPtr(_thisHandle), interfaceLifetimeId);

                    //Find existing lifetime controller array index
                    int controllerArrayId = -1; 
                    for (int c = 0; c < controllers.Count; ++c) {
                        if (controllers[c].LifetimeID == interfaceLifetimeId) {
                            controllerArrayId = c;
                            break;
                        }
                    }
                    //Create lifetime controller if not exists
                    if (controllerArrayId == -1) {
                        controllerArrayId = controllers.Count;
                        controllers.Add(new LifetimeController() { LifetimeID = interfaceLifetimeId, Refcount = 0 });
                    }

                    controllers[controllerArrayId].Interfaces.Add(i.GUID, remap as Details.IUnsafeRemap);

                    //Set lifetime controller array id to remap
                    Details.IUnsafeRemap.SetLifetimeID((remap as Details.IUnsafeRemap).Handle, (ushort)controllerArrayId);
                    
                }
            }
            _lifetimeControllers = controllers.ToArray();
        }

        public ExceptionCode QueryInterface(ref Guid guid, out IntPtr result) {
            for(int i = 0; i < _lifetimeControllers.Length; ++i) {
                var error = QueryLifetimeInterfaceByArrayId((ushort)i, ref guid, out result);
                if(error == ExceptionCode.Ok) {
                    return error;
                }
            }
            result = System.IntPtr.Zero;
            return ExceptionCode.NoInterface;
        }

        private ExceptionCode QueryLifetimeInterfaceByArrayId(ushort lifetimeArrayIndex, ref Guid guid, out IntPtr result) {
            Details.IUnsafeRemap remap;

            _lifetimeControllers[lifetimeArrayIndex].Interfaces.TryGetValue(guid, out remap);

            if (remap != null) {
                if (remap is Details.IInterfaceRemap) {
                    (remap as Details.IInterfaceRemap).AddRef();
                }

                result = remap.Handle;
                return ExceptionCode.Ok;
            } else {
                result = IntPtr.Zero;
            }
            return ExceptionCode.NoInterface;
        }

        /// <summary>
        /// Query interface by lifetime id (specified in LifetimeController attribute)
        /// </summary>
        /// <param name="lifetimeId">Lifetime ID</param>
        /// <param name="guid">Interface GUID</param>
        /// <param name="result">Result pointer to interface. Zero if interface not found</param>
        /// <returns>Returns ExceptionCode.Ok if interface exists</returns>
        private ExceptionCode QueryLifetimeInterface(int lifetimeId, ref Guid guid, out IntPtr result) {
            for(int i = 0; i < _lifetimeControllers.Length; ++i) {
                if(_lifetimeControllers[i].LifetimeID == lifetimeId) {
                    return QueryLifetimeInterfaceByArrayId((ushort)i, ref guid, out result);
                }
            }
            result = IntPtr.Zero;
            return ExceptionCode.NoInterface;
        }

        /// <summary>
        /// Query interface (from first lifetime with specified interface)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T QueryInterface<T>() where T : class, IUnsafe {
            var wrapperType = Details.Utils.GetWrapperType<T>();
            if (wrapperType == null) {
                throw new Exception("Failed to find wrapper for interface " + typeof(T).FullName);
            }

            Guid guid = typeof(T).GUID;
            IntPtr ptr = IntPtr.Zero;
            QueryInterface(ref guid, out ptr);
            if (ptr == IntPtr.Zero) {
                return null;
            }

            return (T)Activator.CreateInstance(wrapperType, ptr);
        }

        /// <summary>
        /// Query interface by lifetime id (specified in LifetimeController attribute)
        /// </summary>
        /// <typeparam name="T">type of interface</typeparam>
        /// <param name="lifetimeId">Lifetime ID</param>
        /// <returns>Returns interface if exists, otherwise null</returns>
        public T QueryLifetimeInterface<T>(int lifetimeId) where T : class, IUnsafe {
            var wrapperType = Details.Utils.GetWrapperType<T>();
            if (wrapperType == null) {
                throw new Exception("Failed to find wrapper for interface " + typeof(T).FullName);
            }

            Guid guid = typeof(T).GUID;
            IntPtr ptr = IntPtr.Zero;

            for (int i = 0; i < _lifetimeControllers.Length; ++i) {
                if (_lifetimeControllers[i].LifetimeID == lifetimeId) {
                    if(QueryLifetimeInterface(i, ref guid, out ptr) == ExceptionCode.Ok) {
                        return (T)Activator.CreateInstance(wrapperType, ptr);
                    }
                    break;
                }
            }

            return null;
        }

        public uint AddRef() {
            return (uint)Interlocked.Increment(ref _refCount);
        }

        public uint Release() {
            var refs = Interlocked.Decrement(ref _refCount);
            if (refs == 0) {
                Destroy();
                foreach(var c in _lifetimeControllers) {
                    foreach (var interfaceRemap in c.Interfaces) {
                        interfaceRemap.Value.Dispose();
                    }
                    c.Interfaces.Clear();
                }
              
                _trackedObjects.Remove(GCHandle.ToIntPtr(_thisHandle));
                _thisHandle.Free();

            }
            return (uint)refs;
        }

        protected abstract void Destroy();

        public virtual void Dispose() { }

        public string getMessage() {
            return _exceptionMessage.Value;
        }

        public void setMessage(string message) {
            _exceptionMessage.Value = message;
        }

        private ThreadLocal<string> _exceptionMessage = new ThreadLocal<string>(() => {
            return "";
        });
    }

    [Guid("9154edf5-09e7-45a7-99d9-6380659fa6f2")]
    public interface ILibraryUnloader : IInterface {
        void unloadLibrary();
    }

    namespace Details {
        public class ILibraryUnloaderWrapper : IInterfaceWrapper, ILibraryUnloader {

            private ILibraryUnloaderRemap.VMT _vmt;

            protected new int GetTotalNativeMethodsCount() {
                return base.GetTotalNativeMethodsCount() + typeof(ILibraryUnloaderRemap.VMT).GetFields().Length;
            }
            public ILibraryUnloaderWrapper() : base() { }

            public ILibraryUnloaderWrapper(IntPtr obj) : base(obj) {
                _vmt = LoadVMT<ILibraryUnloaderRemap.VMT>(base.GetTotalNativeMethodsCount());
            }

            public void unloadLibrary() {
                HandleExceptionCode(_vmt.unloadLibrary(_object));
            }
        }

        public class ILibraryUnloaderRemap : IInterfaceRemap {
            public new struct VMT {
                public delegate ExceptionCode unloadLibraryDelegate(IntPtr _this);
#pragma warning disable 0649
                public unloadLibraryDelegate unloadLibrary;
#pragma warning restore 0649
            }

            public readonly static new NativeInterfaceVmt NativeVmt;
            static ILibraryUnloaderRemap() {
                var vmtBlocks = new List<object>();
                AppendVmt(vmtBlocks);
                NativeVmt = new NativeInterfaceVmt(vmtBlocks);
            }

            protected static new void AppendVmt(List<object> buffer) {
                IInterfaceRemap.AppendVmt(buffer);
                var vmt = new VMT();
                vmt.unloadLibrary = (IntPtr _this) => {
                    var obj = Antilatency.InterfaceContract.Details.IInterfaceRemap.GetContext(_this) as ILibraryUnloader;
                    obj.unloadLibrary();
                    return ExceptionCode.Ok;
                };
                buffer.Add(vmt);
            }

            public ILibraryUnloaderRemap() { }
            public ILibraryUnloaderRemap(System.IntPtr context, ushort lifetimeId) {
                AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
            }
        }

        public static class Utils {
            public static Type GetWrapperType<T>() where T : class, IUnsafe {
                return Type.GetType(typeof(T).Namespace + ".Details." + typeof(T).Name + "Wrapper", false, false);
            }
            public static Type GetRemapType<T>() where T : class, IUnsafe {
                return GetRemapType(typeof(T));
            }
            public static Type GetRemapType(Type interfaceType) {
                return Type.GetType(interfaceType.Namespace + ".Details." + interfaceType.Name + "Remap", false, false);
            }
            public static IntPtr IncrementPointer(IntPtr src, int byteOffset) {
                return new IntPtr(src.ToInt64() + byteOffset);
            }
            public static void IncrementPointer(ref IntPtr src, int byteOffset) {
                src = new IntPtr(src.ToInt64() + byteOffset);
            }
        }
    }

    public delegate ExceptionCode LibraryEntryPoint(IntPtr unloader, out IntPtr result);

    public abstract class Library<T> : InterfacedObject, ILibraryUnloader where T : class, IInterface {
        public abstract void unloadLibrary();

        public T createFactory() {
            var unloaderInterface = this.QueryInterface<ILibraryUnloader>();
            var entry = getEntryPoint();
            IInterface result;
            IntPtr resultIntermediate;

            entry(System.IntPtr.Zero, out resultIntermediate);
            result = new Antilatency.InterfaceContract.Details.IInterfaceWrapper(resultIntermediate);
            unloaderInterface.Dispose();

            var factory = result.QueryInterface<T>();
            result.Dispose();
            return factory;
        }

        protected abstract LibraryEntryPoint getEntryPoint();
        protected override void Destroy() {

        }
    }

    namespace Details {
        public struct ArrayInMarshaler {
            public struct Intermediate {
                public IntPtr data;
                public uint size;

                public T[] toArray<T>() where T : struct {
                    T[] result = new T[size];

                    var readPtr = data;
                    if (size != 0) {
                        for (uint i = 0; i < size; ++i) {
                            result[i] = (T)Marshal.PtrToStructure(readPtr, typeof(T));
                            readPtr = new IntPtr(readPtr.ToInt64() + Marshal.SizeOf(typeof(T)));
                        }
                    }
                    return result;
                }

                public string toString() {
                    byte[] buffer = new byte[size];
                    if (size != 0) {
                        Marshal.Copy(data, buffer, 0, (int)size);
                        return System.Text.Encoding.UTF8.GetString(buffer);
                    } else {
                        return "";
                    }
                }

                public static implicit operator string(Intermediate d) {
                    return d.toString();
                }
            }

            public static implicit operator Intermediate(ArrayInMarshaler marshaler) {
                return marshaler.intermediate;
            }

            public Intermediate intermediate;
            private GCHandle _arrayHandle;

            public static ArrayInMarshaler create<T>(T[] source) where T : struct {
                if (source != null) {
                    var handle = GCHandle.Alloc(source, GCHandleType.Pinned);
                    return new ArrayInMarshaler() { _arrayHandle = handle, intermediate = new Intermediate() { data = handle.AddrOfPinnedObject(), size = (uint)source.Length } };
                } else {
                    return new ArrayInMarshaler();
                }
            }

            public static ArrayInMarshaler create(string source) {
                if (!string.IsNullOrEmpty(source)) {
                    return create(System.Text.Encoding.UTF8.GetBytes(source));
                } else {
                    return create<byte>(null);
                }
            }

            public void Dispose() {
                if (_arrayHandle.IsAllocated) {
                    _arrayHandle.Free();
                }
            }
        };

   
        public abstract class ArrayOutMarshaler {
            public delegate IntPtr setArraySizeDelegate(IntPtr context, uint newSize);
            #if __MonoCS__
            [MonoPInvokeCallback(typeof(setArraySizeDelegate))]
            #endif
            public static IntPtr SetArraySize(IntPtr context, uint newSize) {
                var accessorHandle = GCHandle.FromIntPtr(context);
                var accessor = (ArrayOutMarshaler)accessorHandle.Target;
                return accessor.ResizeBuffer(newSize);
            }

            public struct Intermediate {
                public IntPtr context;
                public setArraySizeDelegate setSizeCallback;
                public void assign<T>(T[] data) where T : struct {
                    if (data != null) {
                        IntPtr destinationMemory = setSizeCallback(context, (uint)data.Length);
                        for (int i = 0; i < data.Length; ++i) {
                            Marshal.StructureToPtr(data[i], destinationMemory, false);
                            destinationMemory = new IntPtr(destinationMemory.ToInt64() + Marshal.SizeOf(typeof(T)));
                        }
                    } else {
                        setSizeCallback(context, 0);
                    }
                }
                public void assign(string data) {
                    assign<byte>(string.IsNullOrEmpty(data) ? null : System.Text.Encoding.UTF8.GetBytes(data));
                }
            }
            public Intermediate intermediate;

            public abstract IntPtr ResizeBuffer(uint newSize);

            public static ArrayOutMarshalerImpl<T> create<T>() {
                return new ArrayOutMarshalerImpl<T>();
            }

            public static ArrayOutMarshalerString create() {
                return new ArrayOutMarshalerString();
            }
        }


        public class ArrayOutMarshalerImpl<T> : ArrayOutMarshaler {
            protected T[] _buffer;
            public T[] value {
                get { return _buffer; }
            }
            private GCHandle _arrayHandle;
            private GCHandle _thisHandle;

            public override IntPtr ResizeBuffer(uint newSize) {
                Array.Resize(ref _buffer, (int)newSize);

                if (newSize == 0) {
                    return IntPtr.Zero;
                }

                _arrayHandle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
                return _arrayHandle.AddrOfPinnedObject();
            }

            

            public static implicit operator Intermediate(ArrayOutMarshalerImpl<T> marshaler) {
                return marshaler.intermediate;
            }
            public ArrayOutMarshalerImpl() {
                _thisHandle = GCHandle.Alloc(this);
                intermediate = new Intermediate() { context = GCHandle.ToIntPtr(_thisHandle), setSizeCallback = SetArraySize };
            }
            public void Dispose() {
                if (_arrayHandle.IsAllocated) {
                    _arrayHandle.Free();
                }
                _thisHandle.Free();
            }
        }

        public class ArrayOutMarshalerString : ArrayOutMarshalerImpl<byte> {
            public new string value {
                get { return _buffer == null ? null : System.Text.Encoding.UTF8.GetString(_buffer); }
            }
        }

        public static class InterfaceMarshaler {
            public static IntPtr ManagedToNative<T>(T ManagedObj) where T : class, IUnsafe {
                if (ReferenceEquals(ManagedObj, null)) {
                    return IntPtr.Zero;
                }
                var wrapper = ManagedObj as IInterfaceWrapper;
                //Check if it is a IInterfaceWrapper
                if (ReferenceEquals(wrapper, null)) {
                    var unsafeWrapper = ManagedObj as IUnsafeWrapper;
                    //Check if it is a IUnsafeWrapper
                    if (ReferenceEquals(unsafeWrapper, null)) {
                        //Try to get new reference from InterfacedObject
                        var interfacedObject = ManagedObj as InterfacedObject;
                        if (ReferenceEquals(interfacedObject, null)) {
                            throw new Exception("Can not marshal object of type " + ManagedObj.GetType().FullName + " as it is not InterfaceWrapper nor InterfacedObect");
                        }
                        var tempWrapper = interfacedObject.QueryInterface<T>() as IUnsafeWrapper;
                        return tempWrapper == null ? IntPtr.Zero : tempWrapper.Detach();
                    } else {
                        return unsafeWrapper.NativePointer;
                    }
                } else {
                    return wrapper.NewNativePointer;
                }
            }
        }
    }
}
