#pragma warning disable IDE1006 // Do not warn about naming style violations
#pragma warning disable IDE0017 // Do not suggest to simplify object initialization
using System.Runtime.InteropServices; //GuidAttribute
namespace Antilatency.Bracer {

[Guid("34ef891c-9dd9-429e-8e03-62ef014b0c24")]
public interface ICotask : Antilatency.DeviceNetwork.ICotask {
	void executeVibration();
	uint getTouchNativeValue();
}
namespace Details {
	public class ICotaskWrapper : Antilatency.DeviceNetwork.Details.ICotaskWrapper, ICotask {
		private ICotaskRemap.VMT _VMT = new ICotaskRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ICotaskRemap.VMT).GetFields().Length;
		}
		public ICotaskWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ICotaskRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public void executeVibration() {
			HandleExceptionCode(_VMT.executeVibration(_object));
		}
		public uint getTouchNativeValue() {
			uint result;
			uint resultMarshaler;
			HandleExceptionCode(_VMT.getTouchNativeValue(_object, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
	}
	public class ICotaskRemap : Antilatency.DeviceNetwork.Details.ICotaskRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode executeVibrationDelegate(System.IntPtr _this);
			public delegate Antilatency.InterfaceContract.ExceptionCode getTouchNativeValueDelegate(System.IntPtr _this, out uint result);
			#pragma warning disable 0649
			public executeVibrationDelegate executeVibration;
			public getTouchNativeValueDelegate getTouchNativeValue;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ICotaskRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.DeviceNetwork.Details.ICotaskRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.executeVibration = (System.IntPtr _this) => {
				try {
					var obj = GetContext(_this) as ICotask;
					obj.executeVibration();
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getTouchNativeValue = (System.IntPtr _this, out uint result) => {
				try {
					var obj = GetContext(_this) as ICotask;
					var resultMarshaler = obj.getTouchNativeValue();
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(uint);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ICotaskRemap() { }
		public ICotaskRemap(System.IntPtr context) {
			AllocateNativeInterface(NativeVmt.Handle, context);
		}
	}
}

[Guid("ade6f0ed-3511-4994-8bcb-7017a46712f1")]
public interface ICotaskConstructor : Antilatency.DeviceNetwork.ICotaskConstructor {
	Antilatency.Bracer.ICotask startTask(Antilatency.DeviceNetwork.INetwork network, Antilatency.DeviceNetwork.NodeHandle node);
}
namespace Details {
	public class ICotaskConstructorWrapper : Antilatency.DeviceNetwork.Details.ICotaskConstructorWrapper, ICotaskConstructor {
		private ICotaskConstructorRemap.VMT _VMT = new ICotaskConstructorRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ICotaskConstructorRemap.VMT).GetFields().Length;
		}
		public ICotaskConstructorWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ICotaskConstructorRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.Bracer.ICotask startTask(Antilatency.DeviceNetwork.INetwork network, Antilatency.DeviceNetwork.NodeHandle node) {
			Antilatency.Bracer.ICotask result;
			System.IntPtr resultMarshaler;
			var networkMarshaler = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.DeviceNetwork.INetwork>(network);
			HandleExceptionCode(_VMT.startTask(_object, networkMarshaler, node, out resultMarshaler));
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.Bracer.Details.ICotaskWrapper(resultMarshaler);
			return result;
		}
	}
	public class ICotaskConstructorRemap : Antilatency.DeviceNetwork.Details.ICotaskConstructorRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode startTaskDelegate(System.IntPtr _this, System.IntPtr network, Antilatency.DeviceNetwork.NodeHandle node, out System.IntPtr result);
			#pragma warning disable 0649
			public startTaskDelegate startTask;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ICotaskConstructorRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.DeviceNetwork.Details.ICotaskConstructorRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.startTask = (System.IntPtr _this, System.IntPtr network, Antilatency.DeviceNetwork.NodeHandle node, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as ICotaskConstructor;
					var networkMarshaler = network == System.IntPtr.Zero ? null : new Antilatency.DeviceNetwork.Details.INetworkWrapper(network);
					var resultMarshaler = obj.startTask(networkMarshaler, node);
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.Bracer.ICotask>(resultMarshaler);
				}
				catch (System.Exception ex) {
					result = default(System.IntPtr);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ICotaskConstructorRemap() { }
		public ICotaskConstructorRemap(System.IntPtr context) {
			AllocateNativeInterface(NativeVmt.Handle, context);
		}
	}
}

[Guid("03e5fde8-d90d-422f-8d06-47b0203c8b2c")]
public interface ILibrary : Antilatency.InterfaceContract.IInterface {
	Antilatency.Bracer.ICotaskConstructor getCotaskConstructor();
}
public static class Library{
    [DllImport("AntilatencyBracer")]
    private static extern Antilatency.InterfaceContract.ExceptionCode getLibraryInterface(System.IntPtr unloader, out System.IntPtr result);
    public static ILibrary load(){
        System.IntPtr libraryAsIInterfaceIntermediate;
        getLibraryInterface(System.IntPtr.Zero, out libraryAsIInterfaceIntermediate);
        Antilatency.InterfaceContract.IInterface libraryAsIInterface = new Antilatency.InterfaceContract.Details.IInterfaceWrapper(libraryAsIInterfaceIntermediate);
        var library = libraryAsIInterface.QueryInterface<ILibrary>();
        libraryAsIInterface.Dispose();
        return library;
    }
}
namespace Details {
	public class ILibraryWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, ILibrary {
		private ILibraryRemap.VMT _VMT = new ILibraryRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ILibraryRemap.VMT).GetFields().Length;
		}
		public ILibraryWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ILibraryRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.Bracer.ICotaskConstructor getCotaskConstructor() {
			Antilatency.Bracer.ICotaskConstructor result;
			System.IntPtr resultMarshaler;
			HandleExceptionCode(_VMT.getCotaskConstructor(_object, out resultMarshaler));
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.Bracer.Details.ICotaskConstructorWrapper(resultMarshaler);
			return result;
		}
	}
	public class ILibraryRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode getCotaskConstructorDelegate(System.IntPtr _this, out System.IntPtr result);
			#pragma warning disable 0649
			public getCotaskConstructorDelegate getCotaskConstructor;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ILibraryRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.getCotaskConstructor = (System.IntPtr _this, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					var resultMarshaler = obj.getCotaskConstructor();
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.Bracer.ICotaskConstructor>(resultMarshaler);
				}
				catch (System.Exception ex) {
					result = default(System.IntPtr);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ILibraryRemap() { }
		public ILibraryRemap(System.IntPtr context) {
			AllocateNativeInterface(NativeVmt.Handle, context);
		}
	}
}

public static partial class Constants {
	public const uint VibrationDurationMs = 50;
}


}
