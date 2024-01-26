namespace B2510.Entities
{
    /// <summary>
    /// Class <c>CharacterProperties</c> represents the properties of the character.
    /// </summary>
    public static class CharacterProperties
    {
        /// <value>Property <c>States</c> represents the states of characters.</value>
        public enum States
        {
            Default,
            IdleMove,
            Jumping,
            Ducking,
            Attacking,
            Defending,
            Hit,
            Dead
        }
        
        /// <value>Property <c>Events</c> represents the events of characters.</value>
        public enum Events
        {
            JumpStarted,
            JumpFinished,
            DuckStarted,
            DuckFinished,
            AttackStarted,
            AttackPerformed,
            AttackFinished,
            DefendStarted,
            DefendFinished,
            HitStarted,
            HitFinished,
            DeadStarted,
            DeadFinished
        }
    }
}
