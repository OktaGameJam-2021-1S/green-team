using UnityEngine;
using Cinemachine;
 
/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class LockCameraY : CinemachineExtension
{
    [Tooltip("Lock the camera's Y position to this value")]
    public float m_YPosition = 0;
    public float m_MinXPosition = -1;
    public float m_MaxXPosition = 0;
 
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            float dy = state.Lens.OrthographicSize;
            float dx = dy * state.Lens.Aspect;

            var pos = state.RawPosition;
            pos.y = m_YPosition;
            if (pos.x + dx > m_MaxXPosition)
            {
                pos.x = m_MaxXPosition - dx;
            }
            else if (pos.x - dx < m_MinXPosition)
            {
                pos.x = m_MinXPosition + dx;
            }
            state.RawPosition = pos;
        }
    }
}
 