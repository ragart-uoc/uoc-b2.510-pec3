using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using B2510.Entities.CharacterStates;

namespace B2510.Entities
{
    /// <summary>
    /// Class <c>Character</c> contains the input and movement logic for the character.
    /// </summary>
    public class Character : MonoBehaviour
    {
        #region Character States
            
            /// <value>Property <c>characterState</c> represents the character state.</value>
            public CharacterProperties.States characterState;
                
            /// <value>Property <c>_characterStates</c> represents the list of character states.</value>
            private readonly Dictionary<CharacterProperties.States, ICharacterState> _characterStates = new Dictionary<CharacterProperties.States, ICharacterState>();
                    
            /// <value>Property <c>CurrentState</c> represents the current character state.</value>
            public ICharacterState CurrentState;
            
        #endregion
        
        #region Component References
        
            /// <value>Property <c>playerInput</c> is used to reference the player input component.</value>
            public PlayerInput playerInput;

            /// <value>Property <c>animator</c> is used to reference the animator component.</value>
            public Animator animator;
            
        #endregion

        #region Character Attributes
        
            /// <value>Property <c>characterName</c> is used to reference the character name.</value>
            public string characterName;
        
            /// <value>Property <c>maxHealthPoints</c> is used to reference the maximum health points.</value>
            public float maxHealthPoints = 10f; 

            /// <value>Property <c>hitPoints</c> is used to reference the hit points.</value>
            public float healthPoints = 10f;
            
            /// <value>Property <c>moveSpeed</c> is used to reference the movement speed.</value>
            [SerializeField]
            private float moveSpeed = 5f;
            
            /// <value>Property <c>damagePerHit</c> is used to reference the damage that the character takes per hit.</value>
            [SerializeField]
            private float damagePerHit = 1f;
            
            /// <value>Property <c>_initialPosition</c> is used to reference the initial position of the character.</value>
            private Vector3 _initialPosition;
        
        #endregion
        
        #region Movement

            /// <value>Property <c>moveInput</c> is used to reference the movement input.</value>
            [HideInInspector]
            public Vector2 moveInput;
            
        #endregion
        
        #region Collisions

            /// <value>Property <c>collidingCharacter</c> is used to reference the other character that is colliding with this character.</value>
            [HideInInspector]
            public Character collidingCharacter;
        
        #endregion
        
        #region Animator References
            
            /// <value>Property <c>_animatorSpeed</c> is used to reference the speed parameter.</value>
            private readonly int _animatorSpeed = Animator.StringToHash("Speed");
            
            /// <value>Property <c>_animatorAttack</c> is used to reference the attack parameter.</value>
            private readonly int _animatorAttack = Animator.StringToHash("Attack");
            
            /// <value>Property <c>_animatorDefending</c> is used to reference the defending parameter.</value>
            private readonly int _animatorDefending = Animator.StringToHash("Defending");
            
            /// <value>Property <c>_animatorJump</c> is used to reference the jump parameter.</value>
            private readonly int _animatorJump = Animator.StringToHash("Jump");
            
            /// <value>Property <c>_animatorDucking</c> is used to reference the ducking parameter.</value>
            private readonly int _animatorDucking = Animator.StringToHash("Ducking");
            
            /// <value>Property <c>_animatorHit</c> is used to reference the hit parameter.</value>
            private readonly int _animatorHit = Animator.StringToHash("Hit");
            
            /// <value>Property <c>_animatorDead</c> is used to reference the dead parameter.</value>
            private readonly int _animatorDead = Animator.StringToHash("Dead");
        
        #endregion
        
        #region UI References

            /// <value>Property <c>healthBar</c> is used to reference the health bar.</value>
            public Image healthBar;
            
        #endregion
        
        #region Audio References
        
            /// <value>Property <c>audioSource</c> represents the audio source.</value>
            public AudioSource audioSource;
            
            /// <value>Property <c>AudioClips</c> represents a dictionary containing all sounds and music for this object.</value>
            public Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();
        
        #endregion

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Set the initial position
            _initialPosition = transform.position;

            // Get the components
            playerInput = (playerInput == null) ? GetComponent<PlayerInput>() : playerInput;
            animator = (animator == null) ? GetComponent<Animator>() : animator;
            audioSource = (audioSource == null) ? GetComponent<AudioSource>() : audioSource;
            
            // Get the audio clips
            var audioClips = Resources.LoadAll<AudioClip>("Sounds/Character");
            foreach (var audioClip in audioClips)
            {
                Debug.Log(audioClip.name);
                AudioClips.Add(audioClip.name, audioClip);
            }

            // Initialize the character states
            _characterStates.Add(CharacterProperties.States.Default, new DefaultState(this));
            _characterStates.Add(CharacterProperties.States.IdleMove, new IdleMoveState(this));
            _characterStates.Add(CharacterProperties.States.Jumping, new JumpingState(this));
            _characterStates.Add(CharacterProperties.States.Ducking, new DuckingState(this));
            _characterStates.Add(CharacterProperties.States.Attacking, new AttackingState(this));
            _characterStates.Add(CharacterProperties.States.Defending, new DefendingState(this));
            _characterStates.Add(CharacterProperties.States.Hit, new HitState(this));
            _characterStates.Add(CharacterProperties.States.Dead, new DeadState(this));
            
            // Set the current state
            ChangeState(characterState);
        }

        /// <summary>
        /// Method <c>Start</c> is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            CurrentState.StartState();
        }
        
        /// <summary>
        /// Method <c>Update</c> is called every frame, if the MonoBehaviour is enabled
        /// </summary>
        private void Update()
        {
            CurrentState.UpdateState();
        }
        
        #region Actions
        
            /// <summary>
            /// Method <c>Move</c> is used to move the character.
            /// </summary>
            public void Move()
            {
                transform.position += new Vector3(moveInput.x, 0, 0) * (moveSpeed * Time.deltaTime);
                SetAnimatorSpeed(moveInput.x);
            }
            
            /// <summary>
            /// Method <c>Jump</c> is used to make the character jump.
            /// </summary>
            public void Jump()
            {
                animator.SetTrigger(_animatorJump);
            }
            
            /// <summary>
            /// Method <c>Duck</c> is used to make the character duck.
            /// </summary>
            public void Duck(bool isDucking)
            {
                animator.SetBool(_animatorDucking, isDucking);
            }
            
            /// <summary>
            /// Method <c>Attack</c> is used to make the character attack.
            /// </summary>
            public void Attack()
            {
                animator.SetTrigger(_animatorAttack);
            }
            
            /// <summary>
            /// Method <c>Defend</c> is used to make the character defend.
            /// </summary>
            public void Defend(bool isDefending)
            {
                animator.SetBool(_animatorDefending, isDefending);
            }
            
            /// <summary>
            /// Method <c>GetHit</c> is used to make the character get hit.
            /// </summary>
            public void GetHit()
            {
                animator.SetTrigger(_animatorHit);
                audioSource.PlayOneShot(AudioClips["hit"]);
                healthPoints -= damagePerHit;
                healthBar.fillAmount = healthPoints / maxHealthPoints;
            }

            /// <summary>
            /// Method <c>Die</c> is used to make the character die.
            /// </summary>
            public void Die()
            {
                animator.SetBool(_animatorDead, true);
            }

            #endregion
        
        #region External Actions
        
            /// <summary>
            /// Method <c>Hit</c> allows other objects to hit the character.
            /// </summary>
            public void Hit()
            {
                CurrentState.HandleEvents(CharacterProperties.Events.HitStarted);
            }
        
            /// <summary>
            /// Method <c>ChangeState</c> is used to change the current state.
            /// </summary>
            /// <param name="newState"></param>
            public void ChangeState(CharacterProperties.States newState)
            {
                characterState = newState;
                CurrentState = _characterStates[newState];
                CurrentState.StartState();
            }
            
            /// <summary>
            /// Method <c>SetAnimatorSpeed</c> is used to set the animator speed.
            /// </summary>
            /// <param name="amount">The speed.</param>
            public void SetAnimatorSpeed(float amount)
            {
                animator.SetFloat(_animatorSpeed, Mathf.Abs(amount));
            }
            
            /// <summary>
            /// Method <c>EnableMovement</c> is used to enable/disable the character's movement.
            /// </summary>
            /// <param name="isEnabled">The boolean value to enable/disable the movement.</param>
            public void EnableMovement(bool isEnabled)
            {
                playerInput.enabled = isEnabled;
            }

            /// <summary>
            /// Method <c>Reset</c> is used to reset the character.
            /// </summary>
            public void Reset()
            {
                // Reset state
                ChangeState(CharacterProperties.States.Default);

                // Reset position
                transform.position = _initialPosition;
                
                // Reset health points
                healthPoints = maxHealthPoints;
                
                // Reset animator
                animator.SetFloat(_animatorSpeed, 0);
                animator.SetBool(_animatorDucking, false);
                animator.SetBool(_animatorDefending, false);
                animator.SetBool(_animatorDead, false);
            }
        
        #endregion
        
        #region Input Callbacks

            /// <summary>
            /// Method <c>OnMovement</c> is called when the character inputs the movement keys.
            /// </summary>
            /// <param name="value">The value of the input.</param>
            private void OnMovement(InputValue value)
            {
                moveInput = value.Get<Vector2>();
                switch (moveInput.y)
                {
                    case > 0:
                        CurrentState.HandleEvents(CharacterProperties.Events.JumpStarted);
                        break;
                    case < 0:
                        CurrentState.HandleEvents(CharacterProperties.Events.DuckStarted);
                        break;
                }
            }
            
            /// <summary>
            /// Method <c>OnAttack</c> is called when the character inputs the attack keys.
            /// </summary>
            /// <param name="value">The value of the input.</param>
            private void OnAttack(InputValue value)
            {
                CurrentState.HandleEvents(CharacterProperties.Events.AttackStarted);
            }
            
            /// <summary>
            /// Method <c>OnDefend</c> is called when the character inputs the defend keys.
            /// </summary>
            /// <param name="value">The value of the input.</param>
            private void OnDefend(InputValue value)
            {
                switch (value.isPressed)
                {
                    case true:
                        CurrentState.HandleEvents(CharacterProperties.Events.DefendStarted);
                        break;
                    case false:
                        CurrentState.HandleEvents(CharacterProperties.Events.DefendFinished);
                        break;
                }
            }
        
        #endregion
        
        #region Animator Callbacks
        
            /// <summary>
            /// Method <c>AnimationJumpFinished</c> is called when the jump animation is finished.
            /// </summary>
            public void AnimationJumpFinished()
            {
                CurrentState.HandleEvents(CharacterProperties.Events.JumpFinished);
            }
            
            /// <summary>
            /// Method <c>AnimationAttackPerformed</c> is called when the attack animation is performed.
            /// </summary>
            public void AnimationAttackPerformed() 
            {
                CurrentState.HandleEvents(CharacterProperties.Events.AttackPerformed);
            }
            
            /// <summary>
            /// Method <c>AnimationAttackFinished</c> is called when the attack animation is finished.
            /// </summary>
            public void AnimationAttackFinished() 
            {
                CurrentState.HandleEvents(CharacterProperties.Events.AttackFinished);
            }
            
            /// <summary>
            /// Method <c>AnimationDefendPerformed</c> is called when the defend animation is performed.
            /// </summary>
            public void AnimationHitFinished() 
            {
                CurrentState.HandleEvents(CharacterProperties.Events.HitFinished);
            }
        
        #endregion
        
        #region Collisions

            /// <summary>
            /// Method <c>OnCollisionEnter</c> is called when this collider/rigidbody has begun touching another rigidbody/collider.
            /// </summary>
            /// <param name="col">The collision data associated with this collision.</param>
            private void OnCollisionEnter(Collision col)
            {
                if (col.gameObject.CompareTag("Player")
                    && col.gameObject != gameObject)
                {
                    collidingCharacter = col.gameObject.GetComponent<Character>();
                }
            }

            /// <summary>
            /// Method <c>OnCollisionEnter</c> is called when this collider/rigidbody has stopped touching another rigidbody/collider.
            /// </summary>
            /// <param name="col">The collision data associated with this collision.</param>
            private void OnCollisionExit(Collision col)
            {
                if (col.gameObject.CompareTag("Player")
                    && col.gameObject != gameObject)
                {
                    collidingCharacter = null;
                }
            }

        #endregion
    }
}
