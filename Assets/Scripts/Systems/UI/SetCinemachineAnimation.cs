using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace GDD
{
    public class SetCinemachineAnimation : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera m_vacm;
        [SerializeField] private CinemachineCameraOffset m_vacmOffset;
        [HideInInspector] public LensSettings m_lensSettings;
        [HideInInspector] public CinemachineVirtualCameraBase.TransitionParams m_TransitionParams;
        [HideInInspector] public Vector3 m_trackedObjectOffset;

        private CinemachineComposer _cinemachineComposer;
        private CinemachineTransposer _cinemachineTransposer;

        // Start is called before the first frame update
        void Start()
        {
            m_lensSettings = m_vacm.m_Lens;
            m_TransitionParams = m_vacm.m_Transitions;
            _cinemachineComposer = m_vacm.GetCinemachineComponent<CinemachineComposer>();
            _cinemachineTransposer = m_vacm.GetCinemachineComponent<CinemachineTransposer>();

            print($"Is Null = {_cinemachineComposer == null}");
        }

        public void SetOrthographicSize(float size)
        {
            m_lensSettings.OrthographicSize = size;
            m_vacm.m_Lens = m_lensSettings;
        }
        
        public void SetPerspectivePOV(float value)
        {
            m_lensSettings.FieldOfView = value;
            m_vacm.m_Lens = m_lensSettings;
        }

        public void SetCameraPositionZ(float value)
        {
            m_vacmOffset.m_Offset = new Vector3(m_vacmOffset.m_Offset.x, m_vacmOffset.m_Offset.y, value);
        }
        
        public void SetCameraPositionOffset(string offset = "0,0,0")
        {
            string[] getOffset = offset.Split(",");
            m_vacmOffset.m_Offset = new Vector3(float.Parse(getOffset[0]),
                float.Parse(getOffset[1]), float.Parse(getOffset[2]));
        }

        public void SetTrackedObjectOffset(string offset = "0,0,0")
        {
            string[] getOffset = offset.Split(",");
            _cinemachineComposer.m_TrackedObjectOffset = new Vector3(float.Parse(getOffset[0]),
                float.Parse(getOffset[1]), float.Parse(getOffset[2]));
        }
    }
}