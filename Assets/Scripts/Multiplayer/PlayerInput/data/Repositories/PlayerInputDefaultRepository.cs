using System;
using Multiplayer.PlayerInput.domain.model;
using Multiplayer.PlayerInput.domain.repositories;
using UnityEngine;
using Utils.Misc;

namespace Multiplayer.PlayerInput.data.Repositories
{
    internal class PlayerInputDefaultRepository : IPlayerInputRepository
    {
        public bool GetSelection(out int selection) => NumericInput.GetNumericInput(out selection);

        public float GetAxis(PlayerInputAxis axis) => axis switch
        {
            PlayerInputAxis.Shooting => Input.GetMouseButton(0) ? 1f : 0f,
            PlayerInputAxis.HorizontalLook => Input.GetAxisRaw("Mouse X"),
            PlayerInputAxis.VerticalLook => Input.GetAxisRaw("Mouse Y"),
            PlayerInputAxis.HorizontalMovement => Input.GetAxis("Horizontal"),
            PlayerInputAxis.VerticalMovement => Input.GetAxis("Vertical"),
            PlayerInputAxis.Running => Input.GetKey(KeyCode.LeftShift) ? 1f : 0f,
            PlayerInputAxis.SwitchWeaponUp =>  Input.mouseScrollDelta.y > 0f ? 1f : 0f,
            PlayerInputAxis.SwitchWeaponDown => Input.mouseScrollDelta.y < 0f ? 1f : 0f,
            PlayerInputAxis.Reload => Input.GetKeyDown(KeyCode.R)? 1f : 0f,
            PlayerInputAxis.Jump => Input.GetKeyDown(KeyCode.Space)? 1f : 0f,
            _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
        };
    }
}