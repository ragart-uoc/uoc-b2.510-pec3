using System;
using UnityEngine;

namespace B2510.Entities.CharacterStates
{
    public class AttackingState : ICharacterState
    {
        /// <value>Property <c>Character</c> represents the character.</value>
        private readonly Character _character;

        /// <summary>
        /// Class constructor <c>AttackingState</c> initializes the class.
        /// </summary>
        /// <param name="character">The character.</param>
        public AttackingState(Character character)
        {
            _character = character;
        }

        /// <summary>
        /// Method <c>StartState</c> invokes the state Start method.
        /// </summary>
        public void StartState()
        {
            _character.SetAnimatorSpeed(0f);
            _character.Attack();
        }

        /// <summary>
        /// Method <c>UpdateState</c> invokes the state Update method.
        /// </summary>
        public void UpdateState()
        {
        }

        /// <summary>
        /// Method <c>HandleEvents</c> invokes the state HandleEvents method.
        /// <param name="characterEvent">The character event.</param>
        /// </summary>
        public void HandleEvents(CharacterProperties.Events characterEvent)
        {
            switch (characterEvent)
            {
                case CharacterProperties.Events.AttackPerformed:
                    if (_character.collidingCharacter == null)
                        return;
                    _character.audioSource.PlayOneShot(_character.AudioClips["attack"]);
                    _character.collidingCharacter.Hit();
                    break;
                case CharacterProperties.Events.AttackFinished:
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
                case CharacterProperties.Events.DuckFinished:
                case CharacterProperties.Events.AttackStarted:
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