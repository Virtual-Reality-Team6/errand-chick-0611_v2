using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
  public string transferMapName;
  private Player thePlayer;
  
  void Start()
  {
    if (thePlayer == null)
    thePlayer = FindObjectOfType<Player>();
    
    Debug.Log("시작된겨 뭐여");
  }
  
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.CompareTag("Player"))
    {
      thePlayer.currentMapName = transferMapName;
      SceneManager.LoadScene(transferMapName);
      Debug.Log("시방 뭐여");
    }
  }
}