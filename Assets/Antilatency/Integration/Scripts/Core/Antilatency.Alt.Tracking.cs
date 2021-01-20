//Copyright 2020, ALT LLC. All Rights Reserved.
//This file is part of Antilatency SDK.
//It is subject to the license terms in the LICENSE file found in the top-level directory
//of this distribution and at http://www.antilatency.com/eula
//You may not use this file except in compliance with the License.
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
#pragma warning disable IDE1006 // Do not warn about naming style violations
#pragma warning disable IDE0017 // Do not suggest to simplify object initialization
using System.Runtime.InteropServices; //GuidAttribute
namespace Antilatency.Alt.Tracking {

[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct MarkerIndex {
	public static readonly MarkerIndex MaximumValidMarkerIndex = new MarkerIndex(){ value = 0xFFFFFFF0 };
	public static readonly MarkerIndex Invalid = new MarkerIndex(){ value = 0xFFFFFFFE };
	public static readonly MarkerIndex Unknown = new MarkerIndex(){ value = 0xFFFFFFFF };
	[System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
	public uint value;
	public override string ToString() {
		switch (value) {
			case 0xFFFFFFF0: return "MaximumValidMarkerIndex";
			case 0xFFFFFFFE: return "Invalid";
			case 0xFFFFFFFF: return "Unknown";
		}
		return value.ToString();
	}
	public static implicit operator uint(MarkerIndex value) { return value.value;}
	public static explicit operator MarkerIndex(uint value) { return new MarkerIndex() { value = value }; }
}

[Guid("c257c858-f296-43b7-b6b5-c14b9afb1a13")]
public interface IEnvironment : Antilatency.InterfaceContract.IInterface {
	Antilatency.InterfaceContract.Bool isMutable();
	UnityEngine.Vector3[] getMarkers();
	Antilatency.InterfaceContract.Bool filterRay(UnityEngine.Vector3 up, UnityEngine.Vector3 ray);
	/// <param name = "raysUpSpace">
	/// rays directions. Normalized
	/// </param>
	Antilatency.InterfaceContract.Bool match(UnityEngine.Vector3[] raysUpSpace, out Antilatency.Alt.Tracking.MarkerIndex[] markersIndices, out UnityEngine.Pose poseOfUpSpace);
	/// <summary>Match rays to markers by known position</summary>
	/// <param name = "rays">
	/// rays directions world space. Normalized
	/// </param>
	/// <param name = "origin">
	/// Common rays origin world space
	/// </param>
	/// <returns>Indices of corresponding markers. result.size == rays.size</returns>
	Antilatency.Alt.Tracking.MarkerIndex[] matchByPosition(UnityEngine.Vector3[] rays, UnityEngine.Vector3 origin);
}
namespace Details {
	public class IEnvironmentWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, IEnvironment {
		private IEnvironmentRemap.VMT _VMT = new IEnvironmentRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(IEnvironmentRemap.VMT).GetFields().Length;
		}
		public IEnvironmentWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<IEnvironmentRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.InterfaceContract.Bool isMutable() {
			Antilatency.InterfaceContract.Bool result;
			Antilatency.InterfaceContract.Bool resultMarshaler;
			HandleExceptionCode(_VMT.isMutable(_object, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
		public UnityEngine.Vector3[] getMarkers() {
			UnityEngine.Vector3[] result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create<UnityEngine.Vector3>();
			HandleExceptionCode(_VMT.getMarkers(_object, resultMarshaler));
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public Antilatency.InterfaceContract.Bool filterRay(UnityEngine.Vector3 up, UnityEngine.Vector3 ray) {
			Antilatency.InterfaceContract.Bool result;
			Antilatency.InterfaceContract.Bool resultMarshaler;
			HandleExceptionCode(_VMT.filterRay(_object, up, ray, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
		public Antilatency.InterfaceContract.Bool match(UnityEngine.Vector3[] raysUpSpace, out Antilatency.Alt.Tracking.MarkerIndex[] markersIndices, out UnityEngine.Pose poseOfUpSpace) {
			Antilatency.InterfaceContract.Bool result;
			Antilatency.InterfaceContract.Bool resultMarshaler;
			var raysUpSpaceMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(raysUpSpace);
			var markersIndicesMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create<Antilatency.Alt.Tracking.MarkerIndex>();
			UnityEngine.Pose poseOfUpSpaceMarshaler;
			HandleExceptionCode(_VMT.match(_object, raysUpSpaceMarshaler, markersIndicesMarshaler, out poseOfUpSpaceMarshaler, out resultMarshaler));
			raysUpSpaceMarshaler.Dispose();
			markersIndices = markersIndicesMarshaler.value;
			markersIndicesMarshaler.Dispose();
			poseOfUpSpace = poseOfUpSpaceMarshaler;
			result = resultMarshaler;
			return result;
		}
		public Antilatency.Alt.Tracking.MarkerIndex[] matchByPosition(UnityEngine.Vector3[] rays, UnityEngine.Vector3 origin) {
			Antilatency.Alt.Tracking.MarkerIndex[] result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create<Antilatency.Alt.Tracking.MarkerIndex>();
			var raysMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(rays);
			HandleExceptionCode(_VMT.matchByPosition(_object, raysMarshaler, origin, resultMarshaler));
			raysMarshaler.Dispose();
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
	}
	public class IEnvironmentRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode isMutableDelegate(System.IntPtr _this, out Antilatency.InterfaceContract.Bool result);
			public delegate Antilatency.InterfaceContract.ExceptionCode getMarkersDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode filterRayDelegate(System.IntPtr _this, UnityEngine.Vector3 up, UnityEngine.Vector3 ray, out Antilatency.InterfaceContract.Bool result);
			public delegate Antilatency.InterfaceContract.ExceptionCode matchDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate raysUpSpace, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate markersIndices, out UnityEngine.Pose poseOfUpSpace, out Antilatency.InterfaceContract.Bool result);
			public delegate Antilatency.InterfaceContract.ExceptionCode matchByPositionDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate rays, UnityEngine.Vector3 origin, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			#pragma warning disable 0649
			public isMutableDelegate isMutable;
			public getMarkersDelegate getMarkers;
			public filterRayDelegate filterRay;
			public matchDelegate match;
			public matchByPositionDelegate matchByPosition;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static IEnvironmentRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.isMutable = (System.IntPtr _this, out Antilatency.InterfaceContract.Bool result) => {
				try {
					var obj = GetContext(_this) as IEnvironment;
					var resultMarshaler = obj.isMutable();
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.InterfaceContract.Bool);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getMarkers = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as IEnvironment;
					var resultMarshaler = obj.getMarkers();
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.filterRay = (System.IntPtr _this, UnityEngine.Vector3 up, UnityEngine.Vector3 ray, out Antilatency.InterfaceContract.Bool result) => {
				try {
					var obj = GetContext(_this) as IEnvironment;
					var resultMarshaler = obj.filterRay(up, ray);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.InterfaceContract.Bool);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.match = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate raysUpSpace, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate markersIndices, out UnityEngine.Pose poseOfUpSpace, out Antilatency.InterfaceContract.Bool result) => {
				try {
					var obj = GetContext(_this) as IEnvironment;
					Antilatency.Alt.Tracking.MarkerIndex[] markersIndicesMarshaler;
					UnityEngine.Pose poseOfUpSpaceMarshaler;
					poseOfUpSpace = default(UnityEngine.Pose);
					var resultMarshaler = obj.match(raysUpSpace.toArray<UnityEngine.Vector3>(), out markersIndicesMarshaler, out poseOfUpSpaceMarshaler);
					markersIndices.assign(markersIndicesMarshaler);
					poseOfUpSpace = poseOfUpSpaceMarshaler;
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.InterfaceContract.Bool);
					poseOfUpSpace = default(UnityEngine.Pose);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.matchByPosition = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate rays, UnityEngine.Vector3 origin, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as IEnvironment;
					var resultMarshaler = obj.matchByPosition(rays.toArray<UnityEngine.Vector3>(), origin);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public IEnvironmentRemap() { }
		public IEnvironmentRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

/// <summary>Part of Antilatency.Alt.Tracking.Stability structure. Since meaning of Stability.value depends on stage, this meaning described here.</summary>
[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct Stage {
	/// <summary>Tracker collect accelerometer data to find upright orientation. No tracking data is valid in this stage. value = 0</summary>
	public static readonly Stage InertialDataInitialization = new Stage(){ value = 0x0 };
	/// <summary>Only rotation is valid in this stage. Rotation around vertical axis may drift. value = 0</summary>
	public static readonly Stage Tracking3Dof = new Stage(){ value = 0x1 };
	/// <summary>Full 6 dof tracking, corrected by optics. Value represents how stable solution is. Since this value depends on many factors, there is no units for it. This value may be used for debug purposes. </summary>
	public static readonly Stage Tracking6Dof = new Stage(){ value = 0x2 };
	/// <summary>Inertial only 6 dof tracking, without optical corrections. Value is a fraction of time left before switch to Tracking3Dof. The value is useful for animation effects to notify the user about the oncoming loss of tracking.</summary>
	public static readonly Stage TrackingBlind6Dof = new Stage(){ value = 0x3 };
	[System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
	public int value;
	public override string ToString() {
		switch (value) {
			case 0x0: return "InertialDataInitialization";
			case 0x1: return "Tracking3Dof";
			case 0x2: return "Tracking6Dof";
			case 0x3: return "TrackingBlind6Dof";
		}
		return value.ToString();
	}
	public static implicit operator int(Stage value) { return value.value;}
	public static explicit operator Stage(int value) { return new Stage() { value = value }; }
}

/// <summary>Represents stability of tracking</summary>
[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct Stability {
	public Antilatency.Alt.Tracking.Stage stage;
	/// <summary>Meaning of value depends on stage. See Antilatency.Alt.Tracking.Stage</summary>
	public float value;
}

[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct State {
	/// <summary>World space position meters, local to world rotation</summary>
	public UnityEngine.Pose pose;
	/// <summary>World space, meters per second</summary>
	public UnityEngine.Vector3 velocity;
	/// <summary>Local space, radians per second</summary>
	public UnityEngine.Vector3 localAngularVelocity;
	/// <summary>Tracking stability</summary>
	public Antilatency.Alt.Tracking.Stability stability;
}

[Guid("7f8b603c-fa91-4168-92b7-af1644d087db")]
public interface ITrackingCotask : Antilatency.DeviceNetwork.ICotask {
	/// <param name = "placement">
	/// Position (meters) and orientation (quaternion) of tracker relative to origin of tracked object.
	/// </param>
	/// <param name = "deltaTime">
	/// Extrapolation time (seconds).
	/// </param>
	Antilatency.Alt.Tracking.State getExtrapolatedState(UnityEngine.Pose placement, float deltaTime);
	Antilatency.Alt.Tracking.State getState(float angularVelocityAvgTimeInSeconds);
}
namespace Details {
	public class ITrackingCotaskWrapper : Antilatency.DeviceNetwork.Details.ICotaskWrapper, ITrackingCotask {
		private ITrackingCotaskRemap.VMT _VMT = new ITrackingCotaskRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ITrackingCotaskRemap.VMT).GetFields().Length;
		}
		public ITrackingCotaskWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ITrackingCotaskRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.Alt.Tracking.State getExtrapolatedState(UnityEngine.Pose placement, float deltaTime) {
			Antilatency.Alt.Tracking.State result;
			Antilatency.Alt.Tracking.State resultMarshaler;
			HandleExceptionCode(_VMT.getExtrapolatedState(_object, placement, deltaTime, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
		public Antilatency.Alt.Tracking.State getState(float angularVelocityAvgTimeInSeconds) {
			Antilatency.Alt.Tracking.State result;
			Antilatency.Alt.Tracking.State resultMarshaler;
			HandleExceptionCode(_VMT.getState(_object, angularVelocityAvgTimeInSeconds, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
	}
	public class ITrackingCotaskRemap : Antilatency.DeviceNetwork.Details.ICotaskRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode getExtrapolatedStateDelegate(System.IntPtr _this, UnityEngine.Pose placement, float deltaTime, out Antilatency.Alt.Tracking.State result);
			public delegate Antilatency.InterfaceContract.ExceptionCode getStateDelegate(System.IntPtr _this, float angularVelocityAvgTimeInSeconds, out Antilatency.Alt.Tracking.State result);
			#pragma warning disable 0649
			public getExtrapolatedStateDelegate getExtrapolatedState;
			public getStateDelegate getState;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ITrackingCotaskRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.DeviceNetwork.Details.ICotaskRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.getExtrapolatedState = (System.IntPtr _this, UnityEngine.Pose placement, float deltaTime, out Antilatency.Alt.Tracking.State result) => {
				try {
					var obj = GetContext(_this) as ITrackingCotask;
					var resultMarshaler = obj.getExtrapolatedState(placement, deltaTime);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.Alt.Tracking.State);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getState = (System.IntPtr _this, float angularVelocityAvgTimeInSeconds, out Antilatency.Alt.Tracking.State result) => {
				try {
					var obj = GetContext(_this) as ITrackingCotask;
					var resultMarshaler = obj.getState(angularVelocityAvgTimeInSeconds);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.Alt.Tracking.State);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ITrackingCotaskRemap() { }
		public ITrackingCotaskRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

[Guid("009ebfe1-f85c-4638-be9d-af7990a8cd04")]
public interface ITrackingCotaskConstructor : Antilatency.DeviceNetwork.ICotaskConstructor {
	Antilatency.Alt.Tracking.ITrackingCotask startTask(Antilatency.DeviceNetwork.INetwork network, Antilatency.DeviceNetwork.NodeHandle node, Antilatency.Alt.Tracking.IEnvironment environment);
}
namespace Details {
	public class ITrackingCotaskConstructorWrapper : Antilatency.DeviceNetwork.Details.ICotaskConstructorWrapper, ITrackingCotaskConstructor {
		private ITrackingCotaskConstructorRemap.VMT _VMT = new ITrackingCotaskConstructorRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ITrackingCotaskConstructorRemap.VMT).GetFields().Length;
		}
		public ITrackingCotaskConstructorWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ITrackingCotaskConstructorRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.Alt.Tracking.ITrackingCotask startTask(Antilatency.DeviceNetwork.INetwork network, Antilatency.DeviceNetwork.NodeHandle node, Antilatency.Alt.Tracking.IEnvironment environment) {
			Antilatency.Alt.Tracking.ITrackingCotask result;
			System.IntPtr resultMarshaler;
			var networkMarshaler = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.DeviceNetwork.INetwork>(network);
			var environmentMarshaler = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.Alt.Tracking.IEnvironment>(environment);
			HandleExceptionCode(_VMT.startTask(_object, networkMarshaler, node, environmentMarshaler, out resultMarshaler));
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.Alt.Tracking.Details.ITrackingCotaskWrapper(resultMarshaler);
			return result;
		}
	}
	public class ITrackingCotaskConstructorRemap : Antilatency.DeviceNetwork.Details.ICotaskConstructorRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode startTaskDelegate(System.IntPtr _this, System.IntPtr network, Antilatency.DeviceNetwork.NodeHandle node, System.IntPtr environment, out System.IntPtr result);
			#pragma warning disable 0649
			public startTaskDelegate startTask;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ITrackingCotaskConstructorRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.DeviceNetwork.Details.ICotaskConstructorRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.startTask = (System.IntPtr _this, System.IntPtr network, Antilatency.DeviceNetwork.NodeHandle node, System.IntPtr environment, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as ITrackingCotaskConstructor;
					var networkMarshaler = network == System.IntPtr.Zero ? null : new Antilatency.DeviceNetwork.Details.INetworkWrapper(network);
					var environmentMarshaler = environment == System.IntPtr.Zero ? null : new Antilatency.Alt.Tracking.Details.IEnvironmentWrapper(environment);
					var resultMarshaler = obj.startTask(networkMarshaler, node, environmentMarshaler);
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.Alt.Tracking.ITrackingCotask>(resultMarshaler);
				}
				catch (System.Exception ex) {
					result = default(System.IntPtr);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ITrackingCotaskConstructorRemap() { }
		public ITrackingCotaskConstructorRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

[Guid("13ac393d-a7c5-4e51-a6eb-feaa11c3c049")]
public interface ILibrary : Antilatency.InterfaceContract.IInterface {
	Antilatency.Alt.Tracking.IEnvironment createEnvironment(string code);
	UnityEngine.Pose createPlacement(string code);
	string encodePlacement(UnityEngine.Vector3 position, UnityEngine.Vector3 rotation);
	Antilatency.Alt.Tracking.ITrackingCotaskConstructor createTrackingCotaskConstructor();
}
public static class Library{
    [DllImport("AntilatencyAltTracking")]
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
		public Antilatency.Alt.Tracking.IEnvironment createEnvironment(string code) {
			Antilatency.Alt.Tracking.IEnvironment result;
			System.IntPtr resultMarshaler;
			var codeMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(code);
			HandleExceptionCode(_VMT.createEnvironment(_object, codeMarshaler, out resultMarshaler));
			codeMarshaler.Dispose();
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.Alt.Tracking.Details.IEnvironmentWrapper(resultMarshaler);
			return result;
		}
		public UnityEngine.Pose createPlacement(string code) {
			UnityEngine.Pose result;
			UnityEngine.Pose resultMarshaler;
			var codeMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(code);
			HandleExceptionCode(_VMT.createPlacement(_object, codeMarshaler, out resultMarshaler));
			codeMarshaler.Dispose();
			result = resultMarshaler;
			return result;
		}
		public string encodePlacement(UnityEngine.Vector3 position, UnityEngine.Vector3 rotation) {
			string result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create();
			HandleExceptionCode(_VMT.encodePlacement(_object, position, rotation, resultMarshaler));
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public Antilatency.Alt.Tracking.ITrackingCotaskConstructor createTrackingCotaskConstructor() {
			Antilatency.Alt.Tracking.ITrackingCotaskConstructor result;
			System.IntPtr resultMarshaler;
			HandleExceptionCode(_VMT.createTrackingCotaskConstructor(_object, out resultMarshaler));
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.Alt.Tracking.Details.ITrackingCotaskConstructorWrapper(resultMarshaler);
			return result;
		}
	}
	public class ILibraryRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode createEnvironmentDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate code, out System.IntPtr result);
			public delegate Antilatency.InterfaceContract.ExceptionCode createPlacementDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate code, out UnityEngine.Pose result);
			public delegate Antilatency.InterfaceContract.ExceptionCode encodePlacementDelegate(System.IntPtr _this, UnityEngine.Vector3 position, UnityEngine.Vector3 rotation, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode createTrackingCotaskConstructorDelegate(System.IntPtr _this, out System.IntPtr result);
			#pragma warning disable 0649
			public createEnvironmentDelegate createEnvironment;
			public createPlacementDelegate createPlacement;
			public encodePlacementDelegate encodePlacement;
			public createTrackingCotaskConstructorDelegate createTrackingCotaskConstructor;
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
			vmt.createEnvironment = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate code, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					var resultMarshaler = obj.createEnvironment(code);
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.Alt.Tracking.IEnvironment>(resultMarshaler);
				}
				catch (System.Exception ex) {
					result = default(System.IntPtr);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.createPlacement = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate code, out UnityEngine.Pose result) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					var resultMarshaler = obj.createPlacement(code);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(UnityEngine.Pose);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.encodePlacement = (System.IntPtr _this, UnityEngine.Vector3 position, UnityEngine.Vector3 rotation, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					var resultMarshaler = obj.encodePlacement(position, rotation);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.createTrackingCotaskConstructor = (System.IntPtr _this, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					var resultMarshaler = obj.createTrackingCotaskConstructor();
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.Alt.Tracking.ITrackingCotaskConstructor>(resultMarshaler);
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
		public ILibraryRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

[Guid("e664544b-afd5-4723-949a-9a888526ef97")]
public interface IEnvironmentMutable : Antilatency.InterfaceContract.IInterface {
	Antilatency.InterfaceContract.Bool mutate(float[] powers, UnityEngine.Vector3[] rays, float sphereD, UnityEngine.Vector2[] x, Antilatency.Math.float2x3[] xOverPosition, UnityEngine.Vector3 up);
	int getUpdateId();
}
namespace Details {
	public class IEnvironmentMutableWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, IEnvironmentMutable {
		private IEnvironmentMutableRemap.VMT _VMT = new IEnvironmentMutableRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(IEnvironmentMutableRemap.VMT).GetFields().Length;
		}
		public IEnvironmentMutableWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<IEnvironmentMutableRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.InterfaceContract.Bool mutate(float[] powers, UnityEngine.Vector3[] rays, float sphereD, UnityEngine.Vector2[] x, Antilatency.Math.float2x3[] xOverPosition, UnityEngine.Vector3 up) {
			Antilatency.InterfaceContract.Bool result;
			Antilatency.InterfaceContract.Bool resultMarshaler;
			var powersMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(powers);
			var raysMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(rays);
			var xMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(x);
			var xOverPositionMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(xOverPosition);
			HandleExceptionCode(_VMT.mutate(_object, powersMarshaler, raysMarshaler, sphereD, xMarshaler, xOverPositionMarshaler, up, out resultMarshaler));
			powersMarshaler.Dispose();
			raysMarshaler.Dispose();
			xMarshaler.Dispose();
			xOverPositionMarshaler.Dispose();
			result = resultMarshaler;
			return result;
		}
		public int getUpdateId() {
			int result;
			int resultMarshaler;
			HandleExceptionCode(_VMT.getUpdateId(_object, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
	}
	public class IEnvironmentMutableRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode mutateDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate powers, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate rays, float sphereD, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate x, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate xOverPosition, UnityEngine.Vector3 up, out Antilatency.InterfaceContract.Bool result);
			public delegate Antilatency.InterfaceContract.ExceptionCode getUpdateIdDelegate(System.IntPtr _this, out int result);
			#pragma warning disable 0649
			public mutateDelegate mutate;
			public getUpdateIdDelegate getUpdateId;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static IEnvironmentMutableRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.mutate = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate powers, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate rays, float sphereD, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate x, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate xOverPosition, UnityEngine.Vector3 up, out Antilatency.InterfaceContract.Bool result) => {
				try {
					var obj = GetContext(_this) as IEnvironmentMutable;
					var resultMarshaler = obj.mutate(powers.toArray<float>(), rays.toArray<UnityEngine.Vector3>(), sphereD, x.toArray<UnityEngine.Vector2>(), xOverPosition.toArray<Antilatency.Math.float2x3>(), up);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.InterfaceContract.Bool);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getUpdateId = (System.IntPtr _this, out int result) => {
				try {
					var obj = GetContext(_this) as IEnvironmentMutable;
					var resultMarshaler = obj.getUpdateId();
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(int);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public IEnvironmentMutableRemap() { }
		public IEnvironmentMutableRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

public static partial class Constants {
	public const float DefaultAngularVelocityAvgTime = 0.016f;
}


}
