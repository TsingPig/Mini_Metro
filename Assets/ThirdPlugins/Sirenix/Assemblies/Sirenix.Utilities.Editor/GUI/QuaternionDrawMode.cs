//-----------------------------------------------------------------------
// <copyright file="QuaternionDrawMode.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.Utilities.Editor
{
    /// <summary>
    /// Draw mode of quaternion fields.
    /// </summary>
    /// <seealso cref="SirenixEditorFields"/>
    /// <seealso cref="Sirenix.OdinInspector.Editor.GeneralDrawerConfig"/>
    public enum QuaternionDrawMode
    {
        /// <summary>
        /// Draw the quaterion as euler angles.
        /// </summary>
        Eulers = 0,

        /// <summary>
        /// Draw the quaterion in as an angle and an axis.
        /// </summary>
        AngleAxis = 1,

        /// <summary>
        /// Draw the quaternion as raw x, y, z and w values.
        /// </summary>
        Raw = 2,
    }
}
#endif