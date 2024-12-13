using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlayFiles.Logic
{
    internal class ScreenLogic
    {
        // Import the SetThreadExecutionState method from kernel32.dll
        [DllImport("kernel32.dll")]
        private static extern uint SetThreadExecutionState(uint esFlags);

        // Flags for SetThreadExecutionState
        private const uint ES_CONTINUOUS = 0x80000000;
        private const uint ES_SYSTEM_REQUIRED = 0x00000001;
        private const uint ES_DISPLAY_REQUIRED = 0x00000002;

        /// <summary>
        /// Enables "always-on" mode to prevent the PC from entering sleep or turning off the display.
        /// </summary>
        public static void EnableAlwaysOnMode()
        {
            uint result = SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
            if (result == 0)
            {
                throw new InvalidOperationException("Failed to set execution state.");
            }

            Console.WriteLine("Power save mode disabled. Screen will stay on.");
        }

        /// <summary>
        /// Restores normal power-saving behavior (allows screen to turn off and PC to sleep as per settings).
        /// </summary>
        public static void DisableAlwaysOnMode()
        {
            uint result = SetThreadExecutionState(ES_CONTINUOUS);
            if (result == 0)
            {
                throw new InvalidOperationException("Failed to reset execution state.");
            }

            Console.WriteLine("Power save mode restored. Screen will turn off as per system settings.");
        }
    }
}
