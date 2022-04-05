using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.NameGenerator
{
    public class NameGenerator: MonoBehaviour
    {
        public static NameGenerator instance;

        private List<string> names = new List<string>();

        private System.Random random;

        public bool isShowingName = false;

        public float killRate = 3f;

        public int KillCount = 0;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            random = new System.Random();

            ReadNames();
        }

        public void KillFaster()
        {
            killRate = killRate * 0.5f;
        }

        public void StartKilling()
        {
            Kill();
        }

        private void Kill()
        {
            KillCount++;

            if (PromptCanvasController.instance.canvas.activeInHierarchy && !isShowingName)
            {
                Invoke("Kill", killRate);
                return;
            }
                

            PromptCanvasController.instance.SetText(GenerateName(), true);
            PromptCanvasController.instance.ToggleCanvas(true);

            isShowingName = true;

            Invoke("Clear", killRate);
        }

        private void Clear()
        {
            if (PromptCanvasController.instance.canvas.activeInHierarchy && !isShowingName)
            {
                Invoke("Kill", killRate);
                return;
            }

            PromptCanvasController.instance.ToggleCanvas(false);

            isShowingName = false;

            Invoke("Kill", killRate);
        }

        private void ReadNames() 
        {
            var text = Resources.Load<TextAsset>("names").ToString();
            names = text.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            ).ToList();
        }

        private string GenerateName()
        {
            return $"{GetRandomName()} {GetRandomName()}, age {GetRandomAge()} has expired.";
        }

        private int GetRandomAge()
        {
            return random.Next(18, 65);
        }

        private string GetRandomName()
        {
            int index = random.Next(names.Count);
            return names[index];
        }
    }
}
