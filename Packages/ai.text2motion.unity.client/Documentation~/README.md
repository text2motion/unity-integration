# Overview

> [!NOTE]
> This package is currently in early preview and is subjected to changes at any time.

![Example animation generation](images/generate_animation_example.gif)

Text2Motion's Unity integration client, can be use to make request to Text2Motion's API for generating Animation Clip on a given humanoid model.

Currently, the animation can only be generated for character with Mixamo rig name and structure. This package's sample provided an example Mixamo model that can be used for animation generation. Support for retargeting to other humanoid rig structure may be introduced in the future. Meanwhile, it is possible to take the animation generated from the example model, convert it to humanoid clip, and apply it to other non-Mixamo skeleton.

Text2Motion generates Animation Clip for Generic type FBX rig. This is because the output of Text2Motion API is transformation of bone position/rotation in individual keyframe. Humanoid Animation is based on how much the muscle is stretched instead of transformation/rotation of the bone. Also Humanoid Animation does not support keyframe. To make the generated generic animation clip more useful, you can follow the instruction to [Converting Generic Clip to Humanoid Clip](#converting-generic-clip-to-humanoid-clip).

## Table of Contents

1. [Getting Started](#getting-started)
   1. [Obtaining the API Key](#obtaining-the-api-key)
   2. [Unity installation](#unity-installation)
   3. [Installing the package](#installing-the-package)
   4. [Animation Generation Script](#animation-generation-script)
2. [Features](#features)
3. [Additional Information](#additional-information)
   1. [Modifying keyframe](#modifying-keyframe)
   2. [Converting Generic Clip to Humanoid Clip](#converting-generic-clip-to-humanoid-clip)

## Getting Started

### Obtaining the API Key

In order to use Text2Motion, you must sign up for an account and obtain the API Key via the developer portal.

1. Go to [https://developer.text2motion.ai/](https://developer.text2motion.ai/)  
![Developer Porta](images/Obtaining_API_Key-Portal.png)
2. Click on Sign in on the upper right corner  
![Sign in](images/Obtaining_API_Key-Sign_In.png)
3. Click on **LOGIN WITH SAML**
4. Choose either **Continue with Google** for Google SSO, or **Sign Up** with any email you own
5. Click on your username on the upper right corner of the site
6. Click on **Apps**  
![Apps](images/Obtaining_API_Key-Apps.png)
7. Click on **+ NEW APP** on the right side of the screen  
![New App](images/Obtaining_API_Key-New_App.png)
8. Put in an **App Name**, it could be `Unity Client` or whatever you want
9. Click **Enable** for **Text2motion Free Tier**  
![Create App](images/Obtaining_API_Key-Create_App.png)
10. Click **Save**
11. You should now have an API Key. This will be needed later.
![Api Key](images/Obtaining_API_Key-Key.png)

### Unity installation

This package is only tested for Unity version `2022.3.4f1`. Other version have not been tested.

### Installing the package

1. Go to **Window** > **Package Manager**  
![Package Manager dropdown](images/Installing-Package_Manager.png)
2. Open the **add** ![Add icon](images/Installing-Add_Package_Icon.png) menu in the Package Manager’s **toolbar**.
3. The options for adding packages appear.  
   ![Add package from git URL button](images/Installing-Add_Package_From_Git.png)  
   Add package from git URL button
4. Select **Add package from git URL** from the add menu. A text box and an **Add** button appear.
5. Add the `Text2Motion.CSharpClient` package first

   ```markdown
   https://github.com/text2motion/unity-integration.git?path=/Packages/ai.text2motion.unity.csharpclient
   ```

6. Add the `Text2Motion` package

   ```markdown
   https://github.com/text2motion/unity-integration.git?path=/Packages/ai.text2motion.unity.client
   ```

7. (Optional) From the **Samples** tab of Text2Motion Unity client package, import the **Example Maximorig model** sample.  
![Adding Samples](images/Installing-Add_Sample.png)

### Animation Generation Script

> [!NOTE]
> All generated animations are licensed under [CC-by-4.0](https://creativecommons.org/licenses/by/4.0/)

This instruction uses the **Example Maximorig model** to create animation. You may try using your own model as long as it is are using Maximo rigs.

1. Create an empty **GameObject** in the Scene from the **Hierarchy** window  
![Create Empty Object](images/Animation_Generation-Create_Empty.png)
2. Drag `Assets/Samples/Text2Motion/1.0.0/Example Maximorig Model/Y Bot` under the **GameObject** created in the previous step  
![Use Sample Model](images/Animation_Generation-Use_Sample_Model.gif)
3. With **Y Bot** selected in the Hierarchy window, click on **Add Component** in the **Inspector** window
4. Select **Component** > **Scripts** > **Text2Motion** > **Animation Clip Generator**  
![Add Script Component](images/Animation_Generation-Add_Script.gif)
5. Fill in the key you obtained from [Obtaining the API Key](#obtaining-the-api-key) here  
![Login](images/Animation_Generation-Login.png)
6. Drag `mixamorig:Hips` under **Y Bot** into **Root Bone** for **Animation input settings**  
![Initialize T-Pose](images/Animation_Generation-T_Pose.gif)
7. Click **Initialize T-Pose from FBX asset**
8. Click **Load**
9. After that, you should be able to generate animation by providing a prompt  
![Animation generation widget](images/Animation_Generation-Generate.png)

## Development

We recommend using Visual Studio Code for developing this package. To get started, make sure you follow the [Unity installation](#unity-installation) step first. Then with the Unity project open:

1. Go to **Edit** > **Preferences...** > **External Tools**  
![Preferences Windows](images/Development-Preference_Window.png)
2. Change **External Script Editor** to `Visual Studio Code` and click **Regenerate project files**  
![Regenerate project files](images/Development-Preference-External_Tools.png)

After that, set up Visual Studio code

1. [Visual Studio Code](https://code.visualstudio.com/)
2. [.Net SDK for VS Code](https://dotnet.microsoft.com/en-us/download/dotnet/sdk-for-vs-code?utm_source=vs-code&utm_medium=referral&utm_campaign=sdk-install)
3. [Unity Development with VS Code](https://code.visualstudio.com/docs/other/unity)

From here you should be able to open up the repository in Visual Studio Code and start developing. You can hit `F5` to start debugger and set breakpoint to test the Package.

## Features

The **Animation Clip Generator** script has 4 tabs.

- **New** tab let you create new animation clips
  - Clip Name must be unique
    - If left empty, one will be generated
  - Advanced Parameters:
    - You can disable root motion by unchecking **Apply Root Motion** so the animation play in place without moving the character away from the starting location.  
   ![Apply Root Motion](images/Features_apply-root-motion.gif)
      - Since the generated animation clip is generic type, changing **Apply Root Motion** setting on the **Animator** will not work.
      - Disable root motion using the option under **Animation output settings** and regenerate a new animation
      - Alternatively, if you [Converting Generic Clip to Humanoid Clip](#converting-generic-clip-to-humanoid-clip), **Apply Root Motion** setting on the **Animator** will work.
    - Position offset can be use to adjust the character's position in the generated animation. For example, the current sample we have sinks into the ground so we move it up by 0.37 in the Y axis.  
![New](images/Features-New.png)
- **Replace** tab is essentially the same as **New**, but instead of creating new Clip, you select an existing clip to overrides  
![Replace](images/Features-Replace.png)
- **Load** tab let you load animation from a list of previous Text2Motion API request, in case the clips were deleted.  
![Load](images/Features-Load.png)
- **Option** tab let you reset the script and also point to additional setting under the **Project Settings**. Note that you must click **Reload Settings** button under the **Option** tab to refresh setting cache if the **Project Settings** was modified.  
![Options](images/Features-Options.png)

## Additional information

### Modifying keyframe

Text2Motion generates generic animation clips, which means you will be able to modify keyframe directly. This may be useful for trimming generated animation where the start and end of the animation isn't doing much, or just for snipping part of the animation.

![Example of modifying keyframe](images/Modifying_Keyframe.gif)

### Converting Generic Clip to Humanoid Clip

It may be useful to convert the Generic Animation Clip generated from Text2Motion to Humanoid Clip. For example, once it is converted to Humanoid Clip, we can apply this animation on any other humanoid rig type that may not be maximorig structure.

To do this, get the [FBX Exporter](https://docs.unity3d.com/Packages/com.unity.formats.fbx@2.0/manual/index.html) package first.

Once you have the package:

1. Select the **GameObject** with the Text2Motion animation clips.
2. Right click the **GameObject** and select **Export to FBX**
3. For **Options** > **Include** select **Model(s) + Animation**.
4. Click **Export**
5. Select the exported fbx file in **Project** window
6. In the **Inspector** window, go to the **Rig** tab
7. Change the **Animation Type** from **Generic** to **Humanoid**
8. Hit **Apply**

From here, you'd be able to duplicate the animation clip in the FBX file and use it elsewhere for other humanoid rig.
