using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ShaderControl
{
    public class Dissolve : MonoBehaviour
    {
        Material material;

        public bool isDissolving;
        public bool isUnDissolving;
        public float fade = 1f;

        bool hasDash;
        bool hasThrusters;
        bool hasSprint;
        bool hasImpulse;
        bool hasMovement;

        public bool isPlayer;

        Rigidbody2D rbAtDeath;
        float gravityAtDeath;

        public void DoDissolve()
        {
            if(isPlayer)
                TurnShitOff();

            isDissolving = true;
        }

        public void RevertDissolve()
        {
            if (isPlayer)
                TurnShitOn();

            isUnDissolving = true;
        }

        private void TurnShitOff()
        {
            hasDash = GetComponent<Dash>().enabled;
            hasThrusters = GetComponent<ZeroGravityMovement>().enabled;
            hasSprint = GetComponent<Sprint>().enabled;
            hasImpulse = GetComponent<ImpulseJump>().enabled;
            hasMovement = GetComponent<Movement>().enabled;

            rbAtDeath = GetComponent<Rigidbody2D>();

            GetComponent<Movement>().enabled = false;
            GetComponent<ImpulseJump>().enabled = false;
            GetComponent<Dash>().enabled = false;
            GetComponent<ZeroGravityMovement>().enabled = false;
            GetComponent<Sprint>().enabled = false;
        }

        private void TurnShitOn()
        {
            GetComponent<Movement>().enabled = hasMovement;
            GetComponent<ImpulseJump>().enabled = hasImpulse;
            GetComponent<Dash>().enabled = hasDash;
            GetComponent<ZeroGravityMovement>().enabled = hasThrusters;
            GetComponent<Sprint>().enabled = hasSprint;

            if (hasThrusters)
                rbAtDeath.gravityScale = 0;
            else
                rbAtDeath.gravityScale = 3;
        }

        private void Start()
        {
            material = GetComponent<SpriteRenderer>().material;   
        }

        private void Update()
        {
            if (isDissolving)
            {
                fade -= Time.deltaTime;

                if(fade <= 0f)
                {
                    fade = 0f;
                    isDissolving=false;
                }

                material.SetFloat("_Fade", fade);
            }

            if (isUnDissolving)
            {
                fade += Time.deltaTime;

                if (fade >= 1f)
                {
                    fade = 1f;
                    isUnDissolving = false;
                }

                material.SetFloat("_Fade", fade);
            }
        }
    }
}
