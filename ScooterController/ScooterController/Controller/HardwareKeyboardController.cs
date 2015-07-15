using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScooterController.HardwareAbstractionLayer;

namespace ScooterController.Controller
{
    class HardwareKeyboardController : HardwareBaseController
    {
        public List<KeyCode> GetPressedKeys()
        {
            var pressedKeys = new List<KeyCode>();
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (NativeKeyboard.IsKeyDown(key))
                {
                    pressedKeys.Add(key);
                };
            }

            return pressedKeys;
        }
    }
}
