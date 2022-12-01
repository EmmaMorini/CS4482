using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{

    public static void ResetStats() {
        PlayerPrefs.DeleteKey("MAX_HEALTH");
        PlayerPrefs.DeleteKey("MOVE_SPEED");
        PlayerPrefs.DeleteKey("DAMAGE");
        PlayerPrefs.DeleteKey("BUFF_MAX_HEALTH");
        PlayerPrefs.DeleteKey("BUFF_MOVE_SPEED");
        PlayerPrefs.DeleteKey("BUFF_DAMAGE");
    }
    public static float BaseMaxHealth {
        get {
            return PlayerPrefs.GetFloat("MAX_HEALTH", 10);
        }

        set{
            PlayerPrefs.SetFloat("MAX_HEALTH", value);
        }
    }

    public static float BaseMoveSpeed {
        get {
            return PlayerPrefs.GetFloat("MOVE_SPEED", 10);
        }

        set{
            PlayerPrefs.SetFloat("MOVE_SPEED", value);
        }
    }

    public static float BaseDamage {
        get {
            return PlayerPrefs.GetFloat("DAMAGE", 10);
        }

        set{
            PlayerPrefs.SetFloat("DAMAGE", value);
        }
    }

    public static float Damage {
        get{
            return PlayerStats.BaseDamage * PlayerStats.Buffs.Damage;
        }
    }

    public static float MoveSpeed {
        get{
            return PlayerStats.BaseMoveSpeed * PlayerStats.Buffs.MoveSpeed;
        }
    }

    public static float MaxHealth {
        get {
            return PlayerStats.BaseMaxHealth * PlayerStats.Buffs.MaxHealth;
        }
    }

    public static class Buffs {
        public static float MaxHealth {
            get {
                return PlayerPrefs.GetFloat("BUFF_MAX_HEALTH", 1);
            }

            set{
                PlayerPrefs.SetFloat("BUFF_MAX_HEALTH", value);
            }
        }

        public static float MoveSpeed {
            get {
                return PlayerPrefs.GetFloat("BUFF_MOVE_SPEED", 1);
            }

            set{
                PlayerPrefs.SetFloat("BUFF_MOVE_SPEED", value);
            }
        }

        public static float Damage {
            get {
                return PlayerPrefs.GetFloat("BUFF_DAMAGE", 1);
            }

            set{
                PlayerPrefs.SetFloat("BUFF_DAMAGE", value);
            }
        }
    }
}
