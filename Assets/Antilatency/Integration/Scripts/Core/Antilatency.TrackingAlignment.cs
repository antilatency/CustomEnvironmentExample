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
namespace Antilatency.TrackingAlignment {

[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct State {
	public Antilatency.Math.doubleQ rotationARelativeToB;
	public Antilatency.Math.doubleQ rotationBSpace;
	public double timeBAheadOfA;
}

/// <summary>This thing allows to align Alt tracking rotation with third-party tracking, i.e. obtain such rotationBSpace, rotationARelativeToB quaternions, that a = rotationBSpace*b*rotationARelativeToB for a == Alt tracking rotation without extrapolation, b == third-party tracker's rotation as is</summary>
[Guid("502bf5a7-9d17-4419-b575-973f467e13d4")]
public interface ITrackingAlignment : Antilatency.InterfaceContract.IInterface {
	/// <summary>Provide data to optimizer</summary>
	/// <param name = "a">
	/// Alt rotation without extrapolation
	/// </param>
	/// <param name = "b">
	/// Third-party tracker's rotation as is
	/// </param>
	/// <param name = "time">
	/// Current time in seconds
	/// </param>
	Antilatency.TrackingAlignment.State update(Antilatency.Math.doubleQ a, Antilatency.Math.doubleQ b, double time);
}
namespace Details {
	public class ITrackingAlignmentWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, ITrackingAlignment {
		private ITrackingAlignmentRemap.VMT _VMT = new ITrackingAlignmentRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ITrackingAlignmentRemap.VMT).GetFields().Length;
		}
		public ITrackingAlignmentWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ITrackingAlignmentRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.TrackingAlignment.State update(Antilatency.Math.doubleQ a, Antilatency.Math.doubleQ b, double time) {
			Antilatency.TrackingAlignment.State result;
			Antilatency.TrackingAlignment.State resultMarshaler;
			HandleExceptionCode(_VMT.update(_object, a, b, time, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
	}
	public class ITrackingAlignmentRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode updateDelegate(System.IntPtr _this, Antilatency.Math.doubleQ a, Antilatency.Math.doubleQ b, double time, out Antilatency.TrackingAlignment.State result);
			#pragma warning disable 0649
			public updateDelegate update;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ITrackingAlignmentRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.update = (System.IntPtr _this, Antilatency.Math.doubleQ a, Antilatency.Math.doubleQ b, double time, out Antilatency.TrackingAlignment.State result) => {
				try {
					var obj = GetContext(_this) as ITrackingAlignment;
					var resultMarshaler = obj.update(a, b, time);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.TrackingAlignment.State);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ITrackingAlignmentRemap() { }
		public ITrackingAlignmentRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

[Guid("5f53c3cf-8080-41f1-8769-4992c0db36ea")]
public interface ILibrary : Antilatency.InterfaceContract.IInterface {
	Antilatency.TrackingAlignment.ITrackingAlignment createTrackingAlignment(Antilatency.Math.doubleQ initialARelativeToB, double initialTimeBAheadOfA);
}
public static class Library{
    [DllImport("AntilatencyTrackingAlignment")]
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
		public Antilatency.TrackingAlignment.ITrackingAlignment createTrackingAlignment(Antilatency.Math.doubleQ initialARelativeToB, double initialTimeBAheadOfA) {
			Antilatency.TrackingAlignment.ITrackingAlignment result;
			System.IntPtr resultMarshaler;
			HandleExceptionCode(_VMT.createTrackingAlignment(_object, initialARelativeToB, initialTimeBAheadOfA, out resultMarshaler));
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.TrackingAlignment.Details.ITrackingAlignmentWrapper(resultMarshaler);
			return result;
		}
	}
	public class ILibraryRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode createTrackingAlignmentDelegate(System.IntPtr _this, Antilatency.Math.doubleQ initialARelativeToB, double initialTimeBAheadOfA, out System.IntPtr result);
			#pragma warning disable 0649
			public createTrackingAlignmentDelegate createTrackingAlignment;
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
			vmt.createTrackingAlignment = (System.IntPtr _this, Antilatency.Math.doubleQ initialARelativeToB, double initialTimeBAheadOfA, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					var resultMarshaler = obj.createTrackingAlignment(initialARelativeToB, initialTimeBAheadOfA);
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.TrackingAlignment.ITrackingAlignment>(resultMarshaler);
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


}
