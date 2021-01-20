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
namespace Antilatency.StorageClient {

[Guid("792d9d14-a2d2-42ad-aeec-7f8a6ba62bd0")]
public interface IStorage : Antilatency.InterfaceContract.IInterface {
	string read(string group, string key);
	void write(string group, string key, string data);
	void remove(string group, string key);
	string next(string group, string key);
	Antilatency.InterfaceContract.Bool exists();
}
namespace Details {
	public class IStorageWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, IStorage {
		private IStorageRemap.VMT _VMT = new IStorageRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(IStorageRemap.VMT).GetFields().Length;
		}
		public IStorageWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<IStorageRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public string read(string group, string key) {
			string result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create();
			var groupMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(group);
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			HandleExceptionCode(_VMT.read(_object, groupMarshaler, keyMarshaler, resultMarshaler));
			groupMarshaler.Dispose();
			keyMarshaler.Dispose();
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public void write(string group, string key, string data) {
			var groupMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(group);
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			var dataMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(data);
			HandleExceptionCode(_VMT.write(_object, groupMarshaler, keyMarshaler, dataMarshaler));
			groupMarshaler.Dispose();
			keyMarshaler.Dispose();
			dataMarshaler.Dispose();
		}
		public void remove(string group, string key) {
			var groupMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(group);
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			HandleExceptionCode(_VMT.remove(_object, groupMarshaler, keyMarshaler));
			groupMarshaler.Dispose();
			keyMarshaler.Dispose();
		}
		public string next(string group, string key) {
			string result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create();
			var groupMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(group);
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			HandleExceptionCode(_VMT.next(_object, groupMarshaler, keyMarshaler, resultMarshaler));
			groupMarshaler.Dispose();
			keyMarshaler.Dispose();
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public Antilatency.InterfaceContract.Bool exists() {
			Antilatency.InterfaceContract.Bool result;
			Antilatency.InterfaceContract.Bool resultMarshaler;
			HandleExceptionCode(_VMT.exists(_object, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
	}
	public class IStorageRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode readDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate group, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode writeDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate group, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate data);
			public delegate Antilatency.InterfaceContract.ExceptionCode removeDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate group, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key);
			public delegate Antilatency.InterfaceContract.ExceptionCode nextDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate group, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode existsDelegate(System.IntPtr _this, out Antilatency.InterfaceContract.Bool result);
			#pragma warning disable 0649
			public readDelegate read;
			public writeDelegate write;
			public removeDelegate remove;
			public nextDelegate next;
			public existsDelegate exists;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static IStorageRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.read = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate group, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as IStorage;
					var resultMarshaler = obj.read(group, key);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.write = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate group, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate data) => {
				try {
					var obj = GetContext(_this) as IStorage;
					obj.write(group, key, data);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.remove = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate group, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key) => {
				try {
					var obj = GetContext(_this) as IStorage;
					obj.remove(group, key);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.next = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate group, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as IStorage;
					var resultMarshaler = obj.next(group, key);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.exists = (System.IntPtr _this, out Antilatency.InterfaceContract.Bool result) => {
				try {
					var obj = GetContext(_this) as IStorage;
					var resultMarshaler = obj.exists();
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.InterfaceContract.Bool);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public IStorageRemap() { }
		public IStorageRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

[Guid("85abab02-be0f-4836-9c1c-3c730bbd0251")]
public interface ILibrary : Antilatency.InterfaceContract.IInterface {
	Antilatency.StorageClient.IStorage getRemoteStorage(string ipAddress, uint port);
	Antilatency.StorageClient.IStorage getLocalStorage();
}
public static class Library{
    [DllImport("AntilatencyStorageClient")]
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
		public Antilatency.StorageClient.IStorage getRemoteStorage(string ipAddress, uint port) {
			Antilatency.StorageClient.IStorage result;
			System.IntPtr resultMarshaler;
			var ipAddressMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(ipAddress);
			HandleExceptionCode(_VMT.getRemoteStorage(_object, ipAddressMarshaler, port, out resultMarshaler));
			ipAddressMarshaler.Dispose();
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.StorageClient.Details.IStorageWrapper(resultMarshaler);
			return result;
		}
		public Antilatency.StorageClient.IStorage getLocalStorage() {
			Antilatency.StorageClient.IStorage result;
			System.IntPtr resultMarshaler;
			HandleExceptionCode(_VMT.getLocalStorage(_object, out resultMarshaler));
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.StorageClient.Details.IStorageWrapper(resultMarshaler);
			return result;
		}
	}
	public class ILibraryRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode getRemoteStorageDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate ipAddress, uint port, out System.IntPtr result);
			public delegate Antilatency.InterfaceContract.ExceptionCode getLocalStorageDelegate(System.IntPtr _this, out System.IntPtr result);
			#pragma warning disable 0649
			public getRemoteStorageDelegate getRemoteStorage;
			public getLocalStorageDelegate getLocalStorage;
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
			vmt.getRemoteStorage = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate ipAddress, uint port, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					var resultMarshaler = obj.getRemoteStorage(ipAddress, port);
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.StorageClient.IStorage>(resultMarshaler);
				}
				catch (System.Exception ex) {
					result = default(System.IntPtr);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getLocalStorage = (System.IntPtr _this, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					var resultMarshaler = obj.getLocalStorage();
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.StorageClient.IStorage>(resultMarshaler);
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
