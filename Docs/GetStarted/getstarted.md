[HoloStylusToolkit-Unity](../../README.md)

<img src="../../Images/Logo_small.png" width="40%"/>

# Get Started With HoloStylusToolkit-Unity

Congrats, hopefully you are one of the first developers with Holo-Light's Holo-Stylus in your hands and want to create your first Apps in Unity. Then that's the right place for you. Here you can find some helpful material for your first steps with the Stylus.

# Manual

First you could have a look at our [booklet](../Manual/Booklet_Stylus_WEB_2018_02.pdf) where you can find detailed information about the Stylus and the head-mounted unit (HMU).

# Prepare your Development Environment

If you are not familiar with development of Mixed Reality Applications for Microsoft HoloLens in Unity 3D a good starting point could be the official [GitHub](https://github.com/) repository of [Microsoft's Mixed Reality Toolkit](https://github.com/Microsoft/MixedRealityToolkit). For HoloStylusToolkit-Unity it is **not required** to use Mixed Reality Toolkit, but it has the same [prerequisites](https://github.com/Microsoft/MixedRealityToolkit-Unity/blob/master/GettingStarted.md) and Mixed Reality Toolkit could also be a good option for you to facitlitate your App development.

The minimum required version of Unity 3D for Holo-Stylus Toolkit is 2017.4.x .

# Download the HoloStylusToolkit-Unity asset packages

You can download the latest unity package from [Releases](../../Releases/) folder.

# Adding the HoloStylusToolkit-Unity package to your Unity project

Open or create your project in Unity.
Then import the Holo-Stylus Toolkit asset using <code>Assets -> Import Package -> Custom Package…</code>

# HoloStylusToolkit-Unity Overview

After successfully importing the Holo-Stylus Toolkit in your Unity project your Assets folder should contain a directory HoloLight. In the Stylus subdirectory there is one folder with Examples and a second one named Toolkit.

<img src="../../Images/HoloStylusToolkit-Unity_Assets.png" alt="Assets folder" />

## Toolkit

The Toolkit is intended to simplify your developer's life. It contains useful scripts and components to help you with connecting and interactions with the Stylus in your applications.

The most important prefabs are **DeviceManager** and **StylusManager** which are located in <code>Toolkit --> UnityAssets --> Prefabs</code>. To set up the most basic Holo-Stylus scene just drag the prefabs DeviceManager and StylusManager in the hierarchy of an empty Unity scene. Also add the prefab **DefaultCursor** from <code>HoloLight --> Stylus --> Examples --> Core --> Prefabs</code> to the hierarchy. Before pressing the Start button of Unity Editor make sure that your **Camera** is set to the origin **(0,0,0)**. Now you can test if the asset import was successful by starting your scene in Unity Player. If everything is ok you should see a small cursor near the mouse cursor when moving the mouse pointer in the scene view of Unity, which is simulating the Stylus tip. You can move the cursor in z direction with W (positive z) and S (negative z) keys of the keyboard. And when clicking a button a message appears near the mouse cursor.

<img src="../../Images/StylusToolkit_ActionButtonClicked.png"/>

## Examples

There are some basic example scenes contained in the Assets folder HoloLight --> Stylus --> Examples. You can study them to learn how to connect with the Stylus by bluetooth or how to do basic interaction with the Stylus.

# How to test the Stylus on Microsoft HoloLens

To test the Stylus on the HoloLens just open the SceneCollection scene from the Examples folder <code>Examples --> SceneCollection --> Scenes</code> by double clicking it. The SceneCollection scene is a collection of all scenes of the examples folder and the best starting point. Build the scene for Universal Windows Platform as you can read in the next section.

## Building your project

From the Unity main menu select File --> Build Settings.

<img src="../../Images/HoloStylusToolkit_Build.png"/>

In the build dialog all scenes are already selected. Select as platform Universal Windows Platform and activate the option Unity C# Projects. Click Build.

<img src="../../Images/HoloStylusToolkit_Build Settings_UWP.png"/>

Create a new folder or select an empty folder where your Visual Studio project should be built in. After successfully building the new folder will contain a Visual Studio solution Holo-Stylus-Toolkit.sln.

<img src="../../Images/HoloStylusToolkit_BuildFolder.png"/>

## Running your project for HoloLens

Open the Holo-Stylus-Toolkit.sln with Visual Studio 2017.

<img src="../../Images/VisualStudioSolution.png"/>

1. Plug in your HoloLens via USB
1. Select Release, x86 and Device as build settings.
1. Build the app and deploy it to the HoloLens.

<img src="../../Images/VisualStudio_BuildSettings.png"/>

When the application starts on HoloLens you first should select the Connection scene and all available bluetooth devices will be listed. You can select the Holo-Stylus from the device list by using the gaze. Looking with the gaze about 2 seconds will activate a menu entry. When connection to your Stylus was successful you can go back to the main menu and calibrate your Stylus if needed. There you can set the visual tip exactly to the tip of the Stylus.

Now you are ready to try out all the other demo scenes. Have fun with your Stylus !







