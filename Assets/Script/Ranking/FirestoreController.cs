using Firebase.Firestore;
using UnityEngine;
using Firebase.Extensions;

public class FirestoreController : MonoBehaviour
{
    void Start()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        db.Collection("Test").Document("Doc1").SetAsync(new { hello = "형님!" })
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                    Debug.Log("Firestore 업로드 성공!");
                else
                    Debug.LogError("Firestore 업로드 실패: " + task.Exception);
            });
    }
}
