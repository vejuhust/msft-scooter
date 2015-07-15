﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterController.HardwareAbstractionLayer
{
    /// <summary>
    /// Codes representing keyboard keys.
    /// </summary>
    /// <remarks>
    /// Key code documentation:
    /// http://msdn.microsoft.com/en-us/library/dd375731%28v=VS.85%29.aspx
    /// </remarks>
    internal enum KeyCode : int
    {
        /// <summary>
        /// The alt key.
        /// </summary>
        Alt = 0x12,

        /// <summary>
        /// The spacebar key.
        /// </summary>
        SpaceBar = 0x20,

        /// <summary>
        /// The left arrow key.
        /// </summary>
        ArrowLeft = 0x25,

        /// <summary>
        /// The up arrow key.
        /// </summary>
        ArrowUp,

        /// <summary>
        /// The right arrow key.
        /// </summary>
        ArrowRight,

        /// <summary>
        /// The down arrow key.
        /// </summary>
        ArrowDown,

        /// <summary>
        /// The letter A key.
        /// </summary>
        LetterA = 0x41,

        /// <summary>
        /// The letter D key.
        /// </summary>
        LetterD = 0x44,

        /// <summary>
        /// The letter S key.
        /// </summary>
        LetterS = 0x53,

        /// <summary>
        /// The letter W key.
        /// </summary>
        LetterW = 0x57,
    }

    /// <summary>
    /// Provides keyboard access.
    /// </summary>
    internal static class NativeKeyboard
    {
        /// <summary>
        /// A positional bit flag indicating the part of a key state denoting
        /// key pressed.
        /// </summary>
        private const int KeyPressed = 0x8000;

        /// <summary>
        /// Returns a value indicating if a given key is pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>
        /// <c>true</c> if the key is pressed, otherwise <c>false</c>.
        /// </returns>
        public static bool IsKeyDown(KeyCode key)
        {
            return (GetKeyState((int)key) & KeyPressed) != 0;
        }

        /// <summary>
        /// Gets the key state of a key.
        /// </summary>
        /// <param name="key">Virtuak-key code for key.</param>
        /// <returns>The state of the key.</returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetKeyState(int key);
    }
}
