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

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Antilatency.Integration {
    /// <summary>
    /// Alt environment base abstract class, represents the Antilatency tracking environment (IR markers pattern).
    /// </summary>
    public abstract class AltEnvironmentComponent : MonoBehaviour {

        /// <returns>Native IEnvironment object.</returns>
        public abstract Antilatency.Alt.Tracking.IEnvironment GetEnvironment();
    }
}
