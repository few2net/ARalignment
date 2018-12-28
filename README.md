# ARalignment

AR system for mold alignment<br>
FIBO (KMUTT) & HIT lab (UTAS) collaboration project.

### Abstract
<p align="justify">&emsp;&emsp; Forging is one of the most important and efficient method for bulk material processing techniques. Currently, the setup procedure is delayed because of the complexity of the assembly and disassembly process between mold and the machine due to the tolerance and the size of equipment. This research has utilized AR technology to solve the time-wasting problem by subplate alignment. The software of this research was developed using the ARcore SDK together with Unity on the Android platform.</p>
<p align="justify">&emsp;&emsp; The system allows the user to align the subplate from top view directly. The Marker is attached to the ENOMOTO 700GFH to creates a virtual image of the final position for the operator, to move the subplate to match that position. During the development of this program, there was a problem in adjusting the offset model because of three main problems: Human error, Plane error and ARcore error. This research uses pose adjusting method to eliminates human error. But in the other problems, this research cannot be resolved because they are an internal problem caused by the ARcore SDK algorithm, which cannot be studied. However, this research was conducted to collect the error results from the problem. By experimenting with simulated environments, which control the variables in the most efficient state of the program. As a result, the maximum error is 50-80mm, which is higher than the acceptable error of 7mm. The improvement needs in order to resolve this problem to match the factory's requirements</p>

### System requirements
1) Unity 2017.4.15f1 or later
2) ARcore SDK for Unity 1.4.0 or later
3) Android SDK 7.0 (API Level 24) or later
