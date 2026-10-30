using System.Runtime.InteropServices;
using UnityEngine;

public static class MyPlugin
{
    [DllImport("CactusPlugin")]
    private static extern int GetDamage();

    [DllImport("CactusPlugin")]
    private static extern void SetDamage(int baseDamage, int isBurstMode);

    public static int Damage
    {
        get
        {
            try { return GetDamage(); }
            catch { return 25; } // Bypasses the crash so bullets shoot!
        }
    }

    public static void UpdateWeaponDamage(int baseDamage, bool isBurstMode)
    {
        try { SetDamage(baseDamage, isBurstMode ? 1 : 0); }
        catch { /* Ignore the DLL error to keep the game running */ }
    }
}