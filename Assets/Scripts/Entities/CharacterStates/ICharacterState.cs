namespace B2510.Entities.CharacterStates
{
    /// <summary>
    /// Interface <c>ICharacterState</c> is the interface for the character states.
    /// </summary>
    public interface ICharacterState
    {
        /// <summary>
        /// Method <c>StartState</c> invokes the state Start method.
        /// </summary>
        void StartState();
        
        /// <summary>
        /// Method <c>UpdateState</c> invokes the state Update method.
        /// </summary>
        void UpdateState();
        
        /// <summary>
        /// Method <c>HandleEvents</c> invokes the state HandleEvents method.
        /// <param name="characterEvent">The character event.</param>
        /// </summary>
        void HandleEvents(CharacterProperties.Events characterEvent);
    }
}