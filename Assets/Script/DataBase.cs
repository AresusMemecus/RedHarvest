using System;
using MySql.Data.MySqlClient;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    private string connectionString = "Server=127.0.0.1;Database=RedHarvest;Uid=unityuser;Pwd=unitypass;SslMode=none;";

    void Start()
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                Debug.Log("✅ Успешное подключение к MySQL!");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Ошибка подключения к MySQL: " + ex.Message);
        }
    }
}
