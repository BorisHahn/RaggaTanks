﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_sSnake.shared
{
    public class ShowTextState : BaseGameState
    {
        public string Text { get; set; }

        private readonly float _duration = 1f;
        private float _timeLeft = 0f;

        public ShowTextState(float duration) : this(string.Empty, duration) { }

        public ShowTextState(string text, float duration)
        {
            this.Text = text;
            _duration = duration;

            Reset();
        }

        public override void Draw(ConsoleRenderer renderer)
        {
            var textHalfLength = Text.Length / 2;
            var textY = renderer.Height / 2;
            var textX = renderer.Width / 2 - textHalfLength;
            renderer.DrawString(Text, textX, textY, ConsoleColor.White);
        }

        public override void Reset()
        {
            _timeLeft = _duration;
        }

        public override void Update(float deltaTime)
        {
            _timeLeft -= deltaTime;
        }

        public override bool IsDone()
        {
            return _timeLeft <= 0f;
        }
    }

}
