using System.Collections.Generic;
using UnityEngine;

namespace CameraStackTest
{
    public interface IBaseCameraStack
    {
        void AddCamerasToStack(List<Camera> cameras);
        void AddCameraToStack(Camera camera);
        void RemoveCamerasFromStack(List<Camera> cameras);
        void RemoveCameraFromStack(Camera camera);
    }
}