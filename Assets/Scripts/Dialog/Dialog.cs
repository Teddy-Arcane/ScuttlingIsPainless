using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Dialog
{
    [Serializable]
    public class Dialog
    {
        public bool endGame;
        public string name;

        [TextArea(1, 3)]
        public string[] sentences;
    }
}