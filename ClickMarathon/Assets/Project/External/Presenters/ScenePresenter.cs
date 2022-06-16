// version 6.4.2022

namespace UnityEngine.SceneManagement
{
     [DisallowMultipleComponent]
     public abstract class ScenePresenter: MonoBehaviour
     {
          protected void Start()
          {
               EnteringScene();
          }

          protected void OnApplicationFocus(bool focus)
          {
               if(focus)
                    OnApplicationAcquiredFocus();
               else
                    OnApplicationLostFocus();
          }

          protected void SwitchScene(SceneName nextScene)
          {
               LeavingScene();
               SceneManager.LoadScene(nextScene.AsString(), LoadSceneMode.Single);
          }

          /// <summary>
          /// auto-invoked at base.Start().
          /// </summary>
          protected abstract void EnteringScene();
          protected virtual void OnApplicationAcquiredFocus() { }
          protected virtual void OnApplicationLostFocus() { }
          protected virtual void LeavingScene() { }
     }
}
