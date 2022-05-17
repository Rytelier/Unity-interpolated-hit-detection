# Unity-interpolated-hit-detection
Interpolated hit detection with approximation of inbetween frames, using spherecasts

Like my work? Check out my stuff: https://twitter.com/Rytelier
This repo is fully free, if you want support me, you can donate at https://ko-fi.com/rytelier

Because detecting if attached colliders collides with other one is done in fixed physics frame step, it might not detect the hit if between the frames the collider doesn't overlap with target.
The interpolated hit detection system uses points assigned onto weapon object and casts spherecast from previous to current frame, making whole area between the frames connected.
Additionally, it creates additional segments with curve approximation to give it even more accuracy, because the perceived curve in animation might not be read in plain frame to frame cast.

![hit detect](https://user-images.githubusercontent.com/45795134/168833369-18540611-3431-4fdd-95a5-88dd4d306243.png)
