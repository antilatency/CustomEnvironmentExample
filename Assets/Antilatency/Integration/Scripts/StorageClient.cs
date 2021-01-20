// Copyright 2020, ALT LLC. All Rights Reserved.
// This file is part of Antilatency SDK.
// It is subject to the license terms in the LICENSE file found in the top-level directory
// of this distribution and at http://www.antilatency.com/eula
// You may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Antilatency.Integration {
    /// <summary>
    /// Antilatency Storage Client wrapper. Allows you to communicate with Antilatency Storage: gets/sets default environment/placement, etc.
    /// </summary>
    public class StorageClient {

        /// <summary>
        /// Get local storage. 
        /// </summary>
        public static Antilatency.StorageClient.IStorage GetLocalStorage (){
            using (var library = GetLibrary()) {
                if (library == null) {
                    return null;
                }

                var storage = library.getLocalStorage();
                if (storage == null) {
                    Debug.LogError("Failed to get local storage");
                    return null;
                }

                if (!storage.exists()) {
                    Utils.SafeDispose(ref storage);
                    Debug.LogError("Failed to connect to local storage");
                    return null;
                }


                return storage;
            }
        }

        /// <summary>
        /// Get remote storage. 
        /// </summary>
        public static Antilatency.StorageClient.IStorage GetRemoteStorage(string ipAddress, uint port) {
            using (var library = GetLibrary()) {
                if (library == null) {
                    return null;
                }

                var storage = library.getRemoteStorage(ipAddress, port);
                if (storage == null) {
                    Debug.LogError("Failed to get remote storage");
                    return null;
                }

                if (!storage.exists()) {
                    Utils.SafeDispose(ref storage);
                    Debug.LogError("Failed to connect to remote storage at " + ipAddress + ":" + port);
                    return null;
                }

                return storage;
            }
        }

        private static Antilatency.StorageClient.ILibrary GetLibrary() {
            var library = Antilatency.StorageClient.Library.load();
            if (library == null) {
                Debug.LogError("Failed to load AltSystemClient library");
                return null;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            var jni = library.QueryInterface<AndroidJniWrapper.IAndroidJni>();
            using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity")) {
                    jni.initJni(IntPtr.Zero, activity.GetRawObject());
                }
            }
            jni.Dispose();
#endif
            return library;
        }
    }
}