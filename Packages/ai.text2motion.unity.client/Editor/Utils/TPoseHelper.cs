
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Text2Motion.Utils
{
    public static class TPoseHelper
    {

        public static Dictionary<string, List<decimal>> GetTPoseFromFbxModel(Transform rootBone, GameObject gameObject)
        {
            if (rootBone == null)
            {
                throw new InvalidOperationException("Root bone is required to get T-Pose.");
            }
            Debug.Log("Attempting to load T-Pose");

            string assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            if (string.IsNullOrWhiteSpace(assetPath))
            {
                throw new InvalidOperationException("Failed to get asset path from GameObject.");
            }

            ModelImporter modelImporter = (ModelImporter)AssetImporter.GetAtPath(assetPath);
            if (modelImporter == null)
            {
                throw new InvalidOperationException("Failed to load ModelImporter from AssetPath: " + assetPath);
            }

            ModelImporterAnimationType oldAnimationType = modelImporter.animationType;
            bool changedTypeToHuman = false;
            if (modelImporter.animationType != ModelImporterAnimationType.Human)
            {
                Debug.Log("Change animation type to Humanoid for asset: " + assetPath);
                modelImporter.animationType = ModelImporterAnimationType.Human;
                modelImporter.SaveAndReimport();
                changedTypeToHuman = true;
            }

            Debug.Log("Grabbing the avatar");
            Avatar avatarObj = (Avatar)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Avatar));
            if (avatarObj == null)
            {
                throw new InvalidOperationException("Failed to load Avatar from AssetPath: " + assetPath);
            }
            if (!gameObject.TryGetComponent(out Animator animator))
            {
                throw new InvalidOperationException("Failed to load Animator from GameObject.");
            }
            Avatar oldAvatar = animator.avatar;
            if (changedTypeToHuman)
            {
                animator.avatar = avatarObj;
            }

            var tPoseMatrixMap = GetTPoseMapFromHumanoidAnimator(animator);

            if (changedTypeToHuman)
            {
                Debug.Log("Change animation type back to: " + oldAnimationType);
                modelImporter.animationType = oldAnimationType;
                modelImporter.SaveAndReimport();
                animator.avatar = oldAvatar;
            }

            return tPoseMatrixMap;
        }

        public static Dictionary<string, List<decimal>> GetTPoseMapFromHumanoidAnimator(Animator animator)
        {
            if (animator.isHuman == false)
            {
                throw new ArgumentException("The animator is not humanoid.");
            }

            // Referenced from https://discussions.unity.com/t/reset-to-t-pose/624025/24
            var tPoseMatrixMap = new Dictionary<string, List<decimal>>();
            SkeletonBone[] skeletonBones = animator.avatar.humanDescription.skeleton; // Get the list of bones in the armature.
            foreach (SkeletonBone sb in skeletonBones) // Loop through all bones in the armature.
            {
                foreach (HumanBodyBones hbb in Enum.GetValues(typeof(HumanBodyBones)))
                {
                    if (hbb != HumanBodyBones.LastBone)
                    {
                        Transform bone = animator.GetBoneTransform(hbb);
                        if (bone != null)
                        {
                            if (sb.name == bone.name) // If this bone is a normal humanoid bone (as opposed to an ear or tail bone), reset its transform.
                            {
                                // The bicycle pose happens when for some reason the transforms of an avatar's bones are incorrectly saved in a state that is not the t-pose.
                                // For most of the bones this affects only their rotation, but for the hips, the position is affected as well.
                                // As the scale should be untouched, and the user may have altered these intentionally, we should leave them alone.

                                if (hbb == HumanBodyBones.Hips)
                                {
                                    tPoseMatrixMap[bone.name] = MatrixExtensions.GetLocalMatrix(
                                        t: sb.position,
                                        r: sb.rotation,
                                        s: bone.localScale,
                                        isLeftHanded: false,
                                        isZup: false,
                                        scaleFactor: 1.0f
                                    ).ToDecimalList();
                                }
                                else
                                {
                                    tPoseMatrixMap[bone.name] = MatrixExtensions.GetLocalMatrix(
                                        bone.localPosition,
                                        r: sb.rotation,
                                        bone.localScale,
                                        isLeftHanded: false,
                                        isZup: false,
                                        scaleFactor: 1.0f
                                    ).ToDecimalList();
                                }

                                break; // We found a HumanBodyBone that matches, so we need not check the rest against this skeleton bone.
                            }

                        }
                    }
                }
            }
            return tPoseMatrixMap;
        }
    }

}