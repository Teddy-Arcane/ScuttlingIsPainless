using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Dialog
{
    public class DialogTrigger: MonoBehaviour
    {
        public Dialog dialog;

        public void TriggerDialog()
        {
            DialogManager.instance.StartDialog(dialog);
        }
    }
}
