using UnityEditor;
using UnityEngine;
using static UnityEditor.SceneManagement.EditorSceneManager;

public class MenuTest: MonoBehaviour
{
     [MenuItem("<Open a Scene>/Connecting Scene")]
     static void OpenConnectingScene()
     {
          var canLeave = SaveCurrentModifiedScenesIfUserWantsTo();
          if(canLeave == false)
               return;

          OpenScene("Assets/Scenes/ConnectingScene.unity");
     }

     [MenuItem("<Open a Scene>/Playing Scene")]
     static void OpenPlayingScene()
     {
          var canLeave = SaveCurrentModifiedScenesIfUserWantsTo();
          if(canLeave == false)
               return;

          OpenScene("Assets/Scenes/PlayingScene.unity");
     }

}
