public enum TimeEffectPriority { Base = 0, Low = 1, Normal = 2, High = 3, Critical = 4 }

public enum TimeEffectPresetID { None, Hitstop_S, Hitstop_M, Hitstop_L, Hitstop_XL, AimSlowdown }

public enum TimeEffectLayer 
{ 
    Cinematics,             //Like slowmotion in cutscenes
    Hitstop,                //Short hitstops for defeating enemies or bosses
    PlayerActions,          //Aiming slowdown 
    Environments,           //Certain level sections that might have a slower time
    Base                    //The base timescale, scene time
}