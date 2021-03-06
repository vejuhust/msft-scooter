﻿using ScooterController.Configuration;
using ScooterController.HardwareAbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScooterController.Controller
{
    class HardwareKeyboardController : HardwareBaseController
    {
        private bool isEscapePressed = false;

        private readonly IEnumerable<KeyCode> allKeys;
        private readonly IEnumerable<KeyCode> actionKeys;

        private readonly Dictionary<int, bool> channelStatus = new Dictionary<int, bool>()
        {
            { HardwareSetting.ChannelMoveForward, false },
            { HardwareSetting.ChannelMoveBack, false },
            { HardwareSetting.ChannelTurnLeft, false },
            { HardwareSetting.ChannelTurnRight, false },
            { HardwareSetting.ChannelSpeedUp, false },
            { HardwareSetting.ChannelBrake, false },
        }; 

        private readonly Dictionary<KeyCode, int> keyChannelMapping = new Dictionary<KeyCode, int>()
        {
            { KeyCode.LetterW, HardwareSetting.ChannelMoveForward },
            { KeyCode.LetterS, HardwareSetting.ChannelMoveBack },
            { KeyCode.LetterA, HardwareSetting.ChannelTurnLeft },
            { KeyCode.LetterD, HardwareSetting.ChannelTurnRight },
            { KeyCode.Alt, HardwareSetting.ChannelSpeedUp },
            { KeyCode.SpaceBar, HardwareSetting.ChannelBrake },
        };

        public HardwareKeyboardController()
        {
            this.actionKeys = this.keyChannelMapping.Select(item => item.Key);
            this.allKeys = this.actionKeys.Concat(new List<KeyCode>() { KeyCode.Escape });
        }

        public void ExecuteKeyboardInstruction()
        {
            var keys = this.GetPressedKeys();

            if (keys.Contains(KeyCode.Escape))
            {
                this.CloseAllActionChannel();
                this.isEscapePressed = true;
            }
            else
            {
                foreach (var key in this.actionKeys)
                {
                    var targetStatus = keys.Contains(key);
                    var targetChannel = this.keyChannelMapping[key];
                    var currentStatus = this.channelStatus[targetChannel];

                    if (targetStatus != currentStatus)
                    {
                        if (targetStatus == true)
                        {
                            this.OpenChannel(targetChannel);

                            if (targetChannel == HardwareSetting.ChannelSpeedUp)
                            {
                                this.currentSpeed = (this.currentSpeed + 1) % 3;
                                this.currentSpeed += this.currentSpeed == 0 ? 3 : 0;
                            }
                        }
                        else
                        {
                            this.CloseChannel(targetChannel);
                        }

                        this.channelStatus[targetChannel] = targetStatus;
                    }
                }

                var statusLine = string.Format(
                    "[{0}]{1}{2}{3}{4}{5}",
                    this.currentSpeed,
                    this.channelStatus[HardwareSetting.ChannelBrake] ? "⃝" : " ",
                    this.channelStatus[HardwareSetting.ChannelTurnLeft] ? "←" : " ",
                    this.channelStatus[HardwareSetting.ChannelMoveForward] ? "↑" : " ",
                    this.channelStatus[HardwareSetting.ChannelMoveBack] ? "↓" : " ",
                    this.channelStatus[HardwareSetting.ChannelTurnRight] ? "→" : " ");

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(statusLine);
            }
        }

        public bool ShouldExit()
        {
            return this.isEscapePressed;
        }

        private void CloseAllActionChannel()
        {
            var activeChannels = this.channelStatus.Where(status => status.Value).Select(channel => channel.Key).ToList();
            foreach (var channel in activeChannels)
            {
                this.CloseChannel(channel);
                this.channelStatus[channel] = false;
            }
        }

        private List<KeyCode> GetPressedKeys()
        {
            var pressedKeys = new List<KeyCode>();
            foreach (var key in this.allKeys)
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
