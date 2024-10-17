# Unity Integration

> [!NOTE]
> This package is currently in early preview and is subject to changes at any time.

![Example animation generation](images/generate_animation_example.gif)

This repo contains the following:

1. Unity packages:
   1. `ai.text2motion.unity.client`: Text2Motion's unity integration client, can be use to make request to Text2Motion's API for generating Animation Clip on a given humanoid model.
   2. `ai.text2motion.unity.csharpclient`: Text2Motion's openapi spec generated CSharp Client.
2. Example Unity project with the above two package loaded along with a simple model with the integration script attached for generating animation.

## ai.text2motion.unity.client

See the [Package Documentation](Packages/ai.text2motion.unity.client/Documentation~/README.md) for more information.

## ai.text2motion.unity.csharpclient

This package is generated. To update this package, follow the instruction in comment in [generate.sh](Packages/ai.text2motion.unity.csharpclient/generate.sh)

## Example Unity Project

To run the Unity project:

1. Clone this GitHub Repository
2. Install Unity version `2022.3.4f1` via the [Unity Hub](https://docs.unity3d.com/hub/manual/InstallHub.html)
3. Open Unity Hub
4. Click on **Add** and **Add project from disk**  
![Adding project](images/Unity_Hub-Add_Project.png)
5. Navigate to the cloned GitHub Repository root and click **Add Project**

Once the project fully loaded, open **TestScene** under `Assets/Scenes`
![Opening TestScene](images/ExampleProject_TestScene.png)
