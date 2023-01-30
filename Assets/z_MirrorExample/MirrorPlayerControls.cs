using Mirror;
using UnityEngine;

namespace MirrorExample
{
    public class MirrorPlayerControls : NetworkBehaviour
    {

        private void Update()
        {
            if (!isLocalPlayer)
                return;

            transform.Translate(
                x: Input.GetAxis("Horizontal") * Time.deltaTime,
                y: 0f,
                z: Input.GetAxis("Vertical") * Time.deltaTime
            );
        }
    }
}