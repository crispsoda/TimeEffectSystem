### **WIP**

### To do:
* Cleanup & documentation
* Make timed effects using AnimationCurves blend with the next dominant effect’s time scale, rather than the set curve return value
* Design rationale

## Features
* Safely manipulate time from any script using TimeEffects thanks to priority-based stack
  * Can be timed or constant until manually disabled 
  * Use AnimationCurves for smooth timeScale animation in timed effects
* Started as Hitstop system for Honours Project, now expanded for all time manipulation of all lengths
* Attach TimeEffectCaller to GameObjects that can trigger time effects
  * Similar to Cinemachine’s Impulse system
* Requested TimeEffects are added to a list corresponding to an effect layer (i.e. Environment, Hitstop, Cinematics)
  * Layer order sets precedence for incoming effects
  * Automatically cleans up after expiration of effects
* Within each layer is another priority system to manage requested TimeEffects
  * Higher priority effects override lower ones
* Prevent TimeEffect spamming with max instance limits allowed at any given time
* TimeEffects can either be set inside Caller scripts, or use a saved Preset
  * Presets function like Unity Prefabs and instance TimeEffects using Presets that have their values differ from the preset will be marked as such, with a button to reset them to the preset’s values in Editor
