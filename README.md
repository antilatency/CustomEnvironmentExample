Custom environment demonstration
================================

[![Video Antilatency Custom Environment](https://img.youtube.com/vi/XAbDLjyo0A4/0.jpg)](https://www.youtube.com/watch?v=XAbDLjyo0A4)

[Video AntilatencyCustomEnvironmentDemo](https://www.youtube.com/watch?v=XAbDLjyo0A4 "https://www.youtube.com/watch?v=XAbDLjyo0A4")

Prerequisites:
  - 3 IR LED markers to construct environment. Note that markers should not form self-similar pattern, such as equilateral triangle.
  - 1 Alt tracker.

How to run:
1. Open the project in Unity.
2. Open SampleScene.
3. Place objects marker0, marker1 and marker2 according to positions of your real markers. 
4. Connect Alt tracker.
5. Run sample in editor.

Visualization:
Markers displayed as encircled white dots.
Last successful IEnvironment.match() call provides:
  - rays(painted blue) from calculated position to projection on floor
  - projections, transformed to match markers(blue crosses on the floor plane)
  - differences between transformed projections and corresponding markers(painted red)
Last IEnvironment.matchByPosition() provides:
  - matched rays painted green
  - unmatched rays painted magenta

Interesting files:
  - CustomEnvironment.cs: minimal correct IEnvironment implementation with explanations.
  - ThreeMarkersEnvironment.cs: implementaion of simple environment, consisting of 3 markers on the floor plane. 
  - ThreeMarkersEnvironmentComponent.cs: implementation of AltEnvironmentComponent, required by AltTracking script. AltEnvironmentComponent inherits MonoBehaviour and provides environment object to tracking.
