using System;

namespace B2510.Entities.CharacterStates
{
    public class IdleMoveState : ICharacterState
    {
        /// <value>Property <c>Character</c> represents the character.</value>
        private readonly Character _character;

        /// <summary>
        /// Class constructor <c>IdleMoveState</c> initializes the class.
        /// </summary>
        /// <param name="character">The character.</param>
        public IdleMoveState(Character character)
        {
            _character = character;
        }

        /// <summary>
        /// Method <c>StartState</c> invokes the state Start method.
        /// </summary>
        public void StartState()
        {
        }

        /// <summary>
        /// Method <c>UpdateState</c> invokes the state Update method.
        /// </summary>
        public void UpdateState()
        {
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
                case CharacterProperties.Events.JumpStarted:
                    _character.ChangeState(CharacterProperties.States.Jumping);
                    break;
                case CharacterProperties.Events.DuckStarted:
                    _character.ChangeState(CharacterProperties.States.Ducking);
                    break;
                case CharacterProperties.Events.AttackStarted:
                    _character.ChangeState(CharacterProperties.States.Attacking);
                    break;
                case CharacterProperties.Events.DefendStarted:
                    _character.ChangeState(CharacterProperties.States.Defending);
                    break;
                case CharacterProperties.Events.HitStarted:
                    _character.ChangeState(CharacterProperties.States.Hit);
                    break;
                case CharacterProperties.Events.DeadStarted:
                    _character.ChangeState(CharacterProperties.States.Dead);
                    break;
                case CharacterProperties.Events.JumpFinished:
                case CharacterProperties.Events.DuckFinished:
                case CharacterProperties.Events.AttackPerformed:
                case CharacterProperties.Events.AttackFinished:
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