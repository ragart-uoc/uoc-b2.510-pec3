using System;

namespace B2510.Entities.CharacterStates
{
    public class DuckingState : ICharacterState
    {
        /// <value>Property <c>Character</c> represents the character.</value>
        private readonly Character _character;

        /// <summary>
        /// Class constructor <c>DuckingState</c> initializes the class.
        /// </summary>
        /// <param name="character">The character.</param>
        public DuckingState(Character character)
        {
            _character = character;
        }

        /// <summary>
        /// Method <c>StartState</c> invokes the state Start method.
        /// </summary>
        public void StartState()
        {
            _character.Duck(true);
        }

        /// <summary>
        /// Method <c>UpdateState</c> invokes the state Update method.
        /// </summary>
        public void UpdateState()
        {
            if (_character.moveInput.y >= 0)
            {
                _character.CurrentState.HandleEvents(CharacterProperties.Events.DuckFinished);
            }
            _character.Move();
        }

        /// <summary>
        /// Method <c>HandleEvents</c> invokes the state HandleEvents method.
        /// <param name="characterEvent">The character event.</param>
        /// </summary>
        public void HandleEvents(CharacterProperties.Events characterEvent)
        {
            switch (characterEvent)
            {
                case CharacterProperties.Events.DuckFinished:
                    _character.Duck(false);
                    _character.ChangeState(CharacterProperties.States.IdleMove);
                    break;
                case CharacterProperties.Events.HitStarted:
                    _character.ChangeState(CharacterProperties.States.Hit);
                    break;
                case CharacterProperties.Events.DeadStarted:
                    _character.ChangeState(CharacterProperties.States.Dead);
                    break;
                case CharacterProperties.Events.JumpStarted:
                case CharacterProperties.Events.JumpFinished:
                case CharacterProperties.Events.DuckStarted:
                case CharacterProperties.Events.AttackStarted:
                case CharacterProperties.Events.AttackPerformed:
                case CharacterProperties.Events.AttackFinished:
                case CharacterProperties.Events.DefendStarted:
                case CharacterProperties.Events.DefendFinished:
                case CharacterProperties.Events.HitFinished:
                case CharacterProperties.Events.DeadFinished:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(characterEvent), characterEvent, null);
            }
        }
    }
}