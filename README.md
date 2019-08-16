# Rubber Hand VR

An extendable toolkit for exploring neuroplasticity in VR with the leap motion

[Script Reference](https://unoctium1.github.io/RubberHandVR/)

Requires a leap motion and an OpenVR compatible VR headset. Created with Unity 2018.4.

Tested on both the Oculus Rift and the HTC vive, but *should* work with any OpenVR headset.

Rubber Hand VR is a framework for creating study designs involving modifying virtual tracked hands. The project aims to both replicate and extend the [rubber hand experiment](https://en.wikipedia.org/wiki/Body_transfer_illusion). Users will perform basic tasks in VR, such as pressing buttons or sorting blocks, and various hand modifications can be triggered - altering their virtual hand's shape, position, etc. The project is designed to allow developers to create custom tasks and modifications to suit a variety of different purposes. Rubber Hand VR also includes a customizable UI, to allow users to control which tasks are being performed, and for how long. 

## Using the prebuilt app
Download the executable folder in the releases tab.

The app expects 2 users: A study conductor, to trigger tasks and hand modifications, and a participant, to wear the headset and perform tasks. If only one user is available, there is an option to turn on a demo reel - the app will run through all available tasks and modifications, with no required desktop interaction.

If both users are available, the conductor will have a desktop interface. Tasks are on the far left of the screen, while modifications are on the bottom left. To turn on a task, set a time in minutes in the middle input field, then click "start." The participant will then be presented with an instruction screen, and, upon reading and accepting the instruction screen, they will perform the task for the set time. If the conductor wishes to stop the task early, they can hit the "stop" button. If they wish to trigger a modification, they must first hit either the "use right hand" or "use left hand" button. They then can hit the "start" modification button. To restore the hand to its original state, they must hit "reset."

## Building custom modifications
To build custom modifications, or to change the existing ones, download the unitypackage in the releases tab. Add the unity package to a clean project with the leap motion orion unity SDK and the Leap interaction engine. The Leap hands module is not necessary, but is required to run the sample scene, and is recommended. Create a copy of the TemplateScene scene file. 

To build a custom interaction, create a monobehaviour class that implements [the IHand interface](https://unoctium1.github.io/RubberHandVR/interface_hand_v_r_1_1_i_hand.html). It is recommended to keep most of the core functionality of the modification outside of this class. Instead, create static methods that perform the desired transformations in the [HandModifierManager class](https://unoctium1.github.io/RubberHandVR/class_hand_v_r_1_1_hand_modifier_manager.html) or use a Leap PostProcessProvider (as an example, see the [DisplacementPostProcessProvider](https://unoctium1.github.io/RubberHandVR/class_hand_v_r_1_1_modification_examples_1_1_displacement_post_process_provider.html). FOr modifications that require attaching objects to a hand, use the Leap attachment hand object. Example hand modifications are in the [ModificationExamples namespace](https://unoctium1.github.io/RubberHandVR/namespace_hand_v_r_1_1_modification_examples.html).

Once the modification is created, create a prefab variant of the 'mod' prefab in the prefabs folder. Add the component implementing the IHand interface, and then add the new prefab you've created to the modlist in the GameManager in your scene. 

## Building custom tasks
To build custom modifications, or to change the existing ones, download the unitypackage in the releases tab. Add the unity package to a clean project with the leap motion orion unity SDK and the Leap interaction engine. The Leap hands module is not necessary, but is required to run the sample scene, and is recommended. Create a copy of the TemplateScene scene file. 

To build a custom task, create a monobehaviour class that implements [the ITest interface](https://unoctium1.github.io/RubberHandVR/interface_hand_v_r_1_1_i_test.html). As examples of how to create a task, see the ButtonGame or BlockGame namespaces and prefabs. Then, create a prefab with your behaviour and add it to the test list in the the gamemanager, along with a brief label describing it. 

Along with creating a behaviour for the task, you should also implement the [ITestData](https://unoctium1.github.io/RubberHandVR/interface_hand_v_r_1_1_i_test_data.html) interface. Create a struct to record whatever data your task might be recording. This could include time to create a trial, accuracy, or any other metrics. 
