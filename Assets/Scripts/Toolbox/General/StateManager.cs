using UnityEngine;

namespace Toolbox
{
    public class StateManager : Manager<StateManager>
    {
        State active;

        public State Active
        {
            get
            {
                return active;
            }

            set
            {
                if (active != null)
                {
                    active.StateEnd();
                }

                active = value;

                if (active != null)
                {
                    active.StateStart();
                }
            }
        }

        void Update()
        {
            if (active != null)
            {
                active.StateUpdate();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                active = Utils.GetComponentAtMouse3D<State>();
            }
        }

        public void Clear(State s)
        {
            if (active == s)
            {
                active = null;
            }
        }
    }
}