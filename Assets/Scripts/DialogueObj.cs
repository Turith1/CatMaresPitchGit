using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialiogue/DialogueObject")]
public class DialogueObj : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogue;

    public string[] Dialogue => dialogue;
}
