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
namespace AndroidJniWrapper {

[Guid("bd9f72ed-f6e0-44dd-b642-57801f568cea")]
public interface IAndroidJni : Antilatency.InterfaceContract.IInterface {
	void initJni(System.IntPtr vm, System.IntPtr context);
}
namespace Details {
	public class IAndroidJniWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, IAndroidJni {
		private IAndroidJniRemap.VMT _VMT = new IAndroidJniRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(IAndroidJniRemap.VMT).GetFields().Length;
		}
		public IAndroidJniWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<IAndroidJniRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public void initJni(System.IntPtr vm, System.IntPtr context) {
			HandleExceptionCode(_VMT.initJni(_object, vm, context));
		}
	}
	public class IAndroidJniRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode initJniDelegate(System.IntPtr _this, System.IntPtr vm, System.IntPtr context);
			#pragma warning disable 0649
			public initJniDelegate initJni;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static IAndroidJniRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.initJni = (System.IntPtr _this, System.IntPtr vm, System.IntPtr context) => {
				try {
					var obj = GetContext(_this) as IAndroidJni;
					obj.initJni(vm, context);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public IAndroidJniRemap() { }
		public IAndroidJniRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}


}
