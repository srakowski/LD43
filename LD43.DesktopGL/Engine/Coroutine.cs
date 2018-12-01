using Microsoft.Xna.Framework;
using System;
using System.Collections;

namespace LD43.Engine
{
    public class Coroutine
    {
        private IEnumerator _routine;

        private WaitYieldInstruction _wait;

        /// <summary>
        /// Has the Coroutine run to completion?
        /// </summary>
        public bool IsComplete { get; private set; } = false;

        internal Coroutine(IEnumerator routine)
        {
            _routine = routine;
        }

        // TODO: should be able to start again, maybe that is a Pause?
        public void Stop()
        {
            IsComplete = true;
        }

        internal void Update(GameTime gameTime)
        {
            if (IsComplete)
                return;

            if (_wait != null)
            {
                _wait.Update(gameTime);
                if (!_wait.IsOver)
                    return;

                _wait = null;
            }

            if (!_routine.MoveNext())
            {
                IsComplete = true;
            }

            _wait = _routine.Current as WaitYieldInstruction;
        }
    }

    public abstract class YieldInstruction { }

    public class WaitYieldInstruction : YieldInstruction
    {
        private double _timeRemaining = 0;

        internal bool IsOver => _timeRemaining <= 0.0;

        private WaitYieldInstruction(float millesecondsToWait)
        {
            _timeRemaining = millesecondsToWait;
        }

        public void Update(GameTime gameTime) =>
            _timeRemaining -= gameTime.ElapsedGameTime.TotalMilliseconds;

        /// <summary>
        /// Creates a WaitYieldInstruction for the prescribed number of milleseconds.
        /// </summary>
        /// <param name="milleseconds"></param>
        /// <returns></returns>
        public static WaitYieldInstruction Create(int milleseconds)
            => new WaitYieldInstruction((float)milleseconds);

        /// <summary>
        /// Creates a WaitYieldInstruction for the given Timespan.
        /// </summary>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public static WaitYieldInstruction Create(TimeSpan timespan)
            => new WaitYieldInstruction((float)timespan.TotalMilliseconds);
    }
}
