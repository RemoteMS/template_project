using System.Collections.Generic;
using System.Linq;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Extensions;
using UnityEngine;

namespace DI
{
    public class Greeter : MonoBehaviour
    {
        [Inject] private readonly IEnumerable<string> _strings;
        [Inject] private readonly IEnumerable<ScenesTestSSS> _datas;

        private void Start()
        {
            Debug.Log(string.Join(" ", _strings));
            Debug.Log(_datas.Count());

            if (_datas == null)
            {
                Debug.Log("null");
            }
            else
            {
                foreach (var scene in _datas)
                    Debug.Log($"Scene: {scene.MainScene}");
            }

            var container = gameObject.scene.GetSceneContainer();
            var service = container.Resolve<IMyService>();


            var c = new ContainerBuilder();
            c.SetName(" Test Greeter Container");
            c.AddSingleton(_ => { return new BigMyService(); }, typeof(BigMyService));
            c.SetParent(container);
            var build = c.Build();
            build.Construct<BigMyService>();


            Debug.Log($"build container: {build.Name}, {build.ToString()}");
            // Debug.Log($"container: {container.Scope().Name}");
            Debug.Log($"service: {service.Data}");
            Debug.Log($"service 2: {build.Resolve<BigMyService>().Data}");
        }
    }
}

public class BigMyService
{
    public string Data { get; set; } = "sdasdasdasdasdasdasd";
}